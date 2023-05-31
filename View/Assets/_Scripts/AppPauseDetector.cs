using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppPauseDetector : MonoBehaviour
{
    public float timeThreshold; // The amount of time that needs to have passed before the first UI image is displayed
    public float timeThreshold2; // The amount of time that needs to have passed before the second UI image is displayed
    public GameObject firstUIImage; // The first UI image to be displayed
    public GameObject secondUIImage; // The second UI image to be displayed

    private float pauseStartTime; // The time at which the application was paused or closed

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            // The application has been paused, so record the current time
            pauseStartTime = Time.realtimeSinceStartup;
        }
        else
        {
            // The application has been resumed, so reset the pause start time
            pauseStartTime = 0;
        }
    }

    void OnApplicationQuit()
    {
        // The application has been closed, so record the current time
        pauseStartTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (pauseStartTime > 0)
        {
            // The application has been paused or closed, so check how much time has passed
            float elapsedTime = Time.realtimeSinceStartup - pauseStartTime;

            if (elapsedTime >= timeThreshold2)
            {
                // The elapsed time is greater than the second threshold, so display the second UI image
                secondUIImage.SetActive(true);
            }
            else if (elapsedTime >= timeThreshold)
            {
                // The elapsed time is greater than the first threshold, but not the second threshold, so display the first UI image
                firstUIImage.SetActive(true);
            }
        }
    }
}

