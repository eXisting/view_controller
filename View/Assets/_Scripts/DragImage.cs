using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragImage : MonoBehaviour, IDragHandler, IEndDragHandler
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
        imageTransform.position = originalPosition;
    }
}

