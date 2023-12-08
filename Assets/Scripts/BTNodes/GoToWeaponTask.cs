using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToWeaponTask : Node
{
    private Transform _transform;

    public GoToWeaponTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Guard.fovRange);
            if (colliders.Length > 0)
            {
                // Check if the collider has the "Weapon" tag
                if (colliders[0].CompareTag("Weapon"))
                {
                    parent.parent.SetData("target", colliders[0].transform);
                    state = NodeStatus.RUNNING; // Change to RUNNING to indicate ongoing movement
                    return state;
                }
            }

            state = NodeStatus.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, Guard.speed * Time.deltaTime);
            _transform.LookAt(target.position);
        }
        else
        {
            state = NodeStatus.SUCCES;
        }

        return state;
    }

}
