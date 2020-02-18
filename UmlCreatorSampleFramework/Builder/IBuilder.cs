using System;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Param;

namespace UmlCreator.Core.Builder
{
    /// <summary>
    /// 図を生成するオブジェクトが持っている機能です。
    /// </summary>
    /// <typeparam name="T">生成する図の型です。</typeparam>
    internal interface IBuilder<T>
    {
        /// <summary>
        /// 図を生成します。
        /// </summary>
        /// <param name="rootNode">図を作成する為に必要な情報</param>
        /// <returns>図</returns>
        T MakeDiagram(DiagramParam rootNode);

    }
}
