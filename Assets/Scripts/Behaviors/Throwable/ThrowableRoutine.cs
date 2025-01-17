using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableRoutine : BehaviorRoutine
{
    public override BehaviorState StartState {
        get => new ThrowSelect();
    }

    /**
        Temporaries
    **/
    private int _searchRange;
    public int SearchRange {
        get => _searchRange;
        set => _searchRange = value;
    }

    
    public override void GetTemporaries()
    {
        SearchRange = (behavior as Throwable).Range;
        Pathfinder.moveLimit = SearchRange;
    }
    public override void SetTemporaries()
    {
        Debug.Log("Bro are you running");
        // (behavior as Moveable).Movement = TempMovement;
        // Debug.Log($"Movement: {(behavior as Moveable).Movement}");
        Pathfinder.moveLimit = Mathf.Infinity;
    }
}
