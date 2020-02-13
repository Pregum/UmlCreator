namespace UmlCreator.Core.Diagram
{
    /// <summary>
    /// 矢印の種類
    /// </summary>
    internal enum ArrowType
    {
        /// <summary>
        /// なし
        /// </summary>
        None,
        /// <summary>
        /// 依存
        /// </summary>
        Dependency,
        /// <summary>
        /// 継承
        /// </summary>
        Extend,
        /// <summary>
        /// 実装
        /// </summary>
        Implement,
    }
}