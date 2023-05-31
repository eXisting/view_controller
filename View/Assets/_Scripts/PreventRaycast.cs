using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreventRaycast : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Button button;

    void Awake()
    {
        // Get the CanvasGroup component from the UI button
        canvasGroup = GetComponent<CanvasGroup>();
        // Get the Button component
        button = GetComponent<Button>();
    }

    public void OnButtonClick()
    {
        // When the UI button is clicked, block raycasts from passing through the button and its children
        canvasGroup.blocksRaycasts = true;
    }

    public void OnButtonRelease()
    {
        // When the UI button is released, allow raycasts to pass through the button and its children
        canvasGroup.blocksRaycasts = false;
    }
}
