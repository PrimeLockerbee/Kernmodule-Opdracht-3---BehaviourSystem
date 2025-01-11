using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BehaviourTree;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Rogue : BehaviourTree.Tree
{
    public Animator animator;
    public Transform playerTransform;  // Reference to the player's transform
    [SerializeField] private Transform _transform;
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private NavMeshAgent _agent;

    public Transform[] coverPoints;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            // Sequence for reacting when the player is attacked by an enemy
            new Sequence(new List<Node>
            {
                new SetStateTextNode("Check if player under attack", _stateText),
                new CheckIfPlayerUnderAttackTask(playerTransform),  // Check if the player is being attacked
                new SetStateTextNode("Finding cover", _stateText),
                new FindCoverTask(_agent, coverPoints),  // Find cover to hide behind
                new SetStateTextNode("Throwing smoke", _stateText),
                new ThrowSmokeBombTask(_transform),  // Throw a smoke bomb to confuse the enemy
            }),

            // Sequence for following the player if nothing else is happening
            new Sequence(new List<Node>
            {
                new SetStateTextNode("Following Player", _stateText),
                new DebugTask("Works correct"),
                new FollowPlayerTask(_agent, playerTransform, animator),  // Pass the follow range here
            }),

        });

        return root;
    }
}
