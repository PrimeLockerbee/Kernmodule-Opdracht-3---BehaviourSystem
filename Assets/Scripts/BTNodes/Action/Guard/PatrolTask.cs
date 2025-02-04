using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PatrolTask : Node
{
    private Transform _transform;
    private Animator _animator;
    private Transform[] _waypoints;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public PatrolTask(Transform transform, Transform[] waypoints, Animator animator)
    {
        _transform = transform;
        _waypoints = waypoints;
        _animator = animator;
    }

    public override NodeStatus Evaluate()
    {
        _animator.SetBool("Walking", true);
        _animator.SetBool("Attacking", false);

        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if(_waitCounter >= _waitTime)
            {
                _waiting = false;
                _animator.SetBool("Walking", true);
                _animator.SetBool("Attacking", false);
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _animator.SetBool("Walking", false);
                _animator.SetBool("Attacking", false);
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, Guard._speed * Time.deltaTime);
                _transform.LookAt(wp.position);
            }
        }

        state = NodeStatus.RUNNING;
        return state;
    }
}
