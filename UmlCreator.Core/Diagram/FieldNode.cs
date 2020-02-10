using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    internal class FieldNode : INode
    {
        public string Name { get; }

        public string Type { get; }

        public AccessLevel Accessibility { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">型</param>
        /// <param name="accessibility">アクセス修飾子</param>
        public FieldNode(string name, string type, AccessLevel accessibility)
        {
            Name = name;
            Type = type;
            Accessibility = accessibility;
        }
    }
}
