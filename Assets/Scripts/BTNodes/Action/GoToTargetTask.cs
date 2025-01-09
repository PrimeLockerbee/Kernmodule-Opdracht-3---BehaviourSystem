using BehaviourTree;
using UnityEngine;

public class GoToTargetTask : Node
{
    private Transform _transform;
    private Transform _target;

    public GoToTargetTask(Transform transform, Transform target)
    {
        _transform = transform;
        _target = target;
    }

    public override NodeStatus Evaluate()
    {
        Debug.Log("Is running");

        // Get the Guard component
        Guard guard = _transform.GetComponent<Guard>();
        if (guard == null)
        {
            Debug.LogWarning("Guard component not found on target.");
            state = NodeStatus.FAILURE;
            return state;
        }

        // Check if the guard has picked up the weapon
        if (!guard.hasWeapon)
        {
            // If the guard hasn't picked up the weapon yet, move to the next node in the tree (fail this task)
            Debug.Log("Guard hasn't picked up the weapon yet, moving to next task.");
            state = NodeStatus.FAILURE;  // Fail the task, causing the behavior tree to continue to the next node
            return state;
        }


        if (_target != null)
        {
            // Debugging: Log the target and the guard's position
            Debug.Log("Guard has the weapon. Moving towards target: " + _target.name + " | Distance: " + Vector3.Distance(_transform.position, _target.position));

            float distance = Vector3.Distance(_transform.position, _target.position);

            if (distance > 0.1f)  // We want to move until the guard is quite close to the target
            {
                // Move the guard towards the target
                _transform.position = Vector3.MoveTowards(_transform.position, _target.position, Guard.speed * Time.deltaTime);

                // Rotate the guard to face the target
                Vector3 direction = _target.position - _transform.position;
                direction.y = 0;  // Ensure only horizontal rotation (no tilting)
                if (direction.magnitude > 0.1f)  // Only rotate if there's movement
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                // Task is still running as the guard is moving
                state = NodeStatus.RUNNING;
                return state;
            }
            else
            {
                // If the guard has reached the target
                Debug.Log("Guard reached the target: " + _target.name);

                // Mark task as success, guard has reached the player
                state = NodeStatus.SUCCES;
            }
        }
        else
        {
            // If there's no target, fail the task
            Debug.LogWarning("No target found for the guard to move towards.");
            state = NodeStatus.FAILURE;
        }

        state = NodeStatus.RUNNING;
        return state;
    }
}
