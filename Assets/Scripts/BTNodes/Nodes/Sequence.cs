using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        private int _childIndex;

        public override NodeStatus Evaluate()
        {
            bool anyChildIsRunning = false;

            for (int i = _childIndex; i < _children.Count; i++)
            {
                Node child = _children[i];
                switch (child.Evaluate())
                {
                    case NodeStatus.FAILURE:
                        _childIndex = 0;
                        state = NodeStatus.FAILURE;
                        return state;
                    case NodeStatus.SUCCES:
                        continue;
                    case NodeStatus.RUNNING:
                        anyChildIsRunning = true;
                        return NodeStatus.RUNNING;
                    default:
                        state = NodeStatus.SUCCES;
                        return state;
                }
            }

            _childIndex = 0;
            state = anyChildIsRunning ? NodeStatus.RUNNING : NodeStatus.SUCCES;
            return state;
        }
    }
}
