using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class AttackTask : Node
{
    private Transform _guardTransform;
    private Animator _animator;
    private Transform _playerTransform;
    private Player _player;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public AttackTask(Transform transform, Animator animator, Transform playerTransform)
    {
        _guardTransform = transform;
        _animator = animator;
        _playerTransform = playerTransform;
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

        float distance = Vector3.Distance(_guardTransform.position, _playerTransform.position);
        if (distance > Guard._attackRange)
        {
            //Debug.Log("AttackTask: Player is out of attack range.");
            state = NodeStatus.FAILURE;
            return state;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            //Debug.Log($"AttackTask: Attacking player at {_playerTransform.position}.");

            bool playerIsDead = _player.TakeHit();
            _animator.SetBool("Attacking", false);
            _animator.SetBool("Walking", true);
            _attackCounter = 0f;
            state = NodeStatus.SUCCES;
            return state;
        }

        state = NodeStatus.RUNNING;
        return state;
    }
}
