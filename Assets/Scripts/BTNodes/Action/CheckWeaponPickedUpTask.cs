using BehaviourTree;
using UnityEngine;

public class CheckWeaponPickedUpTask : Node
{
    private Transform _transform;

    public CheckWeaponPickedUpTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        Guard guard = _transform.GetComponent<Guard>();

        if (guard == null)
        {
            Debug.LogWarning("Guard component not found.");
            state = NodeStatus.FAILURE;
            return state;
        }

        // If the guard has picked up the weapon, return success
        if (guard.hasWeapon)
        {
            Debug.Log("Guard has picked up the weapon, ready to move to the target.");
            state = NodeStatus.SUCCES;
            return state;
        }
        else
        {
            // Fail this task, so the tree will check other nodes or retry
            Debug.Log("Guard hasn't picked up the weapon yet.");
            state = NodeStatus.FAILURE;
            return state;
        }
    }
}
