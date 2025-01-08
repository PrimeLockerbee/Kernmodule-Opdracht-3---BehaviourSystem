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
        Debug.Log(hasPickedUpWeapon);

        if (!hasPickedUpWeapon)
        {
            hasPickedUpWeapon = true;

            Debug.Log(hasPickedUpWeapon);

            state = NodeStatus.SUCCES;
            return state;
        }

        state = NodeStatus.SUCCES;
        return state;
    }
}
