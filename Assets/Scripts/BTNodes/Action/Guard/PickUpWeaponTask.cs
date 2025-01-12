using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PickUpWeaponTask : Node
{
    private Transform _guardTransform;
    private bool _hasPickedUpWeapon = false;

    public PickUpWeaponTask(Transform transform)
    {
        _guardTransform = transform;
    }

    public override NodeStatus Evaluate()
    {
        if (!_hasPickedUpWeapon)
        {
            object t = GetData("target");

            if (t != null)
            {
                Transform weapon = (Transform)t;

                //Log the target weapon's name for debugging
                //Debug.Log("Guard is heading to pick up the weapon: " + weapon.name);

                //Check if the guard is close enough to the weapon
                float distance = Vector3.Distance(_guardTransform.position, weapon.position);

                if (distance <= 1f)  // Guard is close enough to pick up the weapon
                {
                    //Log when the weapon is picked up
                    //Debug.Log("Guard has picked up the weapon: " + weapon.name);

                    weapon.gameObject.SetActive(false);

                    _hasPickedUpWeapon = true;

                    Guard guard = _guardTransform.GetComponent<Guard>();
                    if (guard != null)
                    {
                        guard.hasWeapon = true;  //Set hasWeapon flag in the Guard class
                    }

                    state = NodeStatus.SUCCES;
                    return state;
                }
            }
            else
            {
                //Log if no weapon target is available
                Debug.LogWarning("No weapon target available for pickup.");
            }
        }

        //if (_hasPickedUpWeapon)
        //{
        //    Debug.Log("Weapon already picked up. Task completed.");
        //}

        state = NodeStatus.SUCCES;
        return state;
    }
}
