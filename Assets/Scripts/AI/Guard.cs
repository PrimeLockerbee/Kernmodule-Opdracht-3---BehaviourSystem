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
    public static float attackRange = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {   
            //new Sequence(new List<Node>
            //{
            //    new CheckEnemyInAttackRange(transform, animator),
            //    new AttackTask(transform),
            //}),
            //new Sequence(new List<Node>
            //{
            //    new CheckEnemyInFOVRange(transform, animator),
            //    //new GoToWeaponTask(transform),
            //    //new PickUpWeaponTask(),
            //    //new GoToTargetTask(transform),
            //}),
            new PatrolTask(transform, waypoints, animator),
        });

        return root;
    }
}
