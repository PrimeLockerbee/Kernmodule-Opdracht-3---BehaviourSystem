using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerTask : Node
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform _playerTransform;

    private float _followRange = 2f;
    private static readonly int WalkingHash = Animator.StringToHash("Walking");

    public FollowPlayerTask(NavMeshAgent navMeshAgent, Transform playerTransform, Animator animator)
    {
        _navMeshAgent = navMeshAgent;
        _playerTransform = playerTransform;
        _animator = animator;
    }

    public override NodeStatus Evaluate()
    {
        if (_playerTransform == null)
        {
            Debug.LogError("Player Transform is null in FollowPlayerTask!");
            state = NodeStatus.FAILURE;
            return state;
        }

        //Calculate the distance to the player
        float distance = Vector3.Distance(_navMeshAgent.transform.position, _playerTransform.position);

        if (distance > _followRange)
        {
            //Reset path before setting a new destination to avoid wrong destinations
            _navMeshAgent.ResetPath();
            _navMeshAgent.SetDestination(_playerTransform.position);

            //Set the Walking boolean to true if the NavMeshAgent is moving
            bool isWalking = _navMeshAgent.velocity.sqrMagnitude > 0.01f;
            _animator.SetBool(WalkingHash, isWalking);

            state = NodeStatus.RUNNING;
        }
        else
        {
            //If within follow range, stop moving
            _navMeshAgent.ResetPath();

            //Set the Walking boolean to false
            _animator.SetBool(WalkingHash, false);

            state = NodeStatus.SUCCES;
        }

        return state;
    }
}
