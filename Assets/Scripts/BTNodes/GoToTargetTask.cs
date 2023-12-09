using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToTargetTask : Node
{
    private Transform _transform;

    public GoToTargetTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        Transform target = (Transform)GetData("target");

        Debug.Log("GoToTargetTask: Evaluating. Target: " + target);

        if (target != null)
        {
            float distance = Vector3.Distance(_transform.position, target.position);
            Debug.Log("GoToTargetTask: Distance to target: " + distance);

            if (distance > 0.01f)
            {
                Debug.Log("GoToTargetTask: Moving towards target...");
                _transform.position = Vector3.MoveTowards(_transform.position, target.position, Guard.speed * Time.deltaTime);
                _transform.LookAt(target.position);
            }
        }

        state = NodeStatus.RUNNING;
        return state;
    }
}
