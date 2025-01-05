using DG.Tweening.Plugins.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[CustomEditor(typeof(SpriteAnimations))]
public class SpriteAnimationsInspector : Editor
{

    public VisualTreeAsset m_InspectorXML;
    public VisualTreeAsset spriteTemplate;
    public VisualTreeAsset animationLabel;

    SpriteAnimations spriteAnimations;

    public override VisualElement CreateInspectorGUI()
    {
        serializedObject.Update();

        VisualElement myInspector = new VisualElement();

        spriteAnimations = (SpriteAnimations)target;

        m_InspectorXML.CloneTree(myInspector);

        //InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);

        SetupAnimationTypeVisibility(myInspector);

        SetupToggle(myInspector);

        SetupAnimationLists(myInspector);

        SetupButtons(myInspector);

        serializedObject.ApplyModifiedProperties();

        var uxmlShowEventsToggle = myInspector.Q<Toggle>("ShowEvents");
        uxmlShowEventsToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            EventVisibility(myInspector, evt.newValue);
        });

        return myInspector;
    }

    void EventVisibility(VisualElement myInspector, bool val)
    {
        if (val)
        {
            myInspector.Query(className: "EventFieldClass").ForEach((element) =>
            {
                element.style.display = DisplayStyle.Flex;
            });
        }
        else
        {
            myInspector.Query(className: "EventFieldClass").ForEach((element) =>
            {
                element.style.display = DisplayStyle.None;
            });
        }
    }


    private void SetupAnimationLists(VisualElement myInspector)
    {
        var uxmlIdleAnim = myInspector.Q<VisualElement>("IdleAnim");
        var uxmlMoveAnim = myInspector.Q<VisualElement>("MoveAnim");
        var uxmlAttackAnim = myInspector.Q<VisualElement>("AttackAnim");
        var uxmlOnHitAnim = myInspector.Q<VisualElement>("OnHitAnim");
        var uxmlOnDeathAnim = myInspector.Q<VisualElement>("OnDeathAnim");
        var uxmlSpecialAnim = myInspector.Q<VisualElement>("SpecialAnim");

        var uxmlPullIdleAnim = myInspector.Q<VisualElement>("PullIdleAnim");
        var uxmlPushIdleAnim = myInspector.Q<VisualElement>("PushIdleAnim");
        var uxmlPullMoveAnim = myInspector.Q<VisualElement>("PullMoveAnim");
        var uxmlPushMoveAnim = myInspector.Q<VisualElement>("PushMoveAnim");
        var uxmlHoldIdleAnim = myInspector.Q<VisualElement>("HoldIdleAnim");
        var uxmlHoldMoveAnim = myInspector.Q<VisualElement>("HoldMoveAnim");

        List<VisualElement> visualElements = new List<VisualElement> { uxmlIdleAnim, uxmlMoveAnim, uxmlAttackAnim, uxmlOnHitAnim, uxmlOnDeathAnim, uxmlSpecialAnim,
        uxmlPullIdleAnim, uxmlPushIdleAnim, uxmlPullMoveAnim, uxmlPushMoveAnim, uxmlHoldIdleAnim, uxmlHoldMoveAnim};

        List<FullAnimation> fullAnimations = new List<FullAnimation> { spriteAnimations.idleAnimations, spriteAnimations.movingAnimations, spriteAnimations.attackAnimations, 
            spriteAnimations.onHitAnimations, spriteAnimations.onDeathAnimations, spriteAnimations.specialAnimations,
            spriteAnimations.pullIdleAnimations, spriteAnimations.pushIdleAnimations, spriteAnimations.pullMovingAnimations,
            spriteAnimations.pushMovingAnimations, spriteAnimations.holdingIdleAnimations, spriteAnimations.holdingMovingAnimations
        };

        List<String> fullAnimationsString = new List<string> { "idleAnimations", "movingAnimations", "attackAnimations", "onHitAnimations", "onDeathAnimations", "specialAnimations",
            "pullIdleAnimations", "pushIdleAnimations", "pullMovingAnimations", "pushMovingAnimations", "holdingIdleAnimations", "holdingMovingAnimations"
        };

        for (int k = 0; k < visualElements.Count; k++)
        {
            for (int i = 0; i < fullAnimations[k].animations.Count; i++)
            {
                VisualElement visualElement = new VisualElement();
                var animLabel = animationLabel.CloneTree();
                var animSpeedfloatField = animLabel.Q<FloatField>("SpeedModifier");

                animSpeedfloatField.bindingPath = $"{fullAnimationsString[k]}.animations.Array.data[{i}].animationSpeedModifier";
                animSpeedfloatField.label = $"Anim {i} Speed Modifier";

                visualElements[k].Add(animLabel);
                for (int j = 0; j < fullAnimations[k].animations[i].directionalAnimations[0].frames.Count; j++)
                {
                    var Tree = spriteTemplate.CloneTree();
                    var objectField = Tree.Q<ObjectField>("Object");
                    objectField.bindingPath = $"{fullAnimationsString[k]}.animations.Array.data[{i}].directionalAnimations.Array.data[0].frames.Array.data[{j}].sprite";
                    var floatField = Tree.Q<FloatField>("Float");
                    floatField.bindingPath = $"{fullAnimationsString[k]}.animations.Array.data[{i}].directionalAnimations.Array.data[0].frames.Array.data[{j}].timeOfSprite";
                    var eventField = Tree.Q<PropertyField>("EventField");
                    eventField.bindingPath = $"{fullAnimationsString[k]}.animations.Array.data[{i}].directionalAnimations.Array.data[0].frames.Array.data[{j}].additionalEvent";
                    var numberLabel = Tree.Q<Label>("NumberLabel");
                    numberLabel.text = $"{j}";
                    visualElement.Add(Tree);
                    visualElements[k].Add(visualElement);
                }

                VisualElement border = new VisualElement();
                border.style.borderBottomColor = new StyleColor(Color.gray);
                border.style.borderBottomWidth = 1f;
                visualElements[k].Add(border);
            }
        }

        var uxmlShowEventsToggle = myInspector.Q<Toggle>("ShowEvents");
        EventVisibility(myInspector, uxmlShowEventsToggle.value);

        var uxmlPlayerButtonsBox = myInspector.Q<VisualElement>("PlayerButtons");
        var uxmlPlayerLabels = myInspector.Q<VisualElement>("PlayerLabels");
        var uxmlPlayerCount = myInspector.Q<VisualElement>("PlayerCount");
        var uxmlPlayerTextures = myInspector.Q<VisualElement>("PlayerTextures");
        var uxmlPlayerAnims = myInspector.Q<VisualElement>("PlayerAnims");

        SetupVisualElements(uxmlPlayerButtonsBox, uxmlPlayerLabels, uxmlPlayerCount, uxmlPlayerTextures, uxmlPlayerAnims);
    }

    private void SetupAnimationTypeVisibility(VisualElement myInspector)
    {
        var uxmlField = myInspector.Q<EnumField>("AnimationEnum");

        uxmlField.value = spriteAnimations.animationType;

        var uxmlPlayerButtonsBox = myInspector.Q<VisualElement>("PlayerButtons");
        var uxmlPlayerLabels = myInspector.Q<VisualElement>("PlayerLabels");
        var uxmlPlayerCount = myInspector.Q<VisualElement>("PlayerCount");
        var uxmlPlayerTextures = myInspector.Q<VisualElement>("PlayerTextures");
        var uxmlPlayerAnims = myInspector.Q<VisualElement>("PlayerAnims");

        uxmlField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            SetupVisualElements(uxmlPlayerButtonsBox, uxmlPlayerLabels, uxmlPlayerCount, uxmlPlayerTextures, uxmlPlayerAnims);
            Debug.Log($"Value Changed to {evt.newValue}");
        });

    }

    private void SetupToggle(VisualElement myInspector)
    {
        var uxmlshowAnimationsToggle = myInspector.Q<Toggle>("showAnimToggle");
        var uxmlanimationsBox = myInspector.Q<VisualElement>("animations");
        uxmlshowAnimationsToggle.RegisterValueChangedCallback(OnToggle);
        void OnToggle(ChangeEvent<bool> evt)
        {
            Debug.Log($"Toggled to {evt.newValue}");
            if (evt.newValue)
                uxmlanimationsBox.style.display = DisplayStyle.Flex;
            else
                uxmlanimationsBox.style.display = DisplayStyle.None;
        }
    }

    private void SetupButtons(VisualElement myInspector)
    {
        var autoSpriteIdleButton = myInspector.Q<Button>("autoSpriteIdle");
        autoSpriteIdleButton.clicked += AutoSpriteIdleButton_clicked;

        var autoSpriteMovementButton = myInspector.Q<Button>("autoSpriteMovement");
        autoSpriteMovementButton.clicked += AutoSpriteMovementButton_clicked;

        var autoSpriteAttackButton = myInspector.Q<Button>("autoSpriteAttack");
        autoSpriteAttackButton.clicked += AutoSpriteAttackButton_clicked;

        var autoSpriteOnHitButton = myInspector.Q<Button>("autoSpriteOnHit");
        autoSpriteOnHitButton.clicked += AutoSpriteOnHitButton_clicked;

        var autoSpriteOnDeathButton = myInspector.Q<Button>("autoSpriteOnDeath");
        autoSpriteOnDeathButton.clicked += AutoSpriteOnDeathButton_clicked;

        var autoSpriteSpecialButton = myInspector.Q<Button>("autoSpriteSpecial");
        autoSpriteSpecialButton.clicked += AutoSpriteSpecialButton_clicked;

        var autoSpritePushPullButton = myInspector.Q<Button>("autoSpritePushPull");
        autoSpritePushPullButton.clicked += AutoSpritePushPullButton_clicked;

        var autoSpriteHoldButton = myInspector.Q<Button>("autoSpriteHold");
        autoSpriteHoldButton.clicked += AutoSpriteHoldButton_clicked;

        var autoSpriteAllButton = myInspector.Q<Button>("autoSpriteAll");
        autoSpriteAllButton.clicked += AutoSpriteAllButton_clicked;

        var fillTimesButton = myInspector.Q<Button>("fillTimes");
        fillTimesButton.clicked += FillTimesButton_clicked;
    }

    private void FillTimesButton_clicked()
    {
        spriteAnimations.SetTimeofSpriteOfAllSprites();
    }

    private void AutoSpriteAllButton_clicked()
    {
        Undo.RecordObject(spriteAnimations, $"Set spriteAnimations");
        spriteAnimations.SetupAllAnimations();
    }

    private void AutoSpriteOnDeathButton_clicked()
    {
        spriteAnimations.SetupOnDeathAnimations();
    }

    private void AutoSpriteOnHitButton_clicked()
    {
        spriteAnimations.SetupOnHitAnimations();
    }

    private void AutoSpriteMovementButton_clicked()
    {
        spriteAnimations.SetupMovingAnimations();
    }

    private void AutoSpriteIdleButton_clicked()
    {
        spriteAnimations.SetupIdleAnimations();
    }

    private void AutoSpriteHoldButton_clicked()
    {
        spriteAnimations.SetupPlayerHoldingAnimations();
    }

    private void AutoSpritePushPullButton_clicked()
    {
        spriteAnimations.SetupPlayerPushPullAnimations();
    }

    private void SetupVisualElements(VisualElement playerButtons, VisualElement playerLabels, VisualElement playerCount, VisualElement playerTextures, VisualElement uxmlPlayerAnims)
    {
        if (spriteAnimations.animationType == NPCAnimationType.Player)
        {
            playerButtons.style.display = DisplayStyle.Flex;
            playerLabels.style.display = DisplayStyle.Flex;
            playerCount.style.display = DisplayStyle.Flex;
            playerTextures.style.display = DisplayStyle.Flex;
            uxmlPlayerAnims.style.display = DisplayStyle.Flex;
        }
        else
        {
            playerButtons.style.display = DisplayStyle.None;
            playerLabels.style.display = DisplayStyle.None;
            playerCount.style.display = DisplayStyle.None;
            playerTextures.style.display = DisplayStyle.None;
            uxmlPlayerAnims.style.display = DisplayStyle.None;
        }
    }

    private void AutoSpriteAttackButton_clicked()
    {
        spriteAnimations.SetupAttackAnimations();
    }

    private void AutoSpriteSpecialButton_clicked()
    {
        spriteAnimations.SetupSpecialAnimations();
    }

}
