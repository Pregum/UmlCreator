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
        /// <param name="name">名前</param>
        /// <param name="type">型</param>
        public ArgumentNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
