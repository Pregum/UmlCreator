﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    /// <summary>
    /// ノードの根本が持っている機能です。
    /// </summary>
    public interface IRootNode : INode
    {
        /// <summary>
        /// 属性(フィールド・プロパティ)のノード
        /// </summary>
        IReadOnlyList<INode> DataNodes { get; }

        /// <summary>
        /// DataNodesの要素の有無
        /// </summary>
        bool HasDataNodes { get; }

        /// <summary>
        /// 操作(メソッド)のノード
        /// </summary>
        IReadOnlyList<IOperationalNode> BehaviorNodes { get; }

        /// <summary>
        /// BehaviorNodesの要素の有無
        /// </summary>
        bool HasBehaviorNodes { get; }
    }
}