using UnityEngine;
using UnityEngine.UI;

public class RaycastBlocker : MonoBehaviour
{
    // The UI image or UI button that the raycast should not hit
    public Graphic graphic;

    void Start()
    {
        // Set the raycast target of the UI image or UI button to be itself
        graphic.raycastTarget = true;
    }
}
