using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PickUpWeaponTask : Node
{
    private Transform _transform;

    private bool hasPickedUpWeapon = false;

    public PickUpWeaponTask()
    {
    }

    public override NodeStatus Evaluate()
    {
        if (!hasPickedUpWeapon)
        {
            hasPickedUpWeapon = true;
            state = NodeStatus.SUCCES;
            return state;
        }

        state = NodeStatus.SUCCES;
        return state;
    }
}
