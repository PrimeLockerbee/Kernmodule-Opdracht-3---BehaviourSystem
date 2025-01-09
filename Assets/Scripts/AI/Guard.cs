using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BehaviourTree;

public class Guard : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.Animator animator;

    public static float speed = 2f;
    public static float fovRange = 5f;
    public static float pickupWeaponRange = 20f;
    public static float attackRange = 1f;

    public bool hasWeapon = false;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            // Sequence when the guard detects a player
            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(transform, animator),  // Check if the player is in range
                new GoToWeaponTask(transform),  // Go pick up the weapon if necessary
                new PickUpWeaponTask(transform),  // Pick up the weapon
                //new GoToTargetTask(transform),  // Move towards the player
                //new CheckEnemyInAttackRange(transform, animator),  // Check if the player is in attack range
                //new AttackTask(transform, animator),  // Attack if the player is in range
            }),

            // Sequence for the guard to return to patrol when the player is lost
            //new Sequence(new List<Node>
            //{
            //    new CheckEnemyInFOVRange(transform, animator), // If player is lost or out of range
            //    new ReturnToPatrolTask(transform, waypoints[0].position)  // Return to the first waypoint
            //}),

            // Default to patrolling when no target is detected
            new PatrolTask(transform, waypoints, animator),
        });

        return root;
    }
}
