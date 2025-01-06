using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Deck> decks;
    public int currentDeck;

    private void Start()
    {
        currentDeck = GameStateManager.instance.ChosenDeck;
    }

    public Deck GetCurrentDeck()
    {
        return decks[currentDeck];
    }

    public FoodItem GetAvailableItem(string itemID)
    {
        var item = decks[currentDeck].availableItems.Find(x => x.id == itemID);
        if (item == null)
            return null;
        return decks[currentDeck].availableItems.Find(x => x.id == itemID);
    }

}

[Serializable]
public class Deck
{
    public string deckID;
    public string deckName;
    public int maxDiscards;
    public int maxPlays;
    public int maxFoodOnScreen;
    public List<FoodItem> startingItems;
    public List<FoodItem> availableItems;
}