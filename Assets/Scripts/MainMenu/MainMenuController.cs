using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject titleScreen;
    public RectTransform titleText;
    public RectTransform titleButtons;
    public Button startGameButton;

    public GameObject runChoiceScreen;
    public List<PetToPetInfo> petList = new List<PetToPetInfo>();
    public List<ComplexAnimationFrame> highlightSprites = new List<ComplexAnimationFrame>();
    private int currentSelectedPet = 0;

    public GameObject optionScreen;
    void Start()
    {
        TitleScreen();
    }

    public void SelectNextPet()
    {
        petList[currentSelectedPet].petInfo.SetActive(false);
        petList[currentSelectedPet].petHighlight.gameObject.SetActive(false);

        currentSelectedPet++;

        if (currentSelectedPet > petList.Count - 1)
            currentSelectedPet = 0;

        Debug.Log(currentSelectedPet);

        startGameButton.interactable = true;
        if (currentSelectedPet == 1 || currentSelectedPet == 2)
        {
            startGameButton.interactable = false;
        }

        petList[currentSelectedPet].petInfo.SetActive(true);
        petList[currentSelectedPet].petHighlight.gameObject.SetActive(true);
        petList[currentSelectedPet].petHighlight.OnInstantiate();
        petList[currentSelectedPet].petHighlight.SetSprites(highlightSprites);
        petList[currentSelectedPet].petHighlight.SetLoopCount(-1);
    }

    public void SelectFirstPet()
    {
        petList[currentSelectedPet].petInfo.SetActive(true);
        petList[currentSelectedPet].petHighlight.gameObject.SetActive(true);
        petList[currentSelectedPet].petHighlight.OnInstantiate();
        petList[currentSelectedPet].petHighlight.SetSprites(highlightSprites);
        petList[currentSelectedPet].petHighlight.SetLoopCount(-1);
    }

    public void DeckChoiceScreen()
    {
        titleScreen.SetActive(false);
        runChoiceScreen.SetActive(true);
        SelectFirstPet();
    }

    public void TitleScreen()
    {
        titleScreen.SetActive(true);
        runChoiceScreen.SetActive(false);
        optionScreen.SetActive(false);
        Debug.Log(titleText.pivot);
        Debug.Log(titleButtons.pivot);
        titleText.pivot = new Vector2(titleText.pivot.x, 0);
        titleButtons.pivot = new Vector2(titleText.pivot.x, 0);
        titleText.DOPivotY(1, 2f);
        titleButtons.DOScale(1, 1f).OnComplete(() => { titleButtons.DOPivotY(0, 1f); });
    }

    public void OnGameStart()
    {
        GameStateManager.instance.ChosenDeck = currentSelectedPet;
    }

    public void ShowOptions()
    {
        titleScreen.SetActive(false);
        optionScreen.SetActive(true);
        var rect = optionScreen.GetComponent<RectTransform>();
        rect.pivot = new Vector2(rect.pivot.x, 0);
        rect.DOPivotY(1, 0.5f);
    }
}

[Serializable]
public class PetToPetInfo
{
    public CustomImageAnimation petHighlight;
    public GameObject petInfo;
}
