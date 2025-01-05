using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FoodObjectUI : MonoBehaviour
{
    public string foodID;
    public Image foodImage;
    private bool isSelected = false;
    public TextMeshProUGUI infoValues;

    public UnityEvent<bool> onSelectEvent;


    public void Replace()
    {

    }

    public void PlayFood()
    {

    }

    private void Highlight(bool value)
    {
        //highlight.gameObject.SetActive(value);
    }

    public void Select()
    {
        isSelected = !isSelected;
        Highlight(isSelected);
        onSelectEvent.Invoke(isSelected);
    }

    public void ShowItemData(List<float> values)
    {
        infoValues.gameObject.SetActive(true);
        infoValues.text = $"<color=#CD4242>{values[0]}\r\n<color=#96B256>{values[1]}\r\n<color=#5B6EE1>{values[2]}";
    }

    public void HideItemData()
    {
        infoValues.gameObject.SetActive(false);
    }
}
