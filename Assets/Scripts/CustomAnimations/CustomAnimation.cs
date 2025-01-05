using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomAnimation : MonoBehaviour
{
    /// <summary>
    /// Simple frame animation script. Use this as a base for other scirpts (Image, Sprite etc.)
    /// Example Usage:
    ///   CustomImageAnimation : CustomAnimation
    ///   
    ///   CustomImageAnimation CIA;
    ///   
    ///   CIA.OnInstantiate();                              Used when instantiating an instance of this script (only if instance has animationSprites set)
    ///   CIA.OnInstantiate(frames);                        Used when instance of the script has no animationSprites set in an inspector 
    ///   
    ///   CIA.SetSprites(frames);                           Set sprites of an animation !Needs to be first in order! (after instantiate)
    ///   CIA.SetLoopCount(0);                              Set loop count (0 = no loops, <0 = infinity loop, >0 = x loops)
    ///   CIA.onLoopFinish.AddListener(() => { Idle(); });  Set on loop finish event (triggers only when loop count is more or equal to zero)
    ///   
    ///   
    /// </summary>

    [HideInInspector]
    public UnityEvent onOneShotFinish;
    [HideInInspector]
    public UnityEvent onLoopFinish;
    [HideInInspector]
    public Queue<UnityAction> onLoopFinishQueue = new Queue<UnityAction>();

    public List<ComplexAnimationFrame> animationSprites;
    protected List<ComplexAnimationFrame> defaultAnimationSprites;

    [SerializeField]
    protected float animationSpeed;
    protected float baseAnimationSpeed;
    private float animationFrame = 0f;

    [SerializeField] protected bool internalAnimationUpdate = false;
    [SerializeField] protected bool oneShot = false;
    protected int loopCount = 0;

    private int previousAnimationFrame = -1;

    //Dont remember what this was for
    //private int oneShotCount = -1;

    public bool instantiateOnStart = false;

    public virtual void OnInstantiate()
    {
        defaultAnimationSprites = animationSprites;
        baseAnimationSpeed = animationSpeed;
    }

    public void OnInstantiate(List<ComplexAnimationFrame> baseSprites)
    {
        animationSprites = baseSprites;
        defaultAnimationSprites = animationSprites;
        baseAnimationSpeed = animationSpeed;
    }

    public void SetSprites(List<ComplexAnimationFrame> _sprites)
    {
        animationSprites = _sprites;
        ResetAnimationFrame();
        ResetOneShot();
    }

    public void SetAnimationSpeed(float _animationSpeed)
    {
        animationSpeed = _animationSpeed;
    }

    protected void CheckOneShot()
    {
        if (oneShot)
        {
            //if (oneShotCount < (int)animationFrame + 1)
            //    oneShotCount = (int)animationFrame + 1;

            //if (oneShotCount >= animationSprites.Count)
            //    onOneShotFinish.Invoke();

            onOneShotFinish.Invoke();
            Destroy(gameObject);
        }
    }

    public void ChangeDefaultAnimationSprites(List<ComplexAnimationFrame> _defaultAnimationSprites)
    {
        defaultAnimationSprites = _defaultAnimationSprites;
    }

    public void SetOneShot()
    {
        oneShot = true;
        loopCount = -1;
    }

    public void ResetOneShot()
    {
        oneShot = false;
    }


    public void AnimationState(bool value)
    {
        internalAnimationUpdate = value;
    }

    private void AnimationLoopFinish()
    {
        ResetAnimationFrame();
        loopCount--;
    }

    private void AllLoopsFinish()
    {
        CheckOneShot();
        ResetAnimationFrame();
        ResetAnimationSpeed();
        ResetAnimationSprites();
        onLoopFinish.Invoke();
        onLoopFinish.RemoveAllListeners();
        if (onLoopFinishQueue.Count > 0)
        {
            onLoopFinish.AddListener(onLoopFinishQueue.Peek());
            onLoopFinishQueue.Dequeue();
        }
    }

    public void AddOnLoopEndActionToQueue(UnityAction act)
    {
        onLoopFinishQueue.Enqueue(act);

        if (EventExtensions.GetListenerNumber(onLoopFinish) <= 0)
        {
            onLoopFinish.AddListener(onLoopFinishQueue.Peek());
            onLoopFinishQueue.Dequeue();
        }
    }

    public void SetLoopCount(int _loopCount)
    {
        loopCount = _loopCount;
    }

    public void ResetAnimationFrame()
    {
        animationFrame = 0f;
    }

    public void ResetAnimationSpeed()
    {
        animationSpeed = baseAnimationSpeed;
    }

    public void ResetAnimationSprites()
    {
        animationSprites = defaultAnimationSprites;
    }

    protected void SetSprite(SpriteRenderer sr)
    {
        try
        {
            if ((int)animationFrame > animationSprites.Count - 1)
            {
                if (loopCount != 0)
                    AnimationLoopFinish();
                else
                    AllLoopsFinish();
            }
            if ((int)animationFrame != previousAnimationFrame)
            {
                sr.sprite = animationSprites[(int)animationFrame].sprite;
                if (animationSprites[(int)animationFrame].additionalEvent != null)
                    animationSprites[(int)animationFrame].additionalEvent.Invoke();
                previousAnimationFrame = (int)animationFrame;
            }

        }
        catch (Exception exc)
        {
            Debug.LogError(exc.Message);
        }
    }



    protected void SetSprite(Image img)
    {
        if ((int)animationFrame > animationSprites.Count - 1)
        {
            if (loopCount != 0)
                AnimationLoopFinish();
            else
                AllLoopsFinish();
        }

        img.sprite = animationSprites[(int)animationFrame].sprite;
    }

    public void UpdateAnimationFrame(bool unscaledTime = false)
    {
        if(unscaledTime)
            animationFrame += Time.unscaledDeltaTime * animationSpeed / animationSprites[(int)animationFrame].timeOfSprite;
        else
            animationFrame += Time.deltaTime * animationSpeed / animationSprites[(int)animationFrame].timeOfSprite;
    }

    public void UpdateAnimationFrame(float xVelocity)
    {
        animationFrame += Mathf.Abs(xVelocity) * animationSpeed;
    }

    public void UpdateAnimationFrame(Vector2 velocity)
    {
        animationFrame += (Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y)) * animationSpeed;
    }

    protected void SetSpriteNoise(SpriteRenderer sr)
    {
        try
        {
            sr.sprite = animationSprites[(int)animationFrame].sprite;
        }
        catch
        {

        }
    }

    Vector2 noise = new Vector2(0, 0);

    public void UpdateAnimationFrameWithNoise(Vector2 noiseScale, int spriteCount, bool unscaledTime = false)
    {
        noise += new Vector2((transform.position.x / 100) * noiseScale.x, (transform.position.y / 100) * noiseScale.y);
        animationFrame = Mathf.Clamp(Mathf.PerlinNoise(noise.x, noise.y) * spriteCount, 0, spriteCount - 1);
    }
}