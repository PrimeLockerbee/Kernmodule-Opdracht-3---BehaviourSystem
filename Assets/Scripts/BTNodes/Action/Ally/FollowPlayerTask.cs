using BehaviourTree;
using UnityEngine;

public class FollowPlayerTask : Node
{
    private Animator _animator;
    private Transform _transform;
    private Transform _playerTransform;
    private float _followRange = 2f;  // Distance at which the Ally will stop following

    public FollowPlayerTask(Transform transform, Transform playerTransform, Animator animator)
    {
        _transform = transform;
        _playerTransform = playerTransform;
        _animator = animator;
    }

    public override NodeStatus Evaluate()
    {
        // Calculate the distance to the player
        float distance = Vector3.Distance(_transform.position, _playerTransform.position);

        // Determine if the Ally is moving
        bool isMoving = distance > _followRange;

        // Trigger animations based on movement state
        if (isMoving)
        {
            // If the Ally is moving, play the "Walk Crouch" animation (or any other animation you want)
            ChangeAnimation("Walk Crouch", .0001f);

            // Move the Ally towards the player
            _transform.position = Vector3.MoveTowards(_transform.position, _playerTransform.position, 2.5f * Time.deltaTime);

            // Smoothly rotate the Ally to face the player
            Vector3 direction = (_playerTransform.position - _transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 5f);

            state = NodeStatus.RUNNING;
        }
        else
        {
            // If the Ally is within follow range, stop the walking animation
            ChangeAnimation("Crouch Idle", 0.15f);

            state = NodeStatus.SUCCES;
        }

        return state;
    }

    // Helper method to change animations based on the condition
    private void ChangeAnimation(string animationName, float speed)
    {
        _animator.CrossFade(animationName, speed);
    }
}
