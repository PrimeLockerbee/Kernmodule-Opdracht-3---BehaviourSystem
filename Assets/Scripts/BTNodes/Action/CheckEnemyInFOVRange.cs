using BehaviourTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    private static int _playerLayerMask = 1 << 6;
    private Transform _transform;
    private Animator _animator;

    public CheckEnemyInFOVRange(Transform transform, Animator animator)
    {
        _transform = transform;
        _animator = animator;
    }

    public override NodeStatus Evaluate()
    {
        object target = GetData("target");

        // Only check for a target if we don't have one already
        if (target == null)
        {
            // Check for player within FOV range
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Guard.fovRange, _playerLayerMask);

            if (colliders.Length > 0)
            {
                // Player found, set the target to the first detected collider (the player)
                parent.parent.SetData("target", colliders[0].transform);
                _animator.SetBool("Walking", true);
                //Debug.Log("Player detected: " + colliders[0].transform.name);
                state = NodeStatus.SUCCES;
                return state;
            }

            // If no player found, fail the task
            //Debug.LogWarning("No player in FOV range.");
            state = NodeStatus.FAILURE;
            return state;
        }

        // Target is already set, continue the task as success
        state = NodeStatus.SUCCES;
        return state;
    }

    private void DebugDrawOverlapSphere(Vector3 center, float radius)
    {
        int numPoints = 30;
        float angleIncrement = 360f / numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * angleIncrement;
            float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector3 startPoint = new Vector3(x, center.y, z);
            Vector3 endPoint = center;

            Debug.DrawLine(startPoint, endPoint, Color.red, 0.1f);
        }
    }
}
