using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToWeaponTask : Node
{
    private static int _weaponLayerMask = 1 << 7;
    private Transform _guardTransform;

    private Transform _weapon;

    public GoToWeaponTask(Transform transform)
    {
        _guardTransform = transform;
    }

    public override NodeStatus Evaluate()
    {
        Guard guard = _guardTransform.GetComponent<Guard>();
        if (guard != null && guard.hasWeapon)
        {
            state = NodeStatus.SUCCES;
            return state;
        }

        //Clear existing target data (we're looking for a weapon)
        _parent._parent.SetData("target", null);

        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_guardTransform.position, Guard._pickupWeaponRange, _weaponLayerMask);

            //Debug.Log("Number of colliders: " + colliders.Length);

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Weapon"))
                    {
                        _weapon = collider.transform;


                        _parent._parent.SetData("target", collider.transform);

                        //Debug.Log("Weapon found: " + collider.transform.name); //Debug: Target weapon name

                        //Calculate the direction to the weapon
                        Vector3 direction = collider.transform.position - _guardTransform.position;
                        direction.y = 0;

                        if (direction.magnitude > 0.1f)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            _guardTransform.rotation = Quaternion.Slerp(_guardTransform.rotation, targetRotation, Time.deltaTime * 5f);

                            // Move the guard towards the weapon
                            _guardTransform.position = Vector3.MoveTowards(_guardTransform.position, collider.transform.position, Guard._speed * Time.deltaTime);

                            //Debug.Log("Moving towards: " + collider.transform.name + " at position: " + collider.transform.position); //Debug: Moving towards weapon

                            float distance = Vector3.Distance(_guardTransform.position, _weapon.position);
                            //Debug.Log("Guard distance to weapon: " + distance);

                            if (distance <= 1f)
                            {
                                state = NodeStatus.SUCCES;
                                return state;
                            }
                        }

                        state = NodeStatus.RUNNING;
                        return state;
                    }
                }
            }

            //No weapon found in range, so fail the task
            //Debug.Log("No weapon found in range."); //Debug: No weapon found

            state = NodeStatus.FAILURE;
            return state;
        }

        state = NodeStatus.RUNNING;
        return state;
    }

    //private void DebugDrawOverlapSphere(Vector3 center, float radius)
    //{
    //    int numPoints = 30;
    //    float angleIncrement = 360f / numPoints;

    //    for (int i = 0; i < numPoints; i++)
    //    {
    //        float angle = i * angleIncrement;
    //        float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
    //        float z = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

    //        Vector3 startPoint = new Vector3(x, center.y, z);
    //        Vector3 endPoint = center;

    //        Debug.DrawLine(startPoint, endPoint, Color.red, 0.1f);
    //    }
    //}
}
