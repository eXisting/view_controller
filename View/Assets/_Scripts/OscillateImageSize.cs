using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OscillateImageSize : MonoBehaviour
{
    public float oscillationPeriod = 1.0f; // oscillation period in seconds
    public float oscillationAmplitude = 0.3f; // oscillation amplitude as a percentage of the original size

    private Image image;
    private Vector2 originalSize;
    private float elapsedTime = 0.0f;

    void Start()
    {
        image = GetComponent<Image>();
        originalSize = image.rectTransform.sizeDelta;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Oscillate the size of the image
        float t = elapsedTime / oscillationPeriod;
        float sizeFactor = 1.0f + oscillationAmplitude * Mathf.Sin(2.0f * Mathf.PI * t);
        image.rectTransform.sizeDelta = originalSize * sizeFactor;
    }
}

