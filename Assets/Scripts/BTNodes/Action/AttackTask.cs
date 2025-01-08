using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class AttackTask : Node
{
    private Animator _animator;

    private Transform _lastTarget;
    private Player _Player;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public AttackTask(Transform transform, Animator animator)
    {
        _animator = animator;
    }

    public override NodeStatus Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _Player = target.GetComponent<Player>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime)
        {
            bool playerIsDead = _Player.TakeHit();
            if(playerIsDead)
            {
                ClearData("target");
                _animator.SetBool("Attacking", false);
                _animator.SetBool("Walking", true);
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = NodeStatus.RUNNING;
        return state;
    }
}
