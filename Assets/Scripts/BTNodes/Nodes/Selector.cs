using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        private int _childIndex;

        public override NodeStatus Evaluate()
        {
            for (int i = _childIndex; i < _children.Count; i++)
            {
                Node child = _children[i];
                switch (child.Evaluate())
                {         
                        case NodeStatus.FAILURE:
                            continue;
                        case NodeStatus.SUCCES:
                        _childIndex = 0;
                            state = NodeStatus.SUCCES;
                            return state;
                        case NodeStatus.RUNNING:
                            state = NodeStatus.RUNNING;
                            return state;
                        default:
                            continue;                
                }     
            }

            _childIndex = 0;
            state = NodeStatus.FAILURE;
            return state;
        }
    }
}
