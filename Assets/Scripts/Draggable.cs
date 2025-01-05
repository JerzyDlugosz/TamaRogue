using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector2 offset;
    private Vector3 basePosition;
    private bool isDragging = false;

    public UnityEvent onRelease;

    private void Start()
    {
        basePosition = transform.position;
    }

    private void Update()
    {
        if (!isDragging)
        {
        }
    }

    public void MoveObjectToPosition(Vector3 newPostion)
    {
        transform.DOMove(newPostion, 0.1f).SetEase(Ease.OutSine);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"DragBegin! Drag:{eventData.dragging}");
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"OnDrag! Drag:{eventData.dragging}");
        Vector2 vec = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position) + offset;
        transform.position = new Vector3(vec.x, vec.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"DragEnd! Drag:{eventData.dragging}");
        var number =  EventExtensions.GetListenerNumber(onRelease);
        Debug.Log($"event count: {number}");
        if (number == 0)
            MoveObjectToPosition(basePosition);
        else
            onRelease.Invoke();
        onRelease.RemoveAllListeners();
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Down! Drag:{eventData.dragging}");
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
    }
}
