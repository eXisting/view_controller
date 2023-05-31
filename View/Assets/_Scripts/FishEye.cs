using UnityEngine;

public class FishEye : MonoBehaviour
{
    // The strength of the fish eye effect
    [Range(0, 1)]
    public float strength = 0.5f;

    // The material to apply the fish eye effect to
    public Material material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Set the strength of the fish eye effect in the material
        material.SetFloat("_Strength", strength);

        // Apply the fish eye effect to the camera's image
        Graphics.Blit(source, destination, material);
    }
}
