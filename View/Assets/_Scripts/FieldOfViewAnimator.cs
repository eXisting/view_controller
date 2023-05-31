using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAnimator : MonoBehaviour
{
    // The camera component of the main camera
    private Camera mainCamera;

    // The target field of view value
    public float targetFieldOfView;

    // The speed at which the field of view should animate
    public float animationSpeed;

    void Start()
    {
        // Get the main camera component
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the right mouse button is down
        if (Input.GetMouseButtonDown(1))
        {
            // Check if the mouse is over the specific object
            if (IsMouseOverObject())
            {
                // Start the field of view animation coroutine
                StartCoroutine(AnimateFieldOfView());
            }
        }
    }

    // Returns true if the mouse is over the specific object, false otherwise
    bool IsMouseOverObject()
    {
        // Create a ray from the main camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits the specific object
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    // Smoothly animates the field of view to the target value
    IEnumerator AnimateFieldOfView()
    {
        // Calculate the difference between the current field of view and the target value
        float fieldOfViewDifference = Mathf.Abs(mainCamera.fieldOfView - targetFieldOfView);

        // Calculate the animation time based on the field of view difference and the animation speed
        float animationTime = fieldOfViewDifference / animationSpeed;

        // Keep animating the field of view until it reaches the target value
        while (Mathf.Abs(mainCamera.fieldOfView - targetFieldOfView) > 0.01f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFieldOfView, Time.deltaTime / animationTime);
            yield return null;
        }

        // Set the field of view to the exact target value to avoid any precision issues
        mainCamera.fieldOfView = targetFieldOfView;
    }
}