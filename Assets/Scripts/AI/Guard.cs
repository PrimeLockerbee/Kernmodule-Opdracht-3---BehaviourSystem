using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BehaviourTree;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class Guard : BehaviourTree.Tree
{
    public Transform[] waypoints;
    public Animator animator;

    [SerializeField] private Transform _transform;

    [SerializeField] private TextMeshProUGUI _stateText;

    public static float speed = 2f;
    public static float fovRange = 5f;
    public static float pickupWeaponRange = 20f;
    public static float attackRange = 2f;

    public bool hasWeapon = false;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private GameObject _invisWall;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            // Sequence when the guard detects a player
            new Sequence(new List<Node>
            {
                new SetStateTextNode("Check player in range", _stateText),
                new CheckEnemyInFOVRange(transform, animator, _invisWall),  // Check if the player is in range
                new SetStateTextNode("Going to weapon", _stateText),
                new GoToWeaponTask(transform),  // Go pick up the weapon if necessary
                new SetStateTextNode("Picking up weapon", _stateText),
                new PickUpWeaponTask(transform),  // Pick up the weapon
                new SetStateTextNode("Going to player", _stateText),
                new GoToTargetTask(transform, _transform),  // Move towards the player
                new SetStateTextNode("Check if player is in attack range", _stateText),
                new CheckEnemyInAttackRange(transform, animator, _transform),  // Check if the player is in attack range
                new SetStateTextNode("Attacking", _stateText),
                new SetPlayerUnderAttackTask(_transform.gameObject.GetComponent<Player>(), true),
                new AttackTask(transform, animator, _transform),  // Attack if the player is in range
            }),

            new Sequence(new List<Node>
            {
                // Default to patrolling when no target is detected
                new SetStateTextNode("Patrolling", _stateText),
                new SetPlayerUnderAttackTask(_transform.gameObject.GetComponent<Player>(), false),
                new PatrolTask(transform, waypoints, animator),
            }),
        });

        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up, _transform.position);
    }
}
