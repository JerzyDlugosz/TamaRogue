using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    //All items that the player currently has (Including discarded)
    public List<FoodItem> currentFoodItems;
    //Items that can be shown on the board (currentFoodItems - discardedFoodItems)
    public List<FoodItem> playableFoodItems;
    //Items that are shown on the board
    public List<FoodItem> playedFoodItems;
    //Items that were discarded
    public List<FoodItem> discardedFoodItems;
    //Items that were eaten (Removed from the round)
    public List<FoodItem> eatenFoodItems;
    public int FoodItemsOnScreenCelling = 100;
    public int maxFoodItemsOnScreen;
    public int maxDiscards;
    public int remainingDiscards;
    public int maxPlays;
    public int remainingPlays;

    public int currentEvolutionNumber = 0;
    public int maxEvolutionNumber = 3;
    public int currentEvoStep = 0;
    public List<int> maxEvoSteps = new List<int> { 2, 3, 4};


    private void Start()
    {
        OnGameStart();
    }

    public void OnGameStart()
    {
        currentFoodItems = new List<FoodItem>();
        var deck = GameManagerScript.instance.deckManager.GetCurrentDeck();
        maxDiscards = deck.maxDiscards;
        maxPlays = deck.maxPlays;
        maxFoodItemsOnScreen = deck.maxFoodOnScreen;
        foreach (var item in deck.startingItems)
        {
            currentFoodItems.Add(Instantiate(item));
        }
    }

    public void AddFoodItemToDeck(string itemID)
    {
        var item = GameManagerScript.instance.deckManager.GetAvailableItem(itemID);
        if (item == null)
        {
            Debug.LogError($"No item of name {itemID} found");
            return;
        }
        currentFoodItems.Add(Instantiate(item));
    }

    public void OnRoundStart()
    {
        playableFoodItems = new List<FoodItem>(currentFoodItems);
        remainingDiscards = maxDiscards;
        remainingPlays = maxPlays;
    }

    public void OnDiscard(string discardedItemId)
    {
        int random = Random.Range(0, playedFoodItems.Count);
        FoodItem foodItem = playedFoodItems.Find(x => x.id == discardedItemId);
        discardedFoodItems.Add(foodItem);
        playedFoodItems.Remove(foodItem);

        GameManagerScript.instance.RefreshInfoPanel();
    }

    public void UpgradeCurrentItems(List<float> values, UpgradeType upgradeType)
    {
        foreach (var item in currentFoodItems)
        {
            for (int i = 0; i < item.foodData.foodValues.Count; i++)
            {
                switch (upgradeType)
                {
                    case UpgradeType.multi:
                        item.foodData.foodValues[i] *= values[i];
                        break;
                    case UpgradeType.divide:
                        item.foodData.foodValues[i] /= values[i];
                        break;
                    case UpgradeType.add:
                        item.foodData.foodValues[i] += values[i];
                        break;
                    case UpgradeType.subtract:
                        item.foodData.foodValues[i] -= values[i];
                        break;
                    default:
                        break;
                }
            }

        }

    }

    public void UpgradePet()
    {
        maxDiscards++;
        maxPlays++;
        maxFoodItemsOnScreen++;
    }

    public void OnGameReset()
    {
        discardedFoodItems.Clear();
        playableFoodItems.Clear();
        playedFoodItems.Clear();
        eatenFoodItems.Clear();
    }

    public FoodItem PickItem()
    {
        if (playableFoodItems.Count <= 0)
            return null;
        int random = Random.Range(0, playableFoodItems.Count);
        FoodItem foodItem = playableFoodItems[random];
        playedFoodItems.Add(foodItem);
        playableFoodItems.RemoveAt(random);
        return foodItem;
    }

    public void OnPlayItems()
    {
        eatenFoodItems.AddRange(playedFoodItems);
        playedFoodItems.Clear();
        playableFoodItems.AddRange(discardedFoodItems);
        discardedFoodItems.Clear();
        remainingPlays--;
        GameManagerScript.instance.RefreshInfoPanel();
    }
}


public enum UpgradeType
{ 
    none = 0,
    multi = 1,
    divide = 2,
    add = 3,
    subtract = 4
}