using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteAnimations : MonoBehaviour
{
    public FullAnimation movingAnimations;
    public FullAnimation idleAnimations;
    public FullAnimation attackAnimations;
    public FullAnimation onHitAnimations;
    public FullAnimation onDeathAnimations;
    public FullAnimation specialAnimations;
    public FullAnimation pushIdleAnimations;
    public FullAnimation pullIdleAnimations;
    public FullAnimation pushMovingAnimations;
    public FullAnimation pullMovingAnimations;
    public FullAnimation holdingIdleAnimations;
    public FullAnimation holdingMovingAnimations;

    public NPCAnimationType animationType;


    private List<Sprite> autoSprites = new List<Sprite>();


#if (UNITY_EDITOR)
    public Texture2D idleTexture;
    public int idleAnimationsCount;

    public Texture2D movementTexture;
    public int movingAnimationsCount;

    public Texture2D attacksTexture;
    public int attacksAnimationsCount;

    public Texture2D onHitTexture;
    public int onHitAnimationsCount = 1;

    public Texture2D onDeathTexture;
    public int onDeathAnimationsCount = 1;

    public Texture2D specialTexture;
    public int specialAnimationsCount;

    public Texture2D playerPushIdleTexture;
    public Texture2D playerPullIdleTexture;
    public Texture2D playerPushMovingTexture;
    public Texture2D playerPullMovingTexture;
    public int playerPushPullAnimationsCount;

    public Texture2D playerHoldingIdleTexture;
    public Texture2D playerHoldingMovingTexture;
    public int playerHoldingAnimationsCount;

    private int previousRowIndex;
    private int spriteIndex;

    private bool FillAutoSprites(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("No texture file specified!");
            return false;
        }

        //This is unsorted btw. Valve, pls fix
        UnityEngine.Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));

        autoSprites.Clear();

        //Have you heard about S O R T I N G? 
        Dictionary<Sprite, int> autoSpritesDictionary = new Dictionary<Sprite, int>();

        foreach (var item in data)
        {
            if (item.GetType() == typeof(Sprite))
            {
                var num = item.name.IndexOf("_");
                string afterThis = item.name.Substring(num + 1);

                Sprite sprite = item as Sprite;
                autoSpritesDictionary.Add(sprite, int.Parse(afterThis));
            }
        }

        var myList = autoSpritesDictionary.ToList();

        myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        foreach (var item in myList)
        {
            autoSprites.Add(item.Key);
        }

        return true;
    }

    public FullAnimation SetupAnimations(Texture2D texture, int animationCount, NPCAnimationType _animationType)
    {
        if (!FillAutoSprites(texture))
            return null;

        previousRowIndex = 0;
        spriteIndex = 0;

        FullAnimation fullAnimation = new FullAnimation();

        int currentRowIndex = (int)(autoSprites[0].rect.yMin / autoSprites[0].rect.size.y);
        previousRowIndex = currentRowIndex;

        int rowMax = (int)(texture.width / autoSprites[spriteIndex].rect.size.x) + 1;

        for (int j = 0; j < animationCount; j++)
        {
            fullAnimation.animations.Add(new SpriteAnimationList());
            switch (_animationType)
            {
                //When animationType is FourDirections, add and apply sprites to E, W animations, in that order
                case NPCAnimationType.TwoDirections:
                    for (int i = 0; i < 2; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                //When animationType is FourDirections, add and apply sprites to N, E, S, W animations, in that order
                case NPCAnimationType.FourDirections:
                    for (int i = 0; i < 4; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                //When animationType is Player, add and apply sprites to N, NE, E, SE, S, SW, W, NW animations, in that order
                case NPCAnimationType.Player:
                    for (int i = 0; i < 8; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                //When animationType is NoDirection, add and apply sprites to only one animation
                case NPCAnimationType.NoDirection:
                    for (int i = 0; i < 1; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                default:
                    break;
            }
        }

        return fullAnimation;
    }

    public FullAnimation SetupAnimations(Texture2D texture, int animationCount)
    {
        if (!FillAutoSprites(texture))
            return null;

        previousRowIndex = 0;
        spriteIndex = 0;

        FullAnimation fullAnimation = new FullAnimation();

        int currentRowIndex = (int)(autoSprites[0].rect.yMin / autoSprites[0].rect.size.y);
        previousRowIndex = currentRowIndex;

        int rowMax = (int)(texture.width / autoSprites[spriteIndex].rect.size.x) + 1;

        for (int j = 0; j < animationCount; j++)
        {
            fullAnimation.animations.Add(new SpriteAnimationList());
            switch (animationType)
            {
                //When animationType is FourDirections, add and apply sprites to E, W animations, in that order
                case NPCAnimationType.TwoDirections:
                    for (int i = 0; i < 2; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                //When animationType is FourDirections, add and apply sprites to N, E, S, W animations, in that order
                case NPCAnimationType.FourDirections:
                    for (int i = 0; i < 4; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                //When animationType is Player, add and apply sprites to N, NE, E, SE, S, SW, W, NW animations, in that order
                case NPCAnimationType.Player:
                    for (int i = 0; i < 8; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                //When animationType is NoDirection, add and apply sprites to only one animation
                case NPCAnimationType.NoDirection:
                    for (int i = 0; i < 1; i++)
                    {
                        ApplySprites(fullAnimation.animations[j].directionalAnimations, rowMax);
                    }
                    break;
                default:
                    break;
            }
        }
        return fullAnimation;
    }


    public void SetupMovingAnimations()
    {
        movingAnimations = SetupAnimations(movementTexture, movingAnimationsCount);

        Debug.Log("Auto applied movement sprites");
    }

    public void SetupIdleAnimations()
    {
        idleAnimations = SetupAnimations(idleTexture, idleAnimationsCount);

        Debug.Log("Auto applied idle sprites");
    }

    public void SetupAttackAnimations()
    {
        attackAnimations = SetupAnimations(attacksTexture, attacksAnimationsCount);

        Debug.Log("Auto applied attack sprites");
    }
    public void SetupOnDeathAnimations()
    {
        onDeathAnimations = SetupAnimations(onDeathTexture, onDeathAnimationsCount);

        Debug.Log("Auto applied OnHit and Death sprites");
    }
    public void SetupOnHitAnimations()
    {
        onHitAnimations = SetupAnimations(onHitTexture, onHitAnimationsCount);

        Debug.Log("Auto applied OnHit and Death sprites");
    }

    public void SetupSpecialAnimations()
    {
        specialAnimations = SetupAnimations(specialTexture, specialAnimationsCount);

        Debug.Log("Auto applied Special sprites");
    }

    public void SetupAllAnimations()
    {
        string appliedSpritesShort = string.Empty;
        if(idleTexture != null)
        {
            idleAnimations = SetupAnimations(idleTexture, idleAnimationsCount);
            appliedSpritesShort += "I";
        }
        if (movementTexture != null)
        {
            movingAnimations = SetupAnimations(movementTexture, movingAnimationsCount);
            appliedSpritesShort += "/M";
        }
        if (attacksTexture != null)
        {
            attackAnimations = SetupAnimations(attacksTexture, attacksAnimationsCount);
            appliedSpritesShort += "/A";
        }
        if (onHitTexture != null)
        {
            onHitAnimations = SetupAnimations(onHitTexture, onHitAnimationsCount);
            appliedSpritesShort += "/H";
        }
        if (onDeathTexture != null)
        {
            onDeathAnimations = SetupAnimations(onDeathTexture, onDeathAnimationsCount);
            appliedSpritesShort += "/D";
        }
        if (specialTexture != null)
        {
            specialAnimations = SetupAnimations(specialTexture, specialAnimationsCount);
            appliedSpritesShort += "/S";
        }

        Debug.Log($"Auto applied [{appliedSpritesShort}] sprites");
    }

    public void SetupPlayerPushPullAnimations()
    {
        //Too lazy to do it correctly
        pushIdleAnimations = SetupAnimations(playerPushIdleTexture, playerPushPullAnimationsCount, NPCAnimationType.FourDirections);
        pullIdleAnimations = SetupAnimations(playerPullIdleTexture, playerPushPullAnimationsCount, NPCAnimationType.FourDirections);
        pushMovingAnimations = SetupAnimations(playerPushMovingTexture, playerPushPullAnimationsCount, NPCAnimationType.FourDirections);
        pullMovingAnimations = SetupAnimations(playerPullMovingTexture, playerPushPullAnimationsCount, NPCAnimationType.FourDirections);
        Debug.Log("Auto applied Push/Pull sprites");
    }

    public void SetupPlayerHoldingAnimations()
    {
        holdingIdleAnimations = SetupAnimations(playerHoldingIdleTexture, playerHoldingAnimationsCount);
        holdingMovingAnimations = SetupAnimations(playerHoldingMovingTexture, playerHoldingAnimationsCount);
        Debug.Log("Auto applied Holding sprites");
    }

    private void ApplySprites(List<SpriteAnimation> spriteAnimationLists, int MaxSpritesInRow)
    {
        int currentRowIndex = 0;

        Sprite sprite;

        SpriteAnimation animList;
        ComplexAnimationFrame animationData;

        animList = new SpriteAnimation();
        animList.frames = new List<ComplexAnimationFrame>();

        for (int j = 0; j < MaxSpritesInRow; j++)
        {
            if (spriteIndex >= autoSprites.Count)
            {
                break;
            }

            sprite = autoSprites[spriteIndex];
            currentRowIndex = (int)(sprite.rect.yMin / sprite.rect.size.y);

            Debug.Log($"AutoSprite | current row: {currentRowIndex}, previous row: {previousRowIndex}");


            if (currentRowIndex == previousRowIndex)
            {
                Debug.Log($"AutoSprite | current row: {currentRowIndex}, sprite index: {spriteIndex}");

                animationData = new ComplexAnimationFrame();
                animationData.sprite = sprite;
                animList.frames.Add(animationData);
                spriteIndex++;
            }
            else
            {
                break;
            }
        }

        spriteAnimationLists.Add(animList);
        previousRowIndex = currentRowIndex;
    }

    public void SetTimeofSpriteOfAllSprites()
    {
        List<FullAnimation> fullAnimations = new List<FullAnimation> { idleAnimations, movingAnimations, attackAnimations,
            onHitAnimations, onDeathAnimations, specialAnimations,
            pullIdleAnimations, pushIdleAnimations, pullMovingAnimations,
            pushMovingAnimations, holdingIdleAnimations, holdingMovingAnimations
        };

        foreach (var animationType in fullAnimations)
        {
            foreach (var animation in animationType.animations)
            {
                for (int i = 1; i < animation.directionalAnimations.Count; i++)
                {
                    for (int j = 0; j < animation.directionalAnimations[i].frames.Count; j++)
                    {
                        animation.directionalAnimations[i].frames[j].timeOfSprite = animation.directionalAnimations[0].frames[j].timeOfSprite;
                    }
                }
            }
        }
    }
#endif
}

public enum NPCAnimationType
{
    TwoDirections = 0,
    FourDirections = 1,
    Player = 2,
    NoDirection = 3
}

[Serializable]
public class SpriteAnimation
{
    [SerializeField]
    public List<ComplexAnimationFrame> frames;
}

[Serializable]
public class SpriteAnimationList
{
    [SerializeField]
    public float animationSpeedModifier = 1f;
    /// <summary>
    /// <para>List of SpriteAnimation. Holds all directional animations of given animation</para> 
    /// <para>directions: TwoDirectional - E, W. FourDirectional - N, E, S, W. EightDirectional (Player) - N, NE, E, SE, S, SW, W, NW</para> 
    /// </summary>
    [SerializeField]
    public List<SpriteAnimation> directionalAnimations = new List<SpriteAnimation>();
}

[Serializable]
public class FullAnimation
{
    /// <summary>
    /// List of spriteAnimationList. Holds all animations of given type
    /// </summary>
    [SerializeField]
    public List<SpriteAnimationList> animations = new List<SpriteAnimationList>();
}