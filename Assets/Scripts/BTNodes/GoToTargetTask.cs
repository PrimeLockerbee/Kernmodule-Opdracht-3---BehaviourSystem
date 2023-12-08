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

        if(Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, Guard.speed * Time.deltaTime);
            _transform.LookAt(target.position);
        }

        state = NodeStatus.RUNNING;
        return state;
    }
}
