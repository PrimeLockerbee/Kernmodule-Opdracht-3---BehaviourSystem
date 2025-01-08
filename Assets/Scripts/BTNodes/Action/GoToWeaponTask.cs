using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToWeaponTask : Node
{
    private static int _weaponLayerMask = 1 << 7;

    private Transform _transform;

    public GoToWeaponTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        // Clear existing target data
        parent.parent.SetData("target", null);

        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Guard.pickupWeaponRange, _weaponLayerMask);
            DebugDrawOverlapSphere(_transform.position, Guard.pickupWeaponRange);
            Debug.Log("Number of colliders: " + colliders.Length);

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Weapon"))
                    {
                        parent.parent.SetData("target", collider.transform);

                        // Move towards the target
                        float distance = Vector3.Distance(_transform.position, collider.transform.position);

                        if (distance > 0.01f)
                        {
                            // Move towards the target
                            _transform.position = Vector3.MoveTowards(_transform.position, collider.transform.position, Guard.speed * Time.deltaTime);

                            // Ensure the guard doesn't rotate along the x-axis
                            Vector3 targetPosition = new Vector3(collider.transform.position.x, _transform.position.y, collider.transform.position.z);
                            _transform.LookAt(targetPosition);

                            state = NodeStatus.RUNNING;
                        }
                        else
                        {
                            // The guard has reached the weapon
                            state = NodeStatus.SUCCES;
                        }


                        // Debug the target information
                        Debug.Log("Target set to: " + collider.transform.name);

                        return state;
                    }
                }
            }

            state = NodeStatus.FAILURE;
            return state;
        }

        // Keep the guard in a running state if it already has a target
        state = NodeStatus.RUNNING;
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
