using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation_Landscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
