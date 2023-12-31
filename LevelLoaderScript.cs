using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;

    public PlayerController script;

    public flameScript flameScript;

    public flameScriptTwo flameScriptTwo;

    public flameScriptThree flameScriptThree;

    public flameScriptFour flameScriptFour;

    public flameScriptFive flameScriptFive;



    // Update is called once per frame
    void Update()
    {

        
        if (SceneManager.GetActiveScene().buildIndex == 0 )
        {
            if (flameScript.completeLevel)
            {
                LoadNextLevel();
                
            }
            
        }
        //check if at level 2
        else if (SceneManager.GetActiveScene().buildIndex == 1) 
        {

            //check if level 1 is completed
            if (script.completeLevel)
            {
                LoadNextLevel();
                
            } else if(script.dead)
            {
                LoadPreviousLevel();
            }
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (flameScriptTwo.completeLevel)
            {
                LoadNextLevel();

            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {

           
            if (script.completeLevel)
            {
                LoadNextLevel();

            }
            else if (script.dead)
            {
                LoadPreviousLevel();
            }
        }
        else if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (flameScriptThree.completeLevel)
            {
                LoadNextLevel();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {

            
            if (script.completeLevel)
            {
                LoadNextLevel();

            }
            else if (script.dead)
            {
                LoadPreviousLevel();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            if (flameScriptFour.completeLevel)
            {
                LoadNextLevel();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7)
        {

            if (script.completeLevel)
            {
                LoadNextLevel();

            }
            else if (script.dead)
            {
                LoadPreviousLevel();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            if (flameScriptFive.completeLevel)
            {
                LoadNextLevel();
            }
        }
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));


    }

    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));


    }
    IEnumerator LoadLevel(int levelIndex)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait 
        yield return new WaitForSeconds(2);


        //Load scene
        SceneManager.LoadScene(levelIndex);

    }
}
