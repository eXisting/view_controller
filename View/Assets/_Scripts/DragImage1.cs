using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragImage1 : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private Transform imageTransform;
    private RectTransform canvasRectTransform;
    private Vector2 clampedPosition;

    void Start()
    {
        imageTransform = transform;
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        originalPosition = imageTransform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the right mouse button was clicked
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Pick up the image
            imageTransform.SetParent(canvasRectTransform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out currentPosition
        ))
        {
            imageTransform.position = currentPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Drop the image
        imageTransform.SetParent(canvasRectTransform.parent);
        imageTransform.position = originalPosition;
    }
}
