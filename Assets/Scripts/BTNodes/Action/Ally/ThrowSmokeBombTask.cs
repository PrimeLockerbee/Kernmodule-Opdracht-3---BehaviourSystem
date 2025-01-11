using BehaviourTree;
using UnityEngine;

public class ThrowSmokeBombTask : Node
{
    private Transform _transform;

    public ThrowSmokeBombTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        // Logic to throw a smoke bomb at the enemy
        // For now, this is a placeholder
        Debug.Log("Throwing smoke bomb!");

        // You could create a visual effect or trigger an event for the enemy to be confused here
        state = NodeStatus.SUCCES;
        return state;
    }
}
