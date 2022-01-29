using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseScript : MonoBehaviour
{

    public bool isPaused = false;
    public bool isSlowMotion = false;
    public float slowMotionTime = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SlowDownButton"))
        {
            toggleSlowMotion();
        }

        if (Input.GetButtonDown("Escape"))
        {
            togglePause();
            
        }
    }


    void togglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseGame();
        }
        else
        {
            resumeGame();
        }

        print("PAUSED: " + isPaused.ToString());
    }

    public void toggleSlowMotion()
    {
        isSlowMotion = !isSlowMotion;
        if (isSlowMotion)
        {
            slowMotionOn();
        }
        else
        {
            slowMotionOff();
        }
    }


    void pauseGame()
    {
        Time.timeScale = 0;
    }

    void resumeGame()
    {
        if (isSlowMotion == true)
        {
            slowMotionOn();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void slowMotionOn()
    {
        Time.timeScale = slowMotionTime;
    }

    void slowMotionOff()
    {
        Time.timeScale = 1;
    }

}
