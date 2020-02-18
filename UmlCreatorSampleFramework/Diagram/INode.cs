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
        /// アクセスレベル+メソッドシグネチャ+型をまとめた名前
        /// </summary>
        //string FullName => string.Join(" ", AccessibilityString, Name, ":", Type);
        string FullName { get; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        AccessLevel Accessibility { get; }

        /// <summary>
        /// アクセスレベルの文字列表記
        /// </summary>
        string AccessibilityString { get; }
        //string AccessibilityString => (this.Accessibility) switch
        //{
        //    AccessLevel.Public => "+",
        //    AccessLevel.Package => "~",
        //    AccessLevel.Protected => "#",
        //    AccessLevel.Private => "-",
        //    _ => throw new InvalidOperationException(),
        //};
    }
}
