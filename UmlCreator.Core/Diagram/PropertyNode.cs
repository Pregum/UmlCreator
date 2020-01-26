using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    internal class PropertyNode : INode
    {
        public string Name { get; }

        public string Type { get; }

        public AccessLevel Accessibility { get; }

        public PropertyNode(string name, string type, AccessLevel accessibility)
        {
            Name = name;
            Type = type;
            Accessibility = accessibility;
        }
    }
}
