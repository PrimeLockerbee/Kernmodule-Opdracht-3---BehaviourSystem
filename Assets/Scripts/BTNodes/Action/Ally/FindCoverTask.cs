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

                // Set the NavMesh destination and log for debugging
                _navMeshAgent.SetDestination(_currentCoverPoint.position);

                // Debug the destination name
                Debug.Log($"New cover point set: {_currentCoverPoint.name}, Position: {_currentCoverPoint.position}");
            }
            else
            {
                // No cover points available
                Debug.LogWarning("No valid cover points found.");
                state = NodeStatus.FAILURE;
                return state;
            }
        }

        // Debug to check if the destination is still the player
        if (_navMeshAgent.destination == _transform.position)
        {
            Debug.LogWarning("NavMeshAgent destination is set to the player!");
        }

        // Check if the ally has reached the cover point
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.2f) // Adjust buffer here
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude < 0.01f) // Ensure minimal movement threshold
            {
                Debug.Log("Reached cover!");
                _currentCoverPoint = null; // Reset cover point to allow for reevaluation
                state = NodeStatus.SUCCES;
                return state;
            }
        }

        // Debug the current destination and remaining distance
        //Debug.Log($"Moving towards cover: {_navMeshAgent.destination}, Remaining Distance: {_navMeshAgent.remainingDistance}, Cover Object: {_currentCoverPoint?.name}");

        // Still moving towards the cover point
        state = NodeStatus.RUNNING;
        return state;
    }
}
