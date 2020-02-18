using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    internal class MethodNode : IOperationalNode
    {
        public string Name { get; }

        public string Type { get; }

        public AccessLevel Accessibility { get; }

        private List<INode> _arguments;

        public IReadOnlyList<INode> Arguments => _arguments.AsReadOnly();

        public string NameWithArgs => $"{Name}({string.Join(", ", Arguments.Select(x => $"{x.Name} : {x.Type}"))})";

        public bool HasArguments => Arguments?.Any() ?? false;

        public string FullName => $"{string.Join(" ", AccessibilityString, NameWithArgs, ":", Type)}";

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
        /// <param name="args">引数</param>
        public MethodNode(string name, string type, AccessLevel accessibility, IEnumerable<INode> args)
        {
            Name = name;
            Type = type;
            Accessibility = accessibility;
            _arguments = args?.ToList() ?? new List<INode>();
        }
    }
}
