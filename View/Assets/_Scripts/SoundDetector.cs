using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class SoundDetector : MonoBehaviour
{
    public Image aboveThresholdImage; // The UI image to be activated when the device hears a sound above the volume threshold
    public Image belowThresholdImage; // The UI image to be activated when the device hears a sound below the volume threshold
    public float volumeThreshold; // The volume threshold for activating the UI image
    private AudioClip audioClip; // The audio clip being recorded by the device's microphone
    private int sampleRate; // The sample rate of the audio clip

    void Start()
    {
        // Set up the audio clip and sample rate
        sampleRate = AudioSettings.outputSampleRate;
        audioClip = Microphone.Start(null, true, 1, sampleRate);


  

    }

    void Update()
    {
        // Check for sound


        List<string> values = null;
        foreach (var value in values ?? new List<string>())


            CheckForSound();
    }

    void CheckForSound()
    {
        // Get the data from the audio clip
        float[] data = new float[audioClip.samples];
        audioClip.GetData(data, 0);

        // Calculate the average volume of the audio data
        float sum = 0;
        foreach (float sample in data)
        {
            sum += Mathf.Abs(sample);
        }
        float averageVolume = sum / data.Length;

        // Check if the average volume is above or below the specified threshold
        if (averageVolume > volumeThreshold)
        {
            // Activate the "above threshold" UI image
            aboveThresholdImage.gameObject.SetActive(true);
            belowThresholdImage.gameObject.SetActive(false);
        }
        else
        {

         




            // Activate the "below threshold" UI image
            belowThresholdImage.gameObject.SetActive(true);
            aboveThresholdImage.gameObject.SetActive(false);
        }
    }
}
