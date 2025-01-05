using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PetController : MonoBehaviour
{
    public CustomImageAnimation CIA;
    public List<ComplexAnimationFrame> idleFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> feedFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> winFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> deathFrames = new List<ComplexAnimationFrame>();
    public List<ComplexAnimationFrame> deadFrames = new List<ComplexAnimationFrame>();

    public List<float> baseRequiredFoodValues = new List<float>();
    public List<float> requiredFoodValues = new List<float>();

    public void OnStart()
    {
        requiredFoodValues = baseRequiredFoodValues;
    }

    public void OnFeed()
    {
        CIA.SetSprites(feedFrames);
        CIA.SetLoopCount(0);
        CIA.onLoopFinish.AddListener(() => { Idle(); });
    }

    public void OnWin(UnityAction evt, int loopCount)
    {
        CIA.SetSprites(winFrames);
        CIA.SetLoopCount(loopCount);
        CIA.onLoopFinish.AddListener(() => evt.Invoke());
        IncreaseFoodRequirement();
    }

    private void IncreaseFoodRequirement()
    {
        requiredFoodValues[0] += Mathf.Round(Random.Range(0.1f, 0.2f) * 100.0f) / 100.0f;
        requiredFoodValues[1] += Mathf.Round(Random.Range(0.1f, 0.2f) * 100.0f) / 100.0f;
        requiredFoodValues[2] += Mathf.Round(Random.Range(0.1f, 0.2f) * 100.0f) / 100.0f;
    }

    public void OnDeath()
    {
        CIA.SetSprites(deathFrames);
        CIA.SetLoopCount(0);
        CIA.onLoopFinish.AddListener(() => { Dead(); });
    }

    public void Idle()
    {
        CIA.SetSprites(idleFrames);
        CIA.SetLoopCount(-1);
    }

    private void Dead()
    {
        CIA.SetSprites(deadFrames);
        CIA.SetLoopCount(-1);
    }
}