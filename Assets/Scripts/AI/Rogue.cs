using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BehaviourTree;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Rogue : BehaviourTree.Tree
{
    public Transform[] _coverPoints;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _ninjaTransform;

    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private GameObject _invisWall;
    [SerializeField] private GameObject _smokeEffect;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            //Sequence for reacting to when the player getss attacked
            new Sequence(new List<Node>
            {
                new SetStateTextNode("Check if player under attack", _stateText),
                new CheckIfPlayerUnderAttackTask(_playerTransform),  //Check if the player is being attacked
                new SetStateTextNode("Finding cover", _stateText),
                new FindCoverTask(_agent, _coverPoints),  //Find nearest cover to hide behind
                new SetStateTextNode("Throwing smoke", _stateText),
                new ThrowSmokeBombTask(_ninjaTransform, _invisWall, this, _smokeEffect),  //Throw a "smoke bomb" to confuse the enemy
            }),

            //Sequence for following the player
            new Sequence(new List<Node>
            {
                new SetStateTextNode("Following Player", _stateText),
                new FollowPlayerTask(_agent, _playerTransform, _animator),
            }),

        });

        return root;
    }
}
