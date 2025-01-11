using BehaviourTree;
using UnityEngine;

public class FindCoverTask : Node
{
    private Transform _transform;
    private Transform[] _coverPoints;

    public FindCoverTask(Transform transform, Transform[] coverPoints)
    {
        _transform = transform;
        _coverPoints = coverPoints;
    }

    public override NodeStatus Evaluate()
    {
        // Logic to find the nearest cover point and move to it
        Transform closestCover = null;
        float closestDistance = float.MaxValue;

        foreach (var cover in _coverPoints)
        {
            float distance = Vector3.Distance(_transform.position, cover.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCover = cover;
            }
        }

        // Move to the closest cover
        if (closestCover != null)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, closestCover.position, 4f * Time.deltaTime);
            state = NodeStatus.RUNNING;
            return state;
        }

        state = NodeStatus.FAILURE;
        return state;
    }
}
