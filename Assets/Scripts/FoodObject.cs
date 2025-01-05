using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerUpHandler
{
    public FoodObjectData foodObjectData;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManagerScript.instance.foodBarsController.UpdateFoodBarAmmount(foodObjectData.foodValues);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log($"Down! Drag:{eventData.dragging}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManagerScript.instance.foodBarsController.UpdateFoodBarPredictedAmmount(foodObjectData.foodValues);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManagerScript.instance.foodBarsController.ResetFoodBarPredictedAmmount();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log($"Move! Drag:{eventData.dragging}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log($"Up! Drag:{eventData.dragging}");
    }
}
