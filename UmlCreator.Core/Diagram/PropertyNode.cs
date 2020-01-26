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

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="accessibility"></param>
        public PropertyNode(string name, string type, AccessLevel accessibility)
        {
            Name = name;
            Type = type;
            Accessibility = accessibility;
        }
    }
}
