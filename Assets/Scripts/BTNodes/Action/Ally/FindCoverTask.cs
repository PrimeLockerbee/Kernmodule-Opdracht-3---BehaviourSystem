using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class FindCoverTask : Node
{
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Transform[] _coverPoints;
    private Transform _currentCoverPoint;

    public FindCoverTask(NavMeshAgent navMeshAgent, Transform[] coverPoints)
    {
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is null in FindCoverTask!");
            throw new System.ArgumentNullException(nameof(navMeshAgent));
        }
        if (coverPoints == null || coverPoints.Length == 0)
        {
            Debug.LogError("CoverPoints are null or empty in FindCoverTask!");
            throw new System.ArgumentNullException(nameof(coverPoints));
        }

        _navMeshAgent = navMeshAgent;
        _coverPoints = coverPoints;
        _transform = navMeshAgent.transform;
    }

    public override NodeStatus Evaluate()
    {
        // If we don't already have a target cover point, find the nearest one
        if (_currentCoverPoint == null)
        {
            Transform closestCover = null;
            float closestDistance = float.MaxValue;

            foreach (var cover in _coverPoints)
            {
                if (cover == null) continue; // Skip null cover points to avoid errors

                float distance = Vector3.Distance(_transform.position, cover.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCover = cover;
                }
            }

            if (closestCover != null)
            {
                _currentCoverPoint = closestCover;
                _navMeshAgent.SetDestination(_currentCoverPoint.position); // Set the NavMesh destination
            }
            else
            {
                // No cover points available
                Debug.LogWarning("No valid cover points found.");
                state = NodeStatus.FAILURE;
                return state;
            }
        }

        // Check if the ally has reached the cover point
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                Debug.Log("Reached cover!");
                state = NodeStatus.SUCCES;
                return state;
            }
        }

        // Still moving towards the cover point
        state = NodeStatus.RUNNING;
        return state;
    }
}
