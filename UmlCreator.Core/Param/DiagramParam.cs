using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UmlCreator.Core.Diagram;

namespace UmlCreator.Core.Param
{
    internal class DiagramParam
    {
        private List<IRootNode> _rootNodes;
        /// <summary>
        /// クラスノード用のリスト
        /// </summary>
        public IReadOnlyList<IRootNode> RootNodes => _rootNodes.AsReadOnly();

        private List<EdgeNode> _edges;

        /// <summary>
        /// エッジノード用のリスト
        /// </summary>
        public IReadOnlyList<EdgeNode> Edges => _edges.AsReadOnly();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="expressions"> 構文解析されたオブジェクト </param>
        public DiagramParam(IEnumerable<IExpression> expressions)
        {
            _rootNodes = expressions.Where(x => x is IRootNode).Cast<IRootNode>()?.ToList() ?? new List<IRootNode>();
            _edges = expressions.Where(x => x is EdgeNode).Cast<EdgeNode>()?.ToList() ?? new List<EdgeNode>();
        }
    }
}
