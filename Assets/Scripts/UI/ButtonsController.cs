using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    public Button discardButton;
    public Button playButton;
    public Button explanationButton;

    List<Button> buttons = new List<Button>();

    private void Awake()
    {
        buttons = new List<Button>() { discardButton, playButton, explanationButton };
    }

    public void SetAllButtonsState(bool value)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            SetButtonState(i, value);
        }
    }

    public void SetButtonState(int buttonNumber, bool value)
    {
        buttons[buttonNumber].interactable = value;
    }
}
