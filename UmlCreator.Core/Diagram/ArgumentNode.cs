using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    internal class ArgumentNode : INode
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 型
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        public AccessLevel Accessibility => AccessLevel.Private;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">型</param>
        public ArgumentNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
