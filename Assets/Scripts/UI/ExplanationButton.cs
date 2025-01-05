using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExplanationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;
    public TextMeshProUGUI playCount;
    public TextMeshProUGUI discardCount;
    public GameObject evoCount;
    public TextMeshProUGUI evoText;
    public List<FoodBar> foodBars;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfo();
    }

    public void ShowItemValues()
    {
        var FIC = GameManagerScript.instance.fooditemsController;
        var player = GameManagerScript.instance.player;
        for (int i = 0; i < FIC.instantiatedFoodObjects.Count; i++)
        {
            FIC.instantiatedFoodObjects[i].ShowItemData(player.playedFoodItems[i].foodData.foodValues);
        }
        foreach (var item in foodBars)
        {
            item.ShowInfo();
        }
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
        evoCount.SetActive(true);
        playCount.text = $"<sprite=2 tint>{GameManagerScript.instance.player.remainingPlays}";
        discardCount.text = $"<sprite=1 tint>{GameManagerScript.instance.player.remainingDiscards}";
        evoText.text = $"{GameManagerScript.instance.player.currentEvoStep}/{GameManagerScript.instance.player.maxEvoSteps[GameManagerScript.instance.player.currentEvolutionNumber]} Evo";
        ShowItemValues();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideInfo();
    }

    private void OnDisable()
    {
        HideInfo();
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
        evoCount.SetActive(false);
        foreach (var item in GameManagerScript.instance.fooditemsController.instantiatedFoodObjects)
        {
            item.HideItemData();
        }
        foreach(var item in foodBars)
        {
            item.HideInfo();
        }
    }
}
