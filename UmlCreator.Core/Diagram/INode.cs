using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    /// <summary>
    /// ノードが持っている機能です。
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// 名前
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 型
        /// </summary>
        string Type { get; }

        /// <summary>
        /// 詳細名
        /// </summary>
        string FullName => string.Join(" ", AccessibilityString, Name, ":", Type);

        /// <summary>
        /// アクセス修飾子
        /// </summary>
        AccessLevel Accessibility { get; }

        /// <summary>
        /// アクセス修飾子の文字列表記
        /// </summary>
        string AccessibilityString => (this.Accessibility) switch
        {
            AccessLevel.Public => "+",
            AccessLevel.Package => "~",
            AccessLevel.Protected => "#",
            AccessLevel.Private => "-",
            _ => throw new InvalidOperationException(),
        };
    }
}
