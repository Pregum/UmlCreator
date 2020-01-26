using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    internal class ClassNode : IRootNode
    {
        private List<INode> _dataNodes;
        public IReadOnlyList<INode> DataNodes => _dataNodes.AsReadOnly();

        public bool HasDataNodes => _dataNodes.Any();

        private List<INode> _behaviorNodes;

        public IReadOnlyList<INode> BehaviorNodes => _behaviorNodes.AsReadOnly();

        public bool HasBehaviorNodes => _behaviorNodes.Any();

        public string Name { get; }

        public string Type { get; }

        public AccessLevel Accessibility { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="accessibility"></param>
        /// <param name="nodes"></param>
        public ClassNode(string name, string type, AccessLevel accessibility, IEnumerable<INode> nodes)
        {

            Name = name;
            Type = type;
            Accessibility = accessibility;
            var dataNodes = nodes.Where(x => x is FieldNode || x is PropertyNode);
            var behaviorNodes = nodes.Where(x => x is MethodNode);

            _dataNodes = dataNodes != null ? new List<INode>(dataNodes) : new List<INode>();
            _behaviorNodes = behaviorNodes != null ? new List<INode>(behaviorNodes) : new List<INode>();
        }
    }
}
