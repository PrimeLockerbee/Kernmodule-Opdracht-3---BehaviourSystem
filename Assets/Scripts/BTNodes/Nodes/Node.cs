using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    public enum NodeStatus
    {
        RUNNING,
        SUCCES,
        FAILURE
    }

    public abstract class Node
    {
        protected NodeStatus state;

        public Node _parent;
        protected List<Node> _children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            _parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node._parent = this;
            _children.Add(node);
        }

        public virtual NodeStatus Evaluate() => NodeStatus.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = _parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node._parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = _parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node._parent;
            }
            return false;
        }
    }
}

