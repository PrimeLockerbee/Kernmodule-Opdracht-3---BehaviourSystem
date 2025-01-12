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
    public Transform[] wayPoints;
    public Animator animator;

    [SerializeField] private Transform _playerTransform;

    [SerializeField] private TextMeshProUGUI _stateText;

    public static float _speed = 2f;
    public static float _fovRange = 5f;
    public static float _pickupWeaponRange = 20f;
    public static float _attackRange = 1.5f;

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
                new CheckEnemyInFOVRange(transform, animator, _invisWall),  //Check if the player is in range
                new SetStateTextNode("Going to weapon", _stateText),
                new GoToWeaponTask(transform),  //Go pick up the weapon if necessary
                new SetStateTextNode("Picking up weapon", _stateText),
                new PickUpWeaponTask(transform),  //Pick up the weapon
                new SetStateTextNode("Going to player", _stateText),
                new GoToTargetTask(transform, _playerTransform),  //Move towards the player
                new SetStateTextNode("Check if player is in attack range", _stateText),
                new CheckEnemyInAttackRange(transform, animator, _playerTransform),  //Check if the player is in attack range
                new SetStateTextNode("Attacking", _stateText),
                new SetPlayerUnderAttackTask(_playerTransform.gameObject.GetComponent<Player>(), true),
                new AttackTask(transform, animator, _playerTransform),  //Attack if the player is in range
            }),

            new Sequence(new List<Node>
            {
                //Default to patrolling
                new SetStateTextNode("Patrolling", _stateText),
                new SetPlayerUnderAttackTask(_playerTransform.gameObject.GetComponent<Player>(), false),
                new PatrolTask(transform, wayPoints, animator),
            }),
        });

        return root;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.position + Vector3.up, _transform.position);
    //}
}
