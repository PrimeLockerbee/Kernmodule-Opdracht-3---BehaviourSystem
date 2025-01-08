using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeStatus Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeStatus.FAILURE:
                        continue;
                    case NodeStatus.SUCCES:
                        state = NodeStatus.SUCCES;
                        return state;
                    case NodeStatus.RUNNING:
                        state = NodeStatus.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeStatus.FAILURE;
            return state;
        }
    }
}
