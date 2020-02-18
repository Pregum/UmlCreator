using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Param;

namespace UmlCreator.Core.Builder
{
    internal class AsciiBuilder : IBuilder<string>
    {
        private int _stringMaxLength = -1;
        private string _oneSpace = " ";
        private string _oneHyphen = "-";

        /// <summary>
        /// 文字列でクラス図を作成します
        /// </summary>
        /// <param name="rootNode"> 根本のノードです。 </param>
        /// <returns></returns>
        public string MakeDiagram(DiagramParam diagram)
        {
            // FIXME: 複数のクラス図をASCIIで表示できるように修正する
            var rootNode = diagram.RootNodes.First();
            if (rootNode is null)
            {
                throw new ArgumentNullException(nameof(rootNode));
            }

            var sb = new StringBuilder();

            _stringMaxLength = GetMaxLengthOfString(rootNode);

            sb.AppendLine(MakeDivider());
            sb.AppendLine(MakeHeader(rootNode.Name));
            sb.AppendLine(MakeDivider());
            if (rootNode.HasDataNodes) sb.AppendLine(MakeAttributeBlock(rootNode.DataNodes));
            sb.AppendLine(MakeDivider());
            if (rootNode.HasBehaviorNodes) sb.AppendLine(MakeBehaviorBlock(rootNode.BehaviorNodes));
            sb.AppendLine(MakeDivider());

            return sb.ToString();
        }

        private int GetMaxLengthOfString(IRootNode rootNode)
        {
            var classFullNameLength = rootNode.FullName.Length;
            var dataNodeFullNameLength = rootNode.HasDataNodes ? rootNode.DataNodes.Max(x => x.FullName.Length) : 0;
            var behaviorNodeFullNameLength = rootNode.HasBehaviorNodes ? rootNode.BehaviorNodes.Max(x => x.FullName.Length) : 0;
            return new[] { classFullNameLength, dataNodeFullNameLength, behaviorNodeFullNameLength }.Max();
        }

        private string MakeHeader(string className) => AppendSideLine(DecorateSide(className, _oneSpace));

        private string MakeAttributeBlock(IReadOnlyList<INode> dataNodes)
            => string.Join(Environment.NewLine, dataNodes.Select(node => AppendSideLine(DecorateSide(node.FullName, _oneSpace))));

        private string MakeBehaviorBlock(IReadOnlyList<INode> behaviorNodes)
            => string.Join(Environment.NewLine, behaviorNodes.Select(node => AppendSideLine(DecorateSide(node.FullName, _oneSpace))));

        private string MakeDivider() => AppendSideLine(DecorateSide(new string('-', _stringMaxLength), _oneHyphen));

        private string DecorateSide(string inner, string sideStr) => sideStr + inner.PadRight(_stringMaxLength) + sideStr;

        private string AppendSideLine(string inner) => "|" + inner + "|";
    }
}
