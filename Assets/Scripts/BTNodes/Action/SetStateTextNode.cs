using BehaviourTree;
using TMPro;
using UnityEngine;

public class SetStateTextNode : Node
{
    private string _message;
    private TextMeshProUGUI _textMeshProUGUI;

    // Constructor that takes a message to log
    public SetStateTextNode(string message, TextMeshProUGUI statetext)
    {
        _message = message;
        _textMeshProUGUI = statetext;
    }

    public override NodeStatus Evaluate()
    {
        //Debug.Log(_message);  // Log the message to the console
        _textMeshProUGUI.text = _message;
        state = NodeStatus.SUCCES;  // Immediately succeed after logging the message
        return state;
    }
}
