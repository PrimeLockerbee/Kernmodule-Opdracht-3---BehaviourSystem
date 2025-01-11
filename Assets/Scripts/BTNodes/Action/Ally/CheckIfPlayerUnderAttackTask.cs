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
        // Logic to check if the player is under attack
        // For now, let's assume there's a simple flag or method on the player that indicates if they're under attack

        Player player = _playerTransform.GetComponent<Player>();
        if (player != null && player.IsUnderAttack())  // You'd need to implement `IsUnderAttack` on the Player
        {
            state = NodeStatus.SUCCES;  // Fixed typo here
            return state;
        }

        state = NodeStatus.FAILURE;
        return state;
    }
}
