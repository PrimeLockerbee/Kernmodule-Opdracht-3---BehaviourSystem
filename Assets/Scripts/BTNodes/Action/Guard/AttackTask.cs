using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class AttackTask : Node
{
    private Transform _transform;  // The guard's transform
    private Animator _animator;
    private Transform _playerTransform; // Reference to the player's transform
    private Player _player;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public AttackTask(Transform transform, Animator animator, Transform playerTransform)
    {
        _transform = transform;         // The guard's transform
        _animator = animator;           // Animator for attack animations
        _playerTransform = playerTransform; // Direct reference to the player's transform
        _player = playerTransform.GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("AttackTask: Player transform does not have a Player component!");
        }
    }

    public override NodeStatus Evaluate()
    {
        if (_player == null)
        {
            Debug.LogError("AttackTask: No valid Player component to attack.");
            state = NodeStatus.FAILURE;
            return state;
        }

        // Calculate distance to the player
        float distance = Vector3.Distance(_transform.position, _playerTransform.position);
        if (distance > Guard.attackRange)
        {
            Debug.Log("AttackTask: Player is out of attack range.");
            state = NodeStatus.FAILURE;
            return state;
        }

        // Increment attack timer
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            // Perform the attack
            //Debug.Log($"AttackTask: Attacking player at {_playerTransform.position}.");
            bool playerIsDead = _player.TakeHit();

            if (playerIsDead)
            {
                Debug.Log("AttackTask: Player is dead. Stopping attack.");
                _animator.SetBool("Attacking", false);
                _animator.SetBool("Walking", true);
                state = NodeStatus.SUCCES;
                return state;
            }
            else
            {
                Debug.Log("AttackTask: Player hit but still alive. Resetting attack counter.");
                _attackCounter = 0f;
            }
        }

        // Continue attacking
        state = NodeStatus.RUNNING;
        return state;
    }
}
