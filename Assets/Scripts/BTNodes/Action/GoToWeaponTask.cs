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
        // Clear existing target data (we're looking for a weapon)
        parent.parent.SetData("target", null);

        object t = GetData("target");
        if (t == null)
        {
            // Look for colliders within the pickup weapon range (trigger colliders)
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Guard.pickupWeaponRange, _weaponLayerMask);
            DebugDrawOverlapSphere(_transform.position, Guard.pickupWeaponRange);
            Debug.Log("Number of colliders: " + colliders.Length);

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Weapon"))
                    {
                        // Set the weapon as the target
                        parent.parent.SetData("target", collider.transform);

                        // Move towards the weapon
                        float distance = Vector3.Distance(_transform.position, collider.transform.position);

                        if (distance > 0.1f)  // Using a small threshold to stop once we are near enough
                        {
                            // Move towards the target (use Vector3.MoveTowards for movement)
                            _transform.position = Vector3.MoveTowards(_transform.position, collider.transform.position, Guard.speed * Time.deltaTime);

                            //// Calculate direction to the weapon (we are only interested in rotation around the Y-axis)
                            //Vector3 direction = new Vector3(collider.transform.position.x, _transform.position.y, collider.transform.position.z);

                            //// Rotate only on the Y-axis (prevent rotating on X and Z)
                            //Quaternion targetRotation = Quaternion.LookRotation(direction - _transform.position);
                            //_transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 5f);  // Smooth rotation

                            state = NodeStatus.RUNNING;  // Task is still running since the guard is moving
                        }
                        else
                        {
                            // The guard has reached the weapon, pick it up and deactivate the weapon
                            Debug.Log("Weapon picked up!");

                            // Deactivate the weapon in the scene to simulate pickup
                            collider.gameObject.SetActive(false);

                            // Transition to success as the task is complete
                            state = NodeStatus.SUCCES;
                        }

                        // Debug the target information
                        Debug.Log("Target set to: " + collider.transform.name);
                        return state;
                    }
                }
            }

            // No weapon found in range, so fail the task
            state = NodeStatus.FAILURE;
            return state;
        }

        // If the guard already has a target, keep the task running (it should not repeat looking for the weapon)
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
