using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgrades = new List<Upgrade>();
    
    public Upgrade GetRandomUpgrade(int currentDeck)
    {
        var availableUpgradesInDeck = upgrades.FindAll(x => x.availableInDecks.Contains(currentDeck) == true);
        var availableUpgrades = availableUpgradesInDeck.FindAll(x => x.isShown == false);
        int rollCount = 0;

        foreach (var item in availableUpgrades)
        {
            rollCount += item.rollAmmount;
        }

        var rand = UnityEngine.Random.Range(0, rollCount);

        int rollResult = 0;

        foreach (var item in availableUpgrades)
        {
            rollResult += item.rollAmmount;
            if (rollResult >= rand)
            {
                SetUpgradeAsShown(item);
                return item;
            }
        }

        return null;
    }

    private void SetUpgradeAsShown(Upgrade upgrade)
    {
        upgrade.isShown = true;
    }

    public void ResetShownUpgrades()
    {
        foreach (var item in upgrades)
        {
            item.isShown = false;
        }
    }

    public void AddFoodToDeck(string foodId)
    {
        GameManagerScript.instance.player.AddFoodItemToDeck(foodId);
    }

    public void UpgradeCurrentItems(string upgradeID)
    {
        var upgrade = upgrades.Find(x => x.upgradeID == upgradeID);
        GameManagerScript.instance.player.UpgradeCurrentItems(upgrade.upgradeValues, upgrade.type);
    }
}

[Serializable]
public class Upgrade
{
    public string upgradeID;
    public List<int> availableInDecks;
    public int rollAmmount;
    public UnityEvent upgradeEvent;



    [Space]
    public bool isSingle = true;

    public UpgradeType type;
    public List<float> upgradeValues;

    public Color upgradeColor;
    public Sprite upgradeSprite;
    public string upgradeTMPText;

    public bool isShown = false;
}

