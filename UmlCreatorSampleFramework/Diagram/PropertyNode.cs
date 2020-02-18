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
        /// アクセスレベル + シグネチャ + 戻り値の型をまとめた名前
        /// </summary>
        public string FullName => string.Join(" ", AccessibilityString, Name, ":", Type);

        /// <summary>
        /// アクセスレベルの文字列表記
        /// </summary>
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
        public PropertyNode(string name, string type, AccessLevel accessibility)
        {
            Name = name;
            Type = type;
            Accessibility = accessibility;
        }
    }
}
