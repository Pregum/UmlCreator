using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    internal class ClassNode : IRootNode, IExpression
    {
        private List<INode> _dataNodes;
        public IReadOnlyList<INode> DataNodes => _dataNodes.AsReadOnly();

        public bool HasDataNodes => _dataNodes.Any();

        private List<IOperationalNode> _behaviorNodes;

        public IReadOnlyList<IOperationalNode> BehaviorNodes => _behaviorNodes.AsReadOnly();

        public bool HasBehaviorNodes => _behaviorNodes.Any();

        public string Name { get; }

        public string Type { get; }

        public AccessLevel Accessibility { get; }

        public string FullName => string.Join(" ", AccessibilityString, Name, ":", Type);

        public string AccessibilityString
        {
            get
            {
                switch (Accessibility)
                {
                    case AccessLevel.Public:
                        return "+";
                    case AccessLevel.Package:
                        return "~";
                    case AccessLevel.Protected:
                        return "#";
                    case AccessLevel.Private:
                        return "-";
                    case AccessLevel.PrivateProtected:
                    case AccessLevel.ProtectedInternal:
                    case AccessLevel.Internal:
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

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
            var behaviorNodes = nodes.Where(x => x is MethodNode)?.Cast<IOperationalNode>();

            _dataNodes = dataNodes != null ? new List<INode>(dataNodes) : new List<INode>();
            _behaviorNodes = behaviorNodes != null ? new List<IOperationalNode>(behaviorNodes) : new List<IOperationalNode>();
        }
    }
}
