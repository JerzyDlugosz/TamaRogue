using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PetEvolution
{
    public List<Sprite> sprites;
}

public class PetInGameController : MonoBehaviour
{
    public CustomSpriteAnimation CIA;
    public List<ComplexAnimationFrame> idleFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> moveFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> feedFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> winFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> deathFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> deadFrames = new List<ComplexAnimationFrame>();

    public List<ComplexAnimationFrame> evoFrames = new List<ComplexAnimationFrame>();

    List<List<ComplexAnimationFrame>> petFrames = new List<List<ComplexAnimationFrame>>();

    public List<float> baseRequiredFoodValues = new List<float>();
    public List<float> requiredFoodValues = new List<float>();

    [HideInInspector] public UnityEvent onTargetReached;
    public PetState petState;
    public GameObject target;
    public GameObject targetRange;
    public CustomAIPath aIPath;

    [Range(0f, 1f)]
    public float walkChance;

    public List<PetEvolution> allSprites;

    public SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        onTargetReached.AddListener(() => StartIdling());
    }

    private void Start()
    {
        petFrames = new List<List<ComplexAnimationFrame>> {idleFrames, feedFrames, moveFrames, winFrames, deathFrames, deadFrames};


        InvokeRepeating(nameof(NewTargetTimer), 0, 0.2f);
    }

    public void SetupEvoFrames()
    {
        var evoNumber = GameManagerScript.instance.player.currentEvolutionNumber;
        evoFrames.Clear();
        for (int i = 0; i < 20; i++)
        {
            evoFrames.Add(new ComplexAnimationFrame(allSprites[evoNumber].sprites[0], 2f / ((i/4) + 1)));
            evoFrames.Add(new ComplexAnimationFrame(allSprites[evoNumber + 1].sprites[0], 2f / ((i/4) + 1)));
        }
        evoFrames.Add(new ComplexAnimationFrame(allSprites[evoNumber + 1].sprites[0], 2f));
    }

    public void SetPetAnimations()
    {
        var evoNumber = GameManagerScript.instance.player.currentEvolutionNumber;
        int i = 0;
        int x = 0;
        foreach (var item in petFrames)
        {
            x = 0;
            for (int j = 0; j < item.Count; j++)
            {
                item[j].sprite = allSprites[evoNumber].sprites[j + i];
                x++;
            }
            i += x;
        }
    }

    public void StartIdling()
    {
        if (petState.HasFlag(PetState.Move))
        {
            petState = PetState.Idle;
            Idle();
        }
    }

    public void NewTargetTimer()
    {

        bool onlyOneEnum = (petState & (petState - 1)) != 0;
        if (petState.HasFlag(PetState.Idle) && !onlyOneEnum)
        {
            var rand = UnityEngine.Random.Range(walkChance, 1f);
            if (rand >= 0.95f)
                DecideNewTarget();
        }
    }

    private void Update()
    {
        if (petState.HasFlag(PetState.Move))
            MovementState(true);
        else
            MovementState(false);

        if (aIPath.desiredVelocity.x < 0f)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public void DecideNewTarget()
    {
        petState = PetState.Move;
        var range = targetRange.transform;
        var posX = UnityEngine.Random.Range(range.position.x - range.localScale.x / 2, range.position.x + range.localScale.x / 2);
        var posY = UnityEngine.Random.Range(range.position.y - range.localScale.y / 2, range.position.y + range.localScale.y / 2);
        target.transform.position = new Vector2(posX, posY);
        OnMove();
    }

    public void MovementState(bool state)
    {
        aIPath.canMove = state;
    }

    public void OnStart()
    {
        requiredFoodValues = baseRequiredFoodValues;
    }

    public void OnFeed()
    {
        CIA.SetSprites(feedFrames);
        CIA.SetLoopCount(2);
        MovementState(false);
        petState = PetState.Idle;
        petState |= PetState.Feed;
        CIA.onLoopFinish.AddListener(() => Idle());
    }

    public void OnWin(UnityAction evt)
    {
        CIA.SetSprites(winFrames);
        CIA.SetLoopCount(1);
        IncreaseFoodRequirement();
        petState = PetState.Idle;
        petState |= PetState.Win;
        CIA.onLoopFinish.AddListener(() => { evt.Invoke(); Idle(); });
    }

    private void IncreaseFoodRequirement()
    {
        var divideValue = MathF.Max(4 - (GameManagerScript.instance.roundNumber / 4), 1);
        float maxValue = 0.5f + (0.2f * GameManagerScript.instance.roundNumber / divideValue);
        float minValue = 0.5f + (0.1f * GameManagerScript.instance.roundNumber / divideValue);

        Debug.Log($"{minValue} / {maxValue}");

        requiredFoodValues[0] = (float)Math.Round(UnityEngine.Random.Range(minValue, maxValue), 1);
        requiredFoodValues[1] = (float)Math.Round(UnityEngine.Random.Range(minValue, maxValue), 1);
        requiredFoodValues[2] = (float)Math.Round(UnityEngine.Random.Range(minValue, maxValue), 1);

        Debug.Log($"Values: {requiredFoodValues[0]} / {requiredFoodValues[1]} / {requiredFoodValues[2]}");
    }

    public void OnDeath(UnityAction evt)
    {
        CIA.SetSprites(deathFrames);
        CIA.SetLoopCount(0);
        petState = PetState.Idle;
        petState |= PetState.Death;
        CIA.onLoopFinish.AddListener(() => { Dead(); evt.Invoke(); });
    }

    public void Idle()
    {
        CIA.SetSprites(idleFrames);
        CIA.SetLoopCount(-1);
        petState = PetState.Idle;
    }

    public void OnMove()
    {
        CIA.SetSprites(moveFrames);
        CIA.SetLoopCount(-1);
        petState = PetState.Move;
    }

    private void Dead()
    {
        CIA.SetSprites(deadFrames);
        CIA.SetLoopCount(-1);
        petState = PetState.Idle;
        petState |= PetState.Dead;
    }

    public void Evolution(UnityAction evt)
    {
        CIA.SetSprites(evoFrames);
        CIA.ChangeDefaultAnimationSprites(idleFrames);
        CIA.SetLoopCount(0);
        CIA.onLoopFinish.AddListener(() => { evt.Invoke();});
        petState = PetState.Evo;
    }
}

[Flags]
public enum PetState
{
    None = 0,
    Idle = 1 << 0,
    Move = 1 << 1,
    Feed = 1 << 2,
    Win = 1 << 3,
    Death = 1 << 4,
    Dead = 1 << 5,
    Evo = 1 << 6
}

