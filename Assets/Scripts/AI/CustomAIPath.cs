using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAIPath : AIPath
{
    private PetInGameController movingNPC;

    protected override void Start()
    {
        base.Start();
        movingNPC = GetComponent<PetInGameController>();
    }

    public override void OnTargetReached()
    {
        movingNPC.onTargetReached.Invoke();
    }
}
