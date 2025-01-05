using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeIcon : MonoBehaviour
{
    public Image foodImage;
    public TextMeshProUGUI upgradeText;


    public void SetUpgrade(Sprite foodSprite, string text, Color color)
    {
        foodImage.sprite = foodSprite;
        upgradeText.text = text;
        upgradeText.color = color;
    }

    public void SetUpgrade(Sprite foodSprite)
    {
        foodImage.sprite = foodSprite;
    }
}
