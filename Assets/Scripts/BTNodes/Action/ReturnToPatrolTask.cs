using BehaviourTree;
using UnityEngine;

public class ReturnToPatrolTask : Node
{
    private Transform _transform;
    private Vector3 _initialPosition;

    public ReturnToPatrolTask(Transform transform, Vector3 initialPosition)
    {
        _transform = transform;
        _initialPosition = initialPosition;
    }

    public override NodeStatus Evaluate()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _initialPosition, Guard.speed * Time.deltaTime);
        if (Vector3.Distance(_transform.position, _initialPosition) < 0.1f)
        {
            state = NodeStatus.SUCCES;  // Reached the initial position, success
            return state;
        }

        state = NodeStatus.RUNNING;  // Still moving back
        return state;
    }
}