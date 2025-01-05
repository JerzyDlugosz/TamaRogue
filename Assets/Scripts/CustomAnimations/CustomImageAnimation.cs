using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomImageAnimation : CustomAnimation
{
    [SerializeField]
    private List<ComplexAnimationFrame> baseSprites;
    private Image imageUI;

    public override void OnInstantiate()
    {
        animationSprites = baseSprites;
        base.OnInstantiate();
    }

    private void Start()
    {
        if (TryGetComponent<Image>(out Image comp))
            imageUI = comp;
        else
            imageUI = GetComponentInChildren<Image>();

        if (defaultAnimationSprites == null)
            defaultAnimationSprites = baseSprites;

        if (instantiateOnStart)
            OnInstantiate();

        baseAnimationSpeed = animationSpeed;

        if (oneShot)
        {
            SetOneShot();
        }
    }

    private void Update()
    {
        if (internalAnimationUpdate)
        {
            UpdateAnimationFrame(true);
            SetSprite(imageUI);
            //CheckOneShot();
        }
    }
}
