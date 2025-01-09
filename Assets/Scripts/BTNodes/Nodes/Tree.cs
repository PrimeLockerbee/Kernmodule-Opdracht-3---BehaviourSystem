using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        public Blackboard blackBoard { get; protected set; }

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}