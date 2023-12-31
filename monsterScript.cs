using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;

public class monsterScript : MonoBehaviour
{

    public bool spotted = false;
    public bool chase;
    public Transform player;

    public AudioSource jumpsource;
    public AudioClip audioclip;

    public AudioSource spikeSource;
    public AudioClip spikeClip;

    public AudioSource screamSource;
    public AudioClip screamClip;

    //makes monster visible in dark when spotted
    public Transform GameObject;

    //access the PlayerController script
    public PlayerController script;


    //makes it so that the jumpscare audio is only played once
    public int value = 0;

    public float time = 0f;

    public bool dead = false;

    //checks if the monster collides with a tile object
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        dead = true;
        if(collision.tag == "spike")
        {
            spikeSource.Play();
            screamSource.Play();
        } else
        {
            screamSource.Play();
        }
        

    }
    // Start is called before the first frame update
    void Start()
    {
        //changes gameobject size (flame on)
        GameObject.transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {

        if (time > 0f && spotted)
        {
            // Subtract the difference of the last time the method has been called
            time -= Time.deltaTime;
        }


        spotted = script.spotted;
        chase = script.flame;
        if (spotted == true && value == 0)
        {
            jumpsource.Play();
            value++;
        }

        if (!dead) 
        { 
            killSequence();
        } 
    }


    void killSequence()
    {

        if (chase == true && spotted == true && time <= 0)
        {
            //makes the eye of the monster visible in the dark
            GameObject.transform.localScale = new Vector3((float).12, (float).1, 1);

            //moves the monster across the x-axis first
            if (transform.position.x != player.transform.position.x )
            {
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                float moveSpeed = 1.3f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            } else
            //y-axis
            {
                Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
                float moveSpeed = 1.3f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }

        }
    }
}
