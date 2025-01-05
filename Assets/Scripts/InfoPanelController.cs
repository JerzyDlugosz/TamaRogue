using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanelController : MonoBehaviour
{

    public TextMeshProUGUI playText;
    public TextMeshProUGUI discardText;
    public TextMeshProUGUI resultText;


    public void OnPlay(string text)
    {
        //playText.text = $"Play Count: {text}";
    }

    public void OnDiscard(string text)
    {
        //discardText.text = $"Discard Count: {text}";
    }

    public void OnResult(string text)
    {
        //resultText.gameObject.SetActive(true);
       // resultText.text = $"{text}";
    }
}
