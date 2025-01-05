using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI writtenValue;

    private float actualValue;
    private float expectedValue;
    private float maxValue;

    public Image actualValueImage;
    public Image expectedValueImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        writtenValue.gameObject.SetActive(true);
        writtenValue.text = $"{actualValue.ToString("F2")}/{maxValue.ToString("F2")}";
    }

    public void SetActualValue(float value)
    {
        actualValue = value;
        SetupActualValue();
    }

    public void SetExpectedValue(float value)
    {
        expectedValue = value;
        SetupExpectedValue();
    }

    public void SetMaxValue(float value)
    {
        maxValue = value;
    }

    public float GetActualFillValue()
    {
        return actualValueImage.fillAmount;
    }

    private void SetupActualValue()
    {
        actualValueImage.fillAmount = actualValue / maxValue;
    }

    private void SetupExpectedValue()
    {
        Debug.Log($"{expectedValue} / {maxValue}");
        expectedValueImage.fillAmount = expectedValue / maxValue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        writtenValue.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        writtenValue.gameObject.SetActive(false);
    }
}
