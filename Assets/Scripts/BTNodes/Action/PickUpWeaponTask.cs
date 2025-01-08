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
            object t = GetData("target");
            if (t != null)
            {
                Transform weapon = (Transform)t;

                // Check if the guard is close enough to the weapon
                float distance = Vector3.Distance(_transform.position, weapon.position);
                if (distance <= 0.1f)
                {
                    // Guard reaches the weapon, so pick it up and deactivate it
                    Debug.Log("Weapon picked up!");

                    // Deactivate the weapon to simulate it being picked up
                    weapon.gameObject.SetActive(false);

                    // Mark the weapon as picked up
                    hasPickedUpWeapon = true;

                    state = NodeStatus.SUCCES; // Task completed
                    return state;
                }
            }
        }

        state = NodeStatus.SUCCES;
        return state;
    }
}
