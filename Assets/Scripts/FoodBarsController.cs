using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBarsController : MonoBehaviour
{
    public List<FoodBar> foodBars = new List<FoodBar>();

    private void Start()
    {
        //ResetFoodBarAmmount();
        //ResetFoodBarPredictedAmmount();
    }

    public void UpdateFoodBarAmmount(List<float> values)
    {
        for (int i = 0; i < 3; i++)
        {
            float val = values[i];

            if (val > 1)
                foodBars[i].SetActualValue(1);
            else
                foodBars[i].SetActualValue(val);
        }
    }

    public void SetMaxValue(List<float> values)
    {
        for (int i = 0; i < 3; i++)
        {
            foodBars[i].SetMaxValue(values[i]);
        }
    }

    public void UpdateFoodBarAmmount(List<FoodItem> foodItems)
    {
        List<float> values = new List<float> { 0, 0, 0 };

        foreach (var item in foodItems)
        {
            for (int i = 0; i < item.foodData.foodValues.Count; i++)
            {
                values[i] += item.foodData.foodValues[i];
            }
        }

        Debug.Log($"{values[0]}, {values[1]}, {values[2]}");

        UpdateFoodBarAmmount(values);
    }

    public void UpdateFoodBarPredictedAmmount(List<float> values)
    {
        for (int i = 0; i < 3; i++)
        {
            float val = foodBars[i].GetActualFillValue() + values[i];

            if (val > 1)
                foodBars[i].SetExpectedValue(1);
            else
                foodBars[i].SetExpectedValue(val);
        }
    }

    public void UpdateFoodBarPredictedAmmount(List<FoodItem> foodItems)
    {
        List<float> predictedValues = new List<float> { 0, 0, 0 };

        foreach (var item in foodItems)
        {
            for (int i = 0; i < item.foodData.foodValues.Count; i++)
            {
                predictedValues[i] += item.foodData.foodValues[i];
            }
        }

        Debug.Log($"{predictedValues[0]}, {predictedValues[1]}, {predictedValues[2]}");

        UpdateFoodBarPredictedAmmount(predictedValues);
    }

    public void ResetFoodBarPredictedAmmount()
    {
        for (int i = 0; i < 3; i++)
        {
            foodBars[i].SetExpectedValue(0);
        }
    }

    public void ResetFoodBarAmmount()
    {
        for (int i = 0; i < 3; i++)
        {
            foodBars[i].SetActualValue(0);
        }
    }
}
