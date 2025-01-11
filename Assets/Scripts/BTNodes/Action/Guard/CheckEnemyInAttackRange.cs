using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckEnemyInAttackRange : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;  // Reference to the guard's transform
    private Animator _animator;    // Reference to the guard's animator
    private Transform _target;     // Reference to the player's transform

    public CheckEnemyInAttackRange(Transform transform, Animator animator, Transform target)
    {
        _transform = transform;  // Guard's transform
        _animator = animator;    // Guard's animator
        _target = target;        // Player's transform (target)
    }

    public override NodeStatus Evaluate()
    {
        // Check if the player (target) is assigned
        if (_target == null)
        {
            Debug.LogWarning("Target is not assigned or null.");
            state = NodeStatus.FAILURE;
            return state;
        }

        // Calculate distance between the guard and the target (player)
        float distance = Vector3.Distance(_transform.position, _target.position);

        Debug.Log($"Distance to target: {distance}");

        // Check if the target is within attack range
        if (distance <= Guard.attackRange)
        {
            // Trigger attack animations
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Walking", false);

            state = NodeStatus.SUCCES;
            return state;
        }

        // If the target is out of range, return failure
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Walking", true);
        state = NodeStatus.FAILURE;
        return state;
    }
}
