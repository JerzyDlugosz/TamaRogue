using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FoodItem", menuName = "ScriptableObjects/FoodItem", order = 1)]
public class FoodItem : ScriptableObject
{
    public string id;
    public FoodObjectData foodData;
    public List<ComplexAnimationFrame> baseSprite;
    public List<ComplexAnimationFrame> selectedSprites;
 
    public bool additionalEffect;
    public bool affectsItself;
    public Vector2Int affectedFoodItems;
    public FoodObjectData additionalBuffToAffected;
    public FoodObjectData multiplicativeBuffToAffected;

}
