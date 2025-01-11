using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ReturnToOriginalPositionTask : Node
{
    private Transform _transform;
    private Vector3 _originalPosition;

    public ReturnToOriginalPositionTask(Transform transform, Vector3 originalPosition)
    {
        _transform = transform;
        _originalPosition = originalPosition;
    }

    public override NodeStatus Evaluate()
    {
        float distance = Vector3.Distance(_transform.position, _originalPosition);

        if (distance > 0.1f)
        {
            // Move the guard towards the original position
            _transform.position = Vector3.MoveTowards(_transform.position, _originalPosition, Guard.speed * Time.deltaTime);

            // Rotate towards the original position
            Vector3 direction = _originalPosition - _transform.position;
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            state = NodeStatus.RUNNING;
            return state;
        }

        // Guard has reached its original position
        Debug.Log("Guard has returned to its original position.");
        state = NodeStatus.SUCCES;
        return state;
    }
}
