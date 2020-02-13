using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Diagram
{
    /// <summary>
    /// エッジを表しているクラス
    /// </summary>
    internal class EdgeNode : IExpression
    {
        /// <summary>
        /// 始点側のノード名
        /// </summary>
        public string SourceNodeName { get; private set; }
        /// <summary>
        /// 終点側のノード名
        /// </summary>
        public string TargetNodeName { get; private set; }
        /// <summary>
        /// 始点方向の矢印の種類
        /// </summary>
        public ArrowType BackArrowType { get; private set; }
        /// <summary>
        /// 終点方向の矢印の種類
        /// </summary>
        public ArrowType ForwardArrowType { get; private set; }

        // TODO: 線の種類のメンバも追加する

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="leftNodeName"> 左側のノード名 </param>
        /// <param name="rightNodeName"> 右側のノード名 </param>
        /// <param name="leftArrowType"> 左方向の矢印の種類</param>
        /// <param name="rightArrowType"> 右方向の矢印の種類 </param>
        public EdgeNode(string leftNodeName, string rightNodeName, ArrowType leftArrowType, ArrowType rightArrowType)
        {
            BackArrowType = leftArrowType;
            ForwardArrowType = rightArrowType;

            // 矢印は多くても片方までしか表示させない
            (SourceNodeName, TargetNodeName) = (leftArrowType, rightArrowType) switch
            {
                (ArrowType.None, ArrowType.None) => (leftNodeName, rightNodeName),
                (ArrowType.None, ArrowType forwardArrow) => (leftNodeName, rightNodeName),
                (ArrowType backArrow, ArrowType.None) => (rightNodeName, leftNodeName),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
