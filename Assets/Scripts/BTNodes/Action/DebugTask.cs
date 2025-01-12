using BehaviourTree;
using UnityEngine;

public class DebugTask : Node
{
    private string _message;

    public DebugTask(string message)
    {
        _message = message;
    }

    public override NodeStatus Evaluate()
    {
        //Debug.Log(_message);  //Log the message to the console
        state = NodeStatus.SUCCES;
        return state;
    }
}