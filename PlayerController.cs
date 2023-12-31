using Microsoft.Cci;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    public bool spotted = false;

    //coordiantes of mosnter and player
    public int playerX = 0;
    public int playerY = 0;
    public int MonsterX = 0;
    public int MonsterY = 6;

    //clip and sound effect for lighting fire.
    public AudioClip clip;
    public AudioSource source;

    public AudioClip Blowclip;
    public AudioSource Blowsource;

    //variable that shows the direction
    //the player is facing
    //0 == forward
    //1 == right
    //-1 == left
    //2 == back
    public int face = 0;

    //delay value
    public double time = .5;

    //sets speed of movement
    public float moveSpeed = 10f;

    //movePoint which the character travels to
    public Transform movePoint;

    //sprite mas that reveals game under Darkness
    public Transform GameObject;


    //target angle that player rotates
    public float Target;
    float r;

    public bool flame = true;

    public bool dead = false;

    public bool completeLevel = false;

    public AudioSource SpikeSource;
    public AudioClip SpikeClip;

    public int sceneBuiltIndex;



    //layer for colliders
    public LayerMask whatStopsMovement;




    //---------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

        movePoint.parent = null;
        source.Play();

    }


    //---------------------------------------------------------------------------------------------------------------------------------------------------------------
    //check if player collides with levelMove
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "levelMove")
        {
            //Player entered, so move level
            //print("Switching Scene to " + sceneBuiltIndex);
            //SceneManager.LoadScene(sceneBuiltIndex, LoadSceneMode.Single);

            completeLevel = true;

        }
        else
        {
            dead = true;
            SpikeSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Spacebar clicked!");
                if (flame)
                {
                    flame = false;
                    Blowsource.Play();

                } else
                {
                    flame = true;
                    source.Play();
                }
            }
            //sets delay value for clicking buttons
            if (time > 0f)
            {
                // Subtract the difference of the last time the method has been called
                time -= Time.deltaTime;
            }


            //every frame, game checks wether to turn off the flame
            if (flame == true)
            {
                //changes gameobject size (flame on)
                GameObject.transform.localScale = new Vector3((float).61, (float)1.2, 1);
            }
            else
            {
                //changes gameobject size (flame off)
                GameObject.transform.localScale = new Vector3((float).17, (float).35, 1);
            }

            //changes the rotation of the GameObject every frame.
            //only changes when ChangeAngle is called by the user input 
            float Angle = Mathf.SmoothDampAngle(GameObject.transform.eulerAngles.z, Target, ref r, 0.1f);
            GameObject.transform.rotation = Quaternion.Euler(0, 0, Angle);


            //updates the face value based on target
            if (Target == -90)
            {
                face = 1;
            }
            if (Target == 90)
            {
                face = -1;
            }
            if (Target == 0)
            {
                face = 0;
            }
            if (Target == 180)
            {
                face = 2;
            }



            playerMovement();
            isSpotted();
        }

    }
    
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------



    //make script that takes in the coordinates of the player and the monster
    //and decides wether the monster has been spotted and update variable.
    public void isSpotted()
    {
        //player on the right side of the monster
        if ((MonsterX == playerX - 1) && (MonsterY == playerY) && Target == 90 && flame == true)
        {
            spotted = true;
        }

        //player on the left side of the monster
        if ((MonsterX == playerX + 1) && (MonsterY == playerY) && Target == -90 && flame == true)
        {
            spotted = true;
        }

        //player under the monster
        if ((MonsterX == playerX) && (MonsterY == playerY + 1) && Target == 0 && flame == true)
        {
            spotted = true;
            
        }

        //player over the monster
        if ((MonsterX == playerX) && (MonsterY == playerY - 1) && Target == 180 && flame == true)
        {
            spotted = true;
            
        }

    }


    //function for the user to change the value of flame
    //gives user the ability to turn on/off the flame
    //updates next frame;
    public void ChangeFlame()
    {
        if (flame == true)
        {
            flame = false;
            Blowsource.Play();
        } else
        {
            flame = true;
            //play sound effect (lighting fire)
            source.Play();
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------


    void playerMovement()
    {

        //Moves the player to the movepoint at according to movespeed
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //if the player is at the Next Tile/MovePoint
        if (Vector3.Distance(transform.position, movePoint.position) <= 0f)
        {
            //Each if Statement checks for a user input in each direction
            //and moves the MovePoint accordingly

            //movment to the right
            if (Input.GetAxisRaw("Horizontal") == 1f && time <= 0)
            {
                

                    //based on face, game will decide what function the key has
                    if (face == 1)
                    {

                        //check if a collision is present
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3((float)2.4, 0f, 0f), .2f, whatStopsMovement))
                        {
                            //moves movepoint
                            movePoint.position += new Vector3((float)2.4, 0f, 0f);
                            playerX++;
                        }
                        
                    } else
                    {
                        //rotates FOV
                        Target = -90;
                        
                    }
                    //reset cooldown for input
                    time = .5;
     
            } 

            //Movement to the left
            else if (Input.GetAxisRaw("Horizontal") == -1f && time <= 0)
            {
                    if (face == -1)
                    {
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3((float)-2.4, 0f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3((float)-2.4, 0f, 0f);
                            playerX--;
                        }
                    }
                    else
                    {
                        Target = 90;

                    }
                    time = .5;
               
            }

            //Movement up
            else if (Input.GetAxisRaw("Vertical") == 1f && time <= 0) 
            {

                    if (face == 0)
                    {
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, (float)2.4, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(0f, (float)2.4, 0f);
                            playerY++;
                        }
                    }
                    else
                    {
                        Target = 0;

                    }
                    time = .5;
                   
            }
   
            //Movement dowm
            else if (Input.GetAxisRaw("Vertical") == -1f && time <= 0)
            {

                    if (face == 2)
                    {
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, (float)-2.4, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(0f, (float)-2.4, 0f);
                            playerY--;
                        }
                    }
                    else
                    {
                        Target = 180;

                    }
                    time = .5;
                   
            }

        }
    }
   
 } 

