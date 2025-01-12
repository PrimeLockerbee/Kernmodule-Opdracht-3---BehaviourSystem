using BehaviourTree;
using UnityEngine;

public class CheckIfPlayerUnderAttackTask : Node
{
    private Transform _playerTransform;

    public CheckIfPlayerUnderAttackTask(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public override NodeStatus Evaluate()
    {
        Player player = _playerTransform.GetComponent<Player>();
        if (player != null && player.IsUnderAttack()) 
        {
            state = NodeStatus.SUCCES;
            return state;
        }

        state = NodeStatus.FAILURE;
        return state;
    }
}
