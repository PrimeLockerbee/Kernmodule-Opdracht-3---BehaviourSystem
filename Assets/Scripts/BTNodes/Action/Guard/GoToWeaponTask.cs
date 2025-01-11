using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToWeaponTask : Node
{
    private static int _weaponLayerMask = 1 << 7;
    private Transform _transform;

    private Transform _weapon;

    public GoToWeaponTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        // Check if the guard already has a weapon
        Guard guard = _transform.GetComponent<Guard>();
        if (guard != null && guard.hasWeapon)
        {
            // If the guard already has the weapon, succeed immediately
            state = NodeStatus.SUCCES;
            return state;
        }

        // Clear existing target data (we're looking for a weapon)
        parent.parent.SetData("target", null);

        object t = GetData("target");
        if (t == null)
        {
            // Look for colliders within the pickup weapon range (trigger colliders)
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Guard.pickupWeaponRange, _weaponLayerMask);
            //DebugDrawOverlapSphere(_transform.position, Guard.pickupWeaponRange);
            //Debug.Log("Number of colliders: " + colliders.Length);

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Weapon"))
                    {
                        _weapon = collider.transform;

                        // Set the weapon as the target
                        parent.parent.SetData("target", collider.transform);
                        //Debug.Log("Weapon found: " + collider.transform.name); // Debug: Target weapon name

                        // Calculate the direction to the weapon
                        Vector3 direction = collider.transform.position - _transform.position;
                        direction.y = 0;  // We only care about rotation on the Y-axis, so set the Y component to 0

                        // Rotate the guard to face the weapon smoothly
                        if (direction.magnitude > 0.1f)  // Only rotate if the direction vector is significant
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 5f);  // Smooth rotation

                            // Move the guard towards the weapon
                            _transform.position = Vector3.MoveTowards(_transform.position, collider.transform.position, Guard.speed * Time.deltaTime);  // Move towards the weapon

                            //Debug.Log("Moving towards: " + collider.transform.name + " at position: " + collider.transform.position); // Debug: Moving towards weapon

                            float distance = Vector3.Distance(_transform.position, _weapon.position);
                            //Debug.Log("Guard distance to weapon: " + distance);

                            if (distance <= 1f)  // Guard is close enough to pick up the weapon
                            {
                                // Set the state to success since the task is complete
                                state = NodeStatus.SUCCES;
                                return state;
                            }
                        }

                        state = NodeStatus.RUNNING;
                        return state;
                    }
                }
            }

            // No weapon found in range, so fail the task
            //Debug.Log("No weapon found in range."); // Debug: No weapon found
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
