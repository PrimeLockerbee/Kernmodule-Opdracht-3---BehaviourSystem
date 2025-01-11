using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PickUpWeaponTask : Node
{
    private Transform _transform;
    private bool hasPickedUpWeapon = false;

    public PickUpWeaponTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeStatus Evaluate()
    {
        // If the weapon hasn't been picked up yet
        if (!hasPickedUpWeapon)
        {
            object t = GetData("target");  // Get the weapon target from the data
            if (t != null)
            {
                Transform weapon = (Transform)t;

                // Log the target weapon's name for debugging
                //Debug.Log("Guard is heading to pick up the weapon: " + weapon.name);

                // Check if the guard is close enough to the weapon
                float distance = Vector3.Distance(_transform.position, weapon.position);
                //Debug.Log("Guard distance to weapon: " + distance);

                if (distance <= 1f)  // Guard is close enough to pick up the weapon
                {
                    // Log when the weapon is picked up
                    //Debug.Log("Guard has picked up the weapon: " + weapon.name);

                    // Deactivate the weapon to simulate it being picked up
                    weapon.gameObject.SetActive(false);

                    // Mark the weapon as picked up
                    hasPickedUpWeapon = true;

                    Guard guard = _transform.GetComponent<Guard>();
                    if (guard != null)
                    {
                        guard.hasWeapon = true;  // Set hasWeapon flag in the Guard class
                    }

                    // Log task success
                    //Debug.Log("Weapon pick up task completed successfully.");

                    // Set the state to success since the task is complete
                    state = NodeStatus.SUCCES;
                    return state;
                }
                else
                {
                    // Log if the guard is still moving towards the weapon
                    //Debug.Log("Guard is moving towards the weapon: " + weapon.name);
                }
            }
            else
            {
                // Log if no weapon target is available
                //Debug.LogWarning("No weapon target available for pickup.");
            }
        }

        // If the weapon has already been picked up, continue and mark as success
        if (hasPickedUpWeapon)
        {
            //Debug.Log("Weapon already picked up. Task completed.");
        }

        // Return success once the weapon is picked up or the task is done
        state = NodeStatus.SUCCES;
        return state;
    }
}
