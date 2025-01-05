using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DraggableTarget : MonoBehaviour
{
    public GameObject placeholderPrefab;
    private GameObject instantiatedPrefab;
    private Draggable currentDraggable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentDraggable == null)
        { 
            Debug.LogWarning(collision.gameObject.name);
            instantiatedPrefab = Instantiate(placeholderPrefab, transform.position, Quaternion.identity);
            currentDraggable = collision.GetComponent<Draggable>();
            currentDraggable.onRelease.AddListener(() => { RemovePlaceholder(); currentDraggable.MoveObjectToPosition(transform.position); });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Draggable>() == currentDraggable)
        {
            RemovePlaceholder();
            currentDraggable.onRelease.RemoveAllListeners();
            currentDraggable = null;
        }
    }

    public void RemovePlaceholder()
    {
        if(instantiatedPrefab != null)
            Destroy(instantiatedPrefab);
    }
}
