using BehaviourTree;
using UnityEngine;

public class SetPlayerUnderAttackTask : Node
{
    private Player _player;
    private bool _underAttackState;

    // Constructor now takes a bool parameter to set the under attack state
    public SetPlayerUnderAttackTask(Player player, bool underAttackState)
    {
        _player = player;
        _underAttackState = underAttackState;
    }

    public override NodeStatus Evaluate()
    {
        if (_player != null)
        {
            _player.SetUnderAttack(_underAttackState); // Set the under attack state based on the passed parameter
            return NodeStatus.SUCCES;
        }

        return NodeStatus.FAILURE;
    }
}
