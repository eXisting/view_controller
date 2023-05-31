using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppLifecycleManager : MonoBehaviour
{
    public Image imageToActivate;

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            Debug.Log("App was closed");
        }
        else
        {
            Debug.Log("App was restarted");
            if (imageToActivate != null)
            {
                imageToActivate.gameObject.SetActive(true);
            }
        }
    }
}