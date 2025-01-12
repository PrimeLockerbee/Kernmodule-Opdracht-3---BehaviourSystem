using BehaviourTree;
using System;
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
        _navMeshAgent = navMeshAgent;
        _coverPoints = coverPoints;
        _transform = navMeshAgent.transform;
    }

   public override NodeStatus Evaluate()
    {
        // Check if we have already reached the current cover point
        if (_currentCoverPoint != null && !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.2f)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude < 0.01f) // Ensure minimal movement threshold
            {
                Debug.Log("Already at cover!");
                _currentCoverPoint = null; // Reset cover point to allow for reevaluation
                state = NodeStatus.SUCCES;  // Once cover is reached, move to success
                return state;
            }
        }

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

                // Reset path to clear any previous destinations (like player)
                _navMeshAgent.ResetPath();
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

        // Still moving towards the cover point
        state = NodeStatus.RUNNING;
        return state;
    }

}
