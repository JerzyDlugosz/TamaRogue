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
    private int upgradeCount = 1;
    public List<ComplexAnimationFrame> highlightSprites = new List<ComplexAnimationFrame>();

    public List<Sprite> upgradeTypeSprites = new List<Sprite>();


    public void OnSceneSwitch()
    {
        RemoveAllUpgrades();

        foreach (UpgradeButton button in upgradeButtons)
        {
            var chance1 = UnityEngine.Random.Range(0, 10);
            upgradeCount = 1;
            if(chance1 == 0)
            {
                button.upgradeButton.GetComponent<Image>().sprite = upgradeTypeSprites[3];
                upgradeCount = 2;
                var chance2 = UnityEngine.Random.Range(0, 100);
                if(chance2 == 0)
                {
                    upgradeCount = 3;
                    button.upgradeButton.GetComponent<Image>().sprite = upgradeTypeSprites[4];
                }
            }

            int upgradeCountTypeNumber = 0;

            button.upgradeButton.onClick.RemoveAllListeners();
            for (int i = 0; i < upgradeCount; i++)
            {
                var upgrade = GameManagerScript.instance.upgradeManager.GetRandomUpgrade(GameManagerScript.instance.deckManager.currentDeck);

                if(upgrade.upgradeRarity > upgradeCountTypeNumber)
                    upgradeCountTypeNumber = upgrade.upgradeRarity;


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
            button.upgradeRarity.sprite = upgradeTypeSprites[upgradeCountTypeNumber];
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
            foreach (var button in upgradeButtons)
            {
                button.upgradeButton.GetComponent<Image>().sprite = upgradeTypeSprites[0];
                button.upgradeRarity.sprite = upgradeTypeSprites[0];
            }
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
    public Image upgradeRarity;
    public CustomImageAnimation CIA;
}
