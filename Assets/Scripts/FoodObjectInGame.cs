using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObjectInGame : MonoBehaviour
{
    private void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Insert(0, transform.DOMove(GameManagerScript.instance.petPosition.position, 1f).SetEase(Ease.InBack));
        mySequence.Insert(0, transform.DOScale(0, 1f).SetEase(Ease.InBack));
        mySequence.OnComplete(() => { Destroy(gameObject); });
    }
}
