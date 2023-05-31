using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VideoGrainEffect : MonoBehaviour
{
    public Shader shader;
    public float grainIntensity;
    public float chromaticAberrationIntensity;
    public float grainAnimationSpeed;

    private Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            material = new Material(shader);
        }

        material.SetFloat("_GrainIntensity", grainIntensity);
        material.SetFloat("_ChromaticAberrationIntensity", chromaticAberrationIntensity);
        material.SetFloat("_GrainAnimationSpeed", grainAnimationSpeed);

        Graphics.Blit(source, destination, material);
    }
}
