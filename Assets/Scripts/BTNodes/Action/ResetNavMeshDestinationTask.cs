using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class ResetNavMeshDestinationTask : Node
{
    private NavMeshAgent _navMeshAgent;

    public ResetNavMeshDestinationTask(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }

    public override NodeStatus Evaluate()
    {
        // Reset the NavMeshAgent path to ensure no stale destination is set
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.ResetPath();
        }

        // Optionally, log the reset for debugging purposes
        Debug.Log("NavMesh destination reset");

        state = NodeStatus.SUCCES;
        return state;
    }
}