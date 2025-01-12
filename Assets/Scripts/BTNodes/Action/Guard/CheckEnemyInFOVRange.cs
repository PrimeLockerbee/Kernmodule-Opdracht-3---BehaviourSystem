using BehaviourTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    private static int _playerLayerMask = 1 << 6;  // Assuming player is on Layer 6
    private Transform _transform;
    private Animator _animator;

    // Guard's field of view properties
    private float _fovAngle = 110f;  // Field of view angle (in degrees)
    private float _fovRange = 20f;   // Field of view range (in units)

    [SerializeField] private GameObject _invisWall;

    public CheckEnemyInFOVRange(Transform transform, Animator animator, GameObject invisWall)
    {
        _transform = transform;
        _animator = animator;
        _invisWall = invisWall;
    }

    public override NodeStatus Evaluate()
    {
        if (_invisWall.activeInHierarchy)
        {
            state = NodeStatus.FAILURE;
            return state;
        }

        object target = GetData("target");

        // Only check for a target if we don't have one already
        if (target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _fovRange, _playerLayerMask);

            // Loop through all colliders in range
            foreach (var collider in colliders)
            {
                Transform playerTransform = collider.transform;

                // Check if the player is within the guard's field of view
                Vector3 directionToPlayer = (playerTransform.position - _transform.position).normalized;
                float angleToPlayer = Vector3.Angle(_transform.forward, directionToPlayer);

                //Debug.DrawRay(_transform.position + Vector3.up, directionToPlayer, Color.red);

                // If the player is within the FOV angle, proceed with line of sight check
                if (angleToPlayer < _fovAngle / 2f)
                {
                    Physics.queriesHitTriggers = true;

                    // Now check if the line of sight is clear (no obstructions)
                    RaycastHit hit;
                    if (Physics.Raycast(_transform.position + Vector3.up, directionToPlayer, out hit, _fovRange))
                    {
                        if (hit.transform == playerTransform)
                        {
                            // Player detected within FOV and clear line of sight
                            parent.parent.SetData("target", playerTransform);
                            _animator.SetBool("Walking", true);
                            state = NodeStatus.SUCCES;
                            return state;
                        }
                    }
                }
            }

            // If no player found within FOV or no clear line of sight, fail the task
            state = NodeStatus.FAILURE;
            return state;
        }

        // Target is already set, continue the task as success
        state = NodeStatus.SUCCES;
        return state;
    }

    private void DebugDrawFOV(Vector3 center, float range, float angle)
    {
        float halfAngle = angle / 2f;
        Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * _transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * _transform.forward;

        Debug.DrawRay(center, leftBoundary * range, Color.green);  // Left edge of the FOV
        Debug.DrawRay(center, rightBoundary * range, Color.green); // Right edge of the FOV

        // Optional: Draw a line indicating the center of the FOV
        Debug.DrawRay(center, _transform.forward * range, Color.red);
    }
}
