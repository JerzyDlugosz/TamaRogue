using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAiTargeter : MonoBehaviour
{
    [SerializeField]
    AIDestinationSetter destinationSetter;
    [SerializeField]
    private destinationTarget destinationTarget;
    [SerializeField]
    private Transform target;

    public bool constantTargetChecking = false;


    public void SetDestinationTarget(destinationTarget _destinationTarget)
    {
        destinationTarget = _destinationTarget;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}

public enum destinationTarget
{
    None,
    Player,
    PlayerSides,
    Target
}
