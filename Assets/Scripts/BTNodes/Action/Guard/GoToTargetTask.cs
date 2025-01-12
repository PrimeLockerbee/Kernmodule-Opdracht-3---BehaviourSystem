using BehaviourTree;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GoToTargetTask : Node
{
    private Transform _guardTransform;
    private Transform _target;

    public GoToTargetTask(Transform transform, Transform target)
    {
        _guardTransform = transform;
        _target = target;
    }

    public override NodeStatus Evaluate()
    {
        //Get the Guard component
        Guard guard = _guardTransform.GetComponent<Guard>();
        if (guard == null)
        {
            Debug.LogWarning("Guard component not found on target.");
            state = NodeStatus.FAILURE;
            return state;
        }

        if (!guard.hasWeapon)
        {
            //Debug.Log("Guard hasn't picked up the weapon yet, moving to next task.");

            state = NodeStatus.FAILURE; 
            return state;
        }

        if (_target != null)
        {
            //Debugging: Log the target and the guard's position
            //Debug.Log("Guard has the weapon. Moving towards target: " + _target.name + " | Distance: " + Vector3.Distance(_transform.position, _target.position));

            float distance = Vector3.Distance(_guardTransform.position, _target.position);

            if (distance > 5f)
            {
                //Debug.Log("Player is too far, stopping chase.");
                ClearData("target");
                state = NodeStatus.FAILURE;
                return state;
            }

            if (distance > Guard._attackRange)
            {
                _guardTransform.position = Vector3.MoveTowards(_guardTransform.position, _target.position, Guard._speed * Time.deltaTime);

                Vector3 direction = _target.position - _guardTransform.position;
                direction.y = 0;
                if (direction.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    _guardTransform.rotation = Quaternion.Slerp(_guardTransform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                state = NodeStatus.RUNNING;
                return state;
            }
            else
            {
                //If the guard has reached the target
                //Debug.Log("Guard reached the target: " + _target.name);

                state = NodeStatus.SUCCES;
                return state;
            }
        }
        else
        {
            Debug.LogWarning("No target found for the guard to move towards.");
            state = NodeStatus.FAILURE;
            return state;
        }
    }
}
