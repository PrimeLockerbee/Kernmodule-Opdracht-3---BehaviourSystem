using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckEnemyInAttackRange : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;
    private Animator _animator;

    public CheckEnemyInAttackRange(Transform transform, Animator animator)
    {
        _transform = transform;
        _animator = animator;
    }

    public override NodeStatus Evaluate()
    {
        object t = GetData("target");
        Debug.Log(t);

        if (t == null)
        {
            state = NodeStatus.FAILURE;
            return state;
        }



        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) <= Guard.attackRange)
        {
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Walking", false);
            state = NodeStatus.SUCCES;
            return state;
        }

        state = NodeStatus.FAILURE;
        return state;
    }
}
