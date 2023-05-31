using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustEnvironmentLighting : MonoBehaviour
{
    public float intensityMultiplier = 1.0f; // Set this value in the Inspector

    void Start()
    {
        // Set the intensity multiplier for the environment lighting
        RenderSettings.ambientIntensity = intensityMultiplier;
    }
}

