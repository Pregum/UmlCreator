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
        /// 引数リストを持っているかどうか
        /// </summary>
        bool HasArguments { get => Arguments?.Any() ?? false; }

    }
}
