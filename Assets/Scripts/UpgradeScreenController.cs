using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreenController : MonoBehaviour
{
    public List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    public GameObject singleUpgradePrefab;
    public GameObject multiUpgradePrefab;

    public Button accept;
    public Button cancel;
    public Button change;

    private int currentUpgrade = 0;
    public List<ComplexAnimationFrame> highlightSprites = new List<ComplexAnimationFrame>();


    public void OnSceneSwitch()
    {
        RemoveAllUpgrades();
        foreach (UpgradeButton button in upgradeButtons)
        {
            button.upgradeButton.onClick.RemoveAllListeners();
            var upgrade = GameManagerScript.instance.upgradeManager.GetRandomUpgrade(GameManagerScript.instance.deckManager.currentDeck);

            if (upgrade.isSingle)
            {
                var upgradeIcon = Instantiate(singleUpgradePrefab, button.upgradeButton.transform).GetComponent<UpgradeIcon>();
                upgradeIcon.SetUpgrade(upgrade.upgradeSprite);
                button.upgradeButton.onClick.AddListener(() =>
                {
                    upgrade.upgradeEvent.Invoke();
                });
            }
            else
            {
                var upgradeIcon = Instantiate(multiUpgradePrefab, button.upgradeButton.transform).GetComponent<UpgradeIcon>();
                upgradeIcon.SetUpgrade(upgrade.upgradeSprite, upgrade.upgradeTMPText, upgrade.upgradeColor);
                button.upgradeButton.onClick.AddListener(() =>
                {
                    upgrade.upgradeEvent.Invoke();
                });

            }

        }
        GameManagerScript.instance.upgradeManager.ResetShownUpgrades();
        SelectFirstUpgrade();
    }

    public void SelectNextUpgrade()
    {
        upgradeButtons[currentUpgrade].CIA.gameObject.SetActive(true);
        upgradeButtons[currentUpgrade].CIA.OnInstantiate();
        upgradeButtons[currentUpgrade].CIA.SetSprites(highlightSprites);
        upgradeButtons[currentUpgrade].CIA.SetLoopCount(-1);
    }

    public void SelectFirstUpgrade()
    {
        upgradeButtons[currentUpgrade].CIA.gameObject.SetActive(true);
        upgradeButtons[currentUpgrade].CIA.OnInstantiate();
        upgradeButtons[currentUpgrade].CIA.SetSprites(highlightSprites);
        upgradeButtons[currentUpgrade].CIA.SetLoopCount(-1);
    }


    public void OnAccept()
    {
        upgradeButtons[currentUpgrade].upgradeButton.onClick.Invoke();
        SwitchScene();
    }

    public void OnPetUpgrade()
    {
        GameManagerScript.instance.player.currentEvoStep++;
        SwitchScene();
    }

    public void SwitchScene()
    {
        RemoveAllUpgrades();
        this.GetComponent<RectTransform>().DOPivotY(2, 0.5f).OnComplete(() => {
            GameManagerScript.instance.ChangeSceneToGame();
        });
    }

    void RemoveAllUpgrades()
    {
        foreach (var item in upgradeButtons)
        {
            int numChildren = item.upgradeButton.transform.childCount;
            for (int i = numChildren - 1; i >= 0; i--)
            {
                Destroy(item.upgradeButton.transform.GetChild(i).gameObject);
            }
        }
    }

    public void OnChangeUpgrade()
    {

        upgradeButtons[currentUpgrade].CIA.gameObject.SetActive(false);

        currentUpgrade++;

        if (currentUpgrade > upgradeButtons.Count - 1)
            currentUpgrade = 0;

        SelectNextUpgrade();
    }
}

[Serializable]
public class UpgradeButton
{
    public Button upgradeButton;
    public CustomImageAnimation CIA;
}
