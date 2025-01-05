using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public FoodBarsController foodBarsController;
    public FoodItemsController fooditemsController;
    public InfoPanelController infoPanelController;
    public PetInGameController petController;
    public ButtonsController buttonsController;
    public DeckManager deckManager;
    public UpgradeManager upgradeManager;
    public UpgradeScreenController upgradeScreenController;
    public Player player;

    public int currentItemsOnScreen = 0;
    public List<float> currentFoodValues = new List<float>();
    public List<float> playedFoodValues = new List<float>();

    public GameObject gameScene;
    public GameObject shopScene;
    public GameObject evoScene;
    public GameObject winScene;
    public GameObject loseScene;
    public GameObject TutorialScene;

    public GameObject foodInGame;
    public Transform petPosition;
    public Transform evoTarget;

    public int roundNumber = 0;

    public AudioClip eatingSound;
    public AudioClip winSound;

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        player.OnGameStart();
        StartGame();
        petController.SetPetAnimations();
    }

    private void StartGame()
    {
        shopScene.SetActive(false);
        gameScene.SetActive(true);
        SetStartingStats();
        player.OnRoundStart();
        RefreshInfoPanel();
        Roll();
        buttonsController.SetAllButtonsState(true);
    }

    private void SetStartingStats()
    {
        player.remainingDiscards = player.maxDiscards;
        player.remainingPlays = player.maxPlays;
        petController.OnStart();
        currentFoodValues = new List<float> { 0, 0, 0 };
        playedFoodValues = new List<float> { 0, 0, 0 };
    }

    public void RefreshInfoPanel()
    {
        foodBarsController.SetMaxValue(petController.requiredFoodValues);
        infoPanelController.OnPlay(player.remainingPlays.ToString());
        infoPanelController.OnDiscard(player.remainingDiscards.ToString());

        foodBarsController.UpdateFoodBarPredictedAmmount(playedFoodValues);
        foodBarsController.UpdateFoodBarAmmount(currentFoodValues);

        if (player.remainingDiscards <= 0)
            buttonsController.SetButtonState(0, false);

        if (player.remainingPlays <= 0)
            buttonsController.SetButtonState(1, false);

    }

    public void Roll()
    {
        for (int i = currentItemsOnScreen; i < player.maxFoodItemsOnScreen; i++)
        {
            var foodObject = player.PickItem();
            if (foodObject == null)
            {
                break;
            }
            fooditemsController.SetupObjectUI(foodObject, i);
            currentItemsOnScreen++;
        }

        ApplyEffects();
        fooditemsController.PlaceAnimation();
        foodBarsController.UpdateFoodBarPredictedAmmount(playedFoodValues);
    }

    public void Discard()
    {
        if (player.remainingDiscards > 0 && fooditemsController.selectedFoodObjects.Count > 0)
        {
            fooditemsController.AddDiscaringFoodObjects(fooditemsController.selectedFoodObjects);
            foreach (var item in fooditemsController.selectedFoodObjects)
            {
                player.OnDiscard(item.foodID);
                fooditemsController.RemoveObjectFromSelectedList(item);
                currentItemsOnScreen--;
            }
            player.remainingDiscards--;
            fooditemsController.RemoveObjectFromSelectedList();

            fooditemsController.DiscardAnimation();
            fooditemsController.selectedFoodObjects.Clear();
            Roll();
        }
    }

    public void ApplyEffects()
    {
        List<List<float>> playedValues = new List<List<float>>();
        playedFoodValues = new List<float> { 0, 0, 0 };
        for (int i = 0; i < player.playedFoodItems.Count; i++)
        {
            playedValues.Add(new List<float> { 0, 0, 0 });
        }

        for (int i = 0; i < player.playedFoodItems.Count; i++)
        {
            for (int j = 0; j < player.playedFoodItems[i].foodData.foodValues.Count; j++)
            {
                playedValues[i][j] = player.playedFoodItems[i].foodData.foodValues[j];
            }
        }


        int itemIndex = 0;
        foreach (var item in player.playedFoodItems)
        {
            if (item.additionalEffect)
            {
                for (int i = item.affectedFoodItems.x; i < item.affectedFoodItems.y + 1; i++)
                {
                    int affectedItemIndex = itemIndex + i;
                    if (affectedItemIndex < 0 || affectedItemIndex >= currentItemsOnScreen)
                        continue;
                    if (affectedItemIndex == itemIndex && !item.affectsItself)
                        continue;

                    for (int j = 0; j < item.foodData.foodValues.Count; j++)
                    {
                        playedValues[affectedItemIndex][j] = (playedValues[affectedItemIndex][j] + item.additionalBuffToAffected.foodValues[j]) * item.multiplicativeBuffToAffected.foodValues[j];
                    }
                }
            }
            itemIndex++;
        }

        foreach (var item in playedValues)
        {
            for (int i = 0; i < item.Count; i++)
            {
                playedFoodValues[i] += item[i];
            }
        }

        List<float> currentFoodValues2 = new List<float> { 0, 0, 0 };


        foreach (var item in player.playedFoodItems)
        {
            for (int i = 0; i < item.foodData.foodValues.Count; i++)
            {
                currentFoodValues2[i] += item.foodData.foodValues[i];
            }
        }

        //Debug.Log($"{playedFoodValues[0]}, {playedFoodValues[1]}, {playedFoodValues[2]} || {currentFoodValues2[0]}, {currentFoodValues2[1]}, {currentFoodValues2[2]}");

        //foreach (var item in player.playedFoodItems)
        //{
        //    if(item.additionalEffect)
        //    {
        //        for (int i = 0; i < item.foodData.foodValues.Count; i++)
        //        {
        //            currentFoodValues[i] = (currentFoodValues[i] + item.additionalBuffToAffected.foodValues[i]) * item.multiplicativeBuffToAffected.foodValues[i];
        //        }
        //    }
        //}
    }

    public void PlayItems()
    {
        buttonsController.SetAllButtonsState(false);
        if (player.remainingPlays == 0)
            return;

        fooditemsController.KillAllTweens();

        for (int i = 0; i < currentFoodValues.Count; i++)
        {
            currentFoodValues[i] += playedFoodValues[i];
        }

        foodBarsController.UpdateFoodBarAmmount(currentFoodValues);
        player.OnPlayItems();
        foreach (var item in fooditemsController.instantiatedFoodObjects)
        {
            var spriteRend = Instantiate(foodInGame, item.transform.position, Quaternion.identity).GetComponentInChildren<SpriteRenderer>();
            spriteRend.sprite = item.foodImage.sprite;
            Destroy(item.gameObject);
            currentItemsOnScreen--;
        }

        fooditemsController.instantiatedFoodObjects.Clear();
        fooditemsController.selectedFoodObjects.Clear();

        bool areFoodValuesReached = true;

        for (int i = 0; i < currentFoodValues.Count; i++)
        {
            if (currentFoodValues[i] < petController.requiredFoodValues[i])
                areFoodValuesReached = false;
        }

        if(!areFoodValuesReached)
            Roll();

        //petController.OnFeed(() => OnFeedEnd());
        GameStateManager.instance.audioManager.PlaySoundEffect(eatingSound);
        petController.OnFeed();
        Invoke(nameof(OnFeedEnd), 1f);
    }

    private void OnFeedEnd()
    {
        bool areFoodValuesReached = true;

        for (int i = 0; i < currentFoodValues.Count; i++)
        {
            if (currentFoodValues[i] < petController.requiredFoodValues[i])
                areFoodValuesReached = false;
        }

        if (player.remainingPlays == 0 || areFoodValuesReached)
        {
            GameResults(areFoodValuesReached);
            return;
        }
        else if (player.playedFoodItems.Count == 0 && (player.playableFoodItems.Count == 0 && player.discardedFoodItems.Count == 0))
        {
            GameResults(areFoodValuesReached);
            return;
        }
        else
        {
            buttonsController.SetAllButtonsState(true);
            RefreshInfoPanel();
        }
    }

    public void GameResults(bool areFoodValuesReached)
    {
        foodBarsController.ResetFoodBarPredictedAmmount();

        if(areFoodValuesReached)
        {
            GameStateManager.instance.audioManager.PlaySoundEffect(winSound);
            petController.OnWin(() => ChangeSceneToShop());
            buttonsController.SetAllButtonsState(false);
        }
        else
        {
            petController.OnDeath(() => LoseGame());
            buttonsController.SetAllButtonsState(false);
        }
    }

    public void ShowTutorial()
    {
        gameScene.SetActive(false);
        TutorialScene.SetActive(true);
        var rect = TutorialScene.GetComponent<RectTransform>();
        rect.pivot = new Vector2(rect.pivot.x, 0);
        rect.DOPivotY(1, 0.5f);
    }

    public void HideTutorial()
    {
        var rect = TutorialScene.GetComponent<RectTransform>();
        rect.DOPivotY(2, 0.5f).OnComplete(() => {
            TutorialScene.SetActive(false);
            gameScene.SetActive(true);
        });
    }

    public void LoseGame()
    {
        GameStateManager.instance.audioManager.OnMapChange((Zone)4);
        gameScene.SetActive(false);
        loseScene.SetActive(true);
        var rect = loseScene.GetComponent<RectTransform>();
        rect.pivot = new Vector2(rect.pivot.x, 0);
        rect.DOPivotY(1, 0.5f).OnComplete(() => {
            upgradeScreenController.OnSceneSwitch();
        });
    }

    public void ResetGame()
    {
        player.OnGameReset();
        fooditemsController.RemoveAllObjects();
        currentItemsOnScreen = 0;
        petController.Idle();
        DOTween.KillAll();
        StartGame();
    }

    public void ChangeSceneToShop()
    {
        shopScene.SetActive(true);
        gameScene.SetActive(false);
        var rect = shopScene.GetComponent<RectTransform>();
        rect.pivot = new Vector2(rect.pivot.x, 0);
        GameStateManager.instance.audioManager.OnMapChange((Zone)3);
        rect.DOPivotY(1, 0.5f).OnComplete(() => { 
            upgradeScreenController.OnSceneSwitch(); 
        }); 
    }

    public void ChangeSceneToGame()
    {
        if(player.currentEvoStep >= player.maxEvoSteps[player.currentEvolutionNumber])
        {
            shopScene.SetActive(false);
            gameScene.SetActive(false);
            evoScene.SetActive(true);
            EvoAnimation();
        }
        else
        {
            shopScene.SetActive(false);
            gameScene.SetActive(true);
            roundNumber++;
            GameStateManager.instance.audioManager.OnMapChange((Zone)2);
            ResetGame();
        }
    }

    private Vector2 previousPosition;

    public void EvoAnimation()
    {
        if(player.currentEvolutionNumber == 2)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Insert(0, petController.gameObject.transform.DOMove(evoTarget.position, 2f));
            mySequence.Append(petController.gameObject.transform.DOScale(30, 10f));
            mySequence.Insert(3f, Camera.main.transform.DOShakePosition(10f, 3).SetEase(Ease.InBack));
            mySequence.OnComplete(() => { WinScreen(); });
        }
        else
        {
            petController.SetupEvoFrames();
            player.UpgradePet();
            previousPosition = petController.gameObject.transform.position;
            player.currentEvolutionNumber++;
            player.currentEvoStep = 0;
            petController.SetPetAnimations();
            petController.gameObject.transform.DOMove(evoTarget.position, 2f);
            petController.Evolution(() => AfterEvo());
        }
    }

    public void WinScreen()
    {
        gameScene.SetActive(false);
        winScene.SetActive(true);
        var rect = winScene.GetComponent<RectTransform>();
        rect.pivot = new Vector2(rect.pivot.x, 0);
        rect.DOPivotY(1, 0.5f);
    }

    public void AfterEvo()
    {
        petController.gameObject.transform.DOMove(previousPosition, 2f).OnComplete(() => { ChangeSceneToGame(); petController.Idle(); });
    }
}
