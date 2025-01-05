using System.Collections.Generic;
using UnityEngine;

public class CustomSpriteAnimation : CustomAnimation
{
    [SerializeField]
    private List<ComplexAnimationFrame> baseSprites;
    private SpriteRenderer spriteRenderer;

    public override void OnInstantiate()
    {
        animationSprites = baseSprites;
        base.OnInstantiate();
    }

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer comp))
            spriteRenderer = comp;
        else
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
            SetSprite(spriteRenderer);
            //CheckOneShot();
        }
    }

}
