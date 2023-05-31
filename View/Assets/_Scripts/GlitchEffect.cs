using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchEffect : MonoBehaviour
{
    public Image image;
    public float glitchIntensity;
    public float glitchFrequency;

    private void Update()
    {
        if (Random.Range(0.0f, 1.0f) < glitchFrequency)
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, Random.Range(1.0f - glitchIntensity, 1.0f));
        }
    }
}
