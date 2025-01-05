using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomAnimationFrames
{

}

[Serializable]
public class ComplexAnimationFrame
{
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public float timeOfSprite = 1f;
    [SerializeField, HideInInspector]
    public UnityEvent additionalEvent;

    public static List<ComplexAnimationFrame> Convert(List<Sprite> sprites)
    {
        var CAFList = new List<ComplexAnimationFrame>();
        foreach (var item in sprites)
        {
            CAFList.Add(new ComplexAnimationFrame(item));
        }
        return CAFList;
    }

    public ComplexAnimationFrame()
    {
        this.sprite = null;
        this.timeOfSprite = 1;
        this.additionalEvent = null;
    }

    public ComplexAnimationFrame(Sprite _sprite)
    {
        this.sprite = _sprite;
        this.timeOfSprite = 1;
        this.additionalEvent = null;
    }

    public ComplexAnimationFrame(Sprite _sprite, float _timeOfSprite)
    {
        this.sprite = _sprite;
        this.timeOfSprite = _timeOfSprite;
        this.additionalEvent = null;
    }
}

[Serializable]
public class AdvancedAttackFrames
{
    [SerializeField]
    public GameObject collider;
    [SerializeField]
    public int frameToEnableColliderAt;
    public int frameToDisableColliderAt => frameToEnableColliderAt + 1;

    public AdvancedAttackFrames(GameObject _collider, int _frameToEnableColliderAt)
    {
        collider = _collider;
        frameToEnableColliderAt = _frameToEnableColliderAt;
    }
}
