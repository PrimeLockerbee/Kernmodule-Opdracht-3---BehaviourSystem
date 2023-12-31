using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeStatus Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeStatus.FAILURE:
                        state = NodeStatus.FAILURE;
                        return state;
                    case NodeStatus.SUCCES:
                        continue;
                    case NodeStatus.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeStatus.SUCCES;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeStatus.RUNNING : NodeStatus.SUCCES;
            return state;
        }
    }
}
