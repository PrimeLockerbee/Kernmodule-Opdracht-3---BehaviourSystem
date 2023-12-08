using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BehaviourTree;

public class Guard : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 2f;

    protected override Node SetupTree()
    {
        Node root = new PatrolTask(transform, waypoints);

        return root;
    }

    
}
