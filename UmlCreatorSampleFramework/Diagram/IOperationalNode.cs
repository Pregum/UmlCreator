using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    /// <summary>
    /// 操作可能なノードを表します
    /// </summary>
    public interface IOperationalNode : INode
    {
        /// <summary>
        /// 引数リスト
        /// </summary>
        IReadOnlyList<INode> Arguments { get; }

        /// <summary>
        /// 引数付きの名前
        /// </summary>
        string NameWithArgs { get; }// => $"{Name}({string.Join(", ", Arguments.Select(x => $"{x.Name} : {x.Type}"))})";

        /// <summary>
        /// アクセスレベル+メソッドシグネチャ+型をまとめた名前
        /// </summary>
        //string INode.FullName; // => $"{string.Join(" ", AccessibilityString, NameWithArgs, ":", Type)}";

        /// <summary>
        /// 引数リストを持っているかどうか
        /// </summary>
        bool HasArguments { get; } // { get => Arguments?.Any() ?? false; }

    }
}
