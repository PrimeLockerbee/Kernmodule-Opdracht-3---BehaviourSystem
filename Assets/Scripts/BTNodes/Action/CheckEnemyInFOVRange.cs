using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
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
        object t = GetData("target");
        if (t == null) 
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Guard.fovRange, _playerLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                _animator.SetBool("Walking", true);
                state = NodeStatus.SUCCES;
                return state;
            }

            state = NodeStatus.FAILURE;
            return state;
        }

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
