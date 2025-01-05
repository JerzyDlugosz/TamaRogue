using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemsController : MonoBehaviour
{
    public GameObject FoodItemUIPrefab;
    public List<FoodObjectUI> selectedFoodObjects;
    public List<FoodObjectUI> instantiatedFoodObjects;
    public void SetupObjectUI(FoodItem foodObject, int objectNumber)
    {
        FoodObjectUI objectUI = Instantiate(FoodItemUIPrefab, transform).GetComponent<FoodObjectUI>();
        instantiatedFoodObjects.Add(objectUI);
        objectUI.foodID = foodObject.id;
        objectUI.foodImage.sprite = foodObject.baseSprite[0].sprite;
        var objectTransform = objectUI.GetComponent<RectTransform>();
        objectTransform.anchoredPosition = new Vector3(objectTransform.anchoredPosition.x + (16 * objectNumber), objectTransform.anchoredPosition.y, objectTransform.position.z);
        CustomImageAnimation CIA = objectUI.GetComponent<CustomImageAnimation>();
        CIA.SetLoopCount(-1);
        CIA.SetSprites(foodObject.baseSprite);
        CIA.OnInstantiate(foodObject.baseSprite);
        objectUI.onSelectEvent.AddListener((bool value) =>
        {
            if (value)
            {
                selectedFoodObjects.Add(objectUI);
                CustomImageAnimation CIA = objectUI.GetComponent<CustomImageAnimation>();
                CIA.SetLoopCount(-1);
                CIA.SetSprites(foodObject.selectedSprites);
            }
            else
            {
                selectedFoodObjects.Remove(objectUI);
                CustomImageAnimation CIA = objectUI.GetComponent<CustomImageAnimation>();
                CIA.SetLoopCount(-1);
                CIA.SetSprites(foodObject.baseSprite);

            }

        });
    }

    //public void PlaceObjectsUI()
    //{
    //    for (int i = 0; i < instantiatedFoodObjects.Count; i++)
    //    {
    //        var objectXPos = (128 / (instantiatedFoodObjects.Count + 1)) * (i + 1);
    //        Debug.Log(objectXPos);
    //        var objectTransform = instantiatedFoodObjects[i].GetComponent<RectTransform>();
    //        objectTransform.anchoredPosition = new Vector3(objectXPos, objectTransform.anchoredPosition.y, objectTransform.position.z);
    //    }
    //}

    public void KillAllTweens()
    {
        foreach (var item in instantiatedFoodObjects)
        {
            item.GetComponent<RectTransform>().DOKill();
        }
    }

    public void PlaceAnimation()
    {
        for (int i = 0; i < instantiatedFoodObjects.Count; i++)
        {
            var objectXPos = ((128 / (instantiatedFoodObjects.Count + 1)) * (i + 1));
            var objectTransform = instantiatedFoodObjects[i].GetComponent<RectTransform>();
            objectTransform.DOAnchorPosX(objectXPos,1f);
        }
    }

    List<FoodObjectUI> discardingFoodObjects = new List<FoodObjectUI>();

    public void AddDiscaringFoodObjects(List<FoodObjectUI> foodObjects)
    {
        discardingFoodObjects = new List<FoodObjectUI>(foodObjects);
    }

    public void DiscardAnimation()
    {
        for (int i = 0; i < discardingFoodObjects.Count; i++)
        {
            var objectTransform = discardingFoodObjects[i].GetComponent<RectTransform>();
            objectTransform.DOAnchorPosY(16, 0.2f).OnComplete(() => { 
                objectTransform.DOAnchorPosX(-144, 1f).OnComplete(() => {
                    objectTransform.DOKill();
                    RemoveAllDiscarded();
                }); 
            });
        }
    }

    public void RemoveObjectFromSelectedList(FoodObjectUI objectUI)
    {
        instantiatedFoodObjects.Remove(objectUI);
    }

    public void RemoveObjectFromSelectedList()
    {
        selectedFoodObjects.Clear();
    }

    private void RemoveObjectUI(FoodObjectUI objectUI)
    {
        Destroy(objectUI.gameObject);
    }

    private void RemoveAllDiscarded()
    {
        for (int i = discardingFoodObjects.Count - 1; i >= 0; i--)
        {
            Destroy(discardingFoodObjects[i].gameObject);
            discardingFoodObjects.RemoveAt(i);
        }
    }

    public void RemoveAllObjects()
    {
        for (int i = instantiatedFoodObjects.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedFoodObjects[i].gameObject);
            instantiatedFoodObjects.RemoveAt(i);
        }
    }
}
