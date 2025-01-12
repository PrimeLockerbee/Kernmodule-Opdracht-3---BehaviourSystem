using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform _guarddTransform;
    private Animator _animator;
    private Transform _target;

    public CheckEnemyInAttackRange(Transform transform, Animator animator, Transform target)
    {
        _guarddTransform = transform;
        _animator = animator;
        _target = target;
    }

    public override NodeStatus Evaluate()
    {
        if (_target == null)
        {
            Debug.LogWarning("Target is not assigned or null.");
            state = NodeStatus.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(_guarddTransform.position, _target.position);

        //Debug.Log($"Distance to target: {distance}");

        if (distance <= Guard._attackRange)
        {
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Walking", false);

            state = NodeStatus.SUCCES;
            return state;
        }

        _animator.SetBool("Attacking", false);
        _animator.SetBool("Walking", true);
        state = NodeStatus.FAILURE;
        return state;
    }
}
