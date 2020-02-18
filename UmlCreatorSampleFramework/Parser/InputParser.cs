using Sprache;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Param;

namespace UmlCreator.Core.Parser
{
    internal class InputParser
    {
        /// <summary>
        /// クラスを表すパーサです。
        /// </summary>
        /// <example>class Hoge { }</example>
        private readonly static Parser<IExpression> ClassDiagram =
            from @class in Parse.String("class").Token().Text()
            from className in Parse.LetterOrDigit.AtLeastOnce().Token().Text()
            from openParenthesense in Parse.Char('{').Token()
            from nodes in DataNode.Or(BehaviorNode).Many().Token()
            from closeParenthesense in Parse.Char('}').Token()
            select new ClassNode(className, className, AccessLevel.Package, nodes);

        /// <summary>
        /// フィールドメンバを表すパーサです。
        /// </summary>
        /// <example> "+ name:string" </example>
        private readonly static Parser<INode> DataNode =
        from modifier in (Parse.Char('-').Return(AccessLevel.Private)
            .Or(Parse.Char('~').Return(AccessLevel.Package))
            .Or(Parse.Char('#').Return(AccessLevel.Protected))
            .Or(Parse.Char('+').Return(AccessLevel.Public))
            .Or(Parse.Return(AccessLevel.Package))).Token()
        from name in Parse.ChainOperator(Parse.Char('_'), Parse.LetterOrDigit.Many().Token().Text(), (lhs, rhs, op) => lhs + op)
        from colon in Parse.Char(':').Token()
        from type in Parse.LetterOrDigit.Many().Token().Text()
        select new FieldNode(name, type, modifier);

        /// <summary>
        /// メソッドの引数を表すパーサです。
        /// </summary>
        /// <example> count: int</example>
        private readonly static Parser<INode> ArgumentNode =
            from name in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token().Token().Text()
            from delimiter in Parse.Char(':')
            from type in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token().Token().Text()
            select new ArgumentNode(name, type);

        /// <summary>
        /// メソッドを表すパーサです。
        /// </summary>
        /// <example> "# GetName(hoge:int, test:string):string"</example>
        private readonly static Parser<INode> BehaviorNode =
        from modifier in (Parse.Char('-').Return(AccessLevel.Private)
            .Or(Parse.Char('~').Return(AccessLevel.Package))
            .Or(Parse.Char('#').Return(AccessLevel.Protected))
            .Or(Parse.Char('+').Return(AccessLevel.Public))
            .Or(Parse.Return(AccessLevel.Package))).Token()
        from name in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token().Text()
        from openBlaces in Parse.Char('(').Token()
            // args
        from arguments in ArgumentNode.DelimitedBy(Parse.Char(',')).Or(Parse.Return(default(IEnumerable<INode>)))
        from closeBlaces in Parse.Char(')').Token()
        from colon in Parse.Char(':').Token()
        from type in Parse.LetterOrDigit.Many().Token().Text()
        select new MethodNode(name, type, modifier, arguments);

        /// <summary>
        /// エッジを表すパーサです。
        /// </summary>
        /// <example> "XXX ---> YYY" </example>
        private readonly static Parser<IExpression> EdgeParser =
            from leftNode in Parse.LetterOrDigit.AtLeastOnce().Token().Text()
            from leftArrow in Parse.String("<|").Return(ArrowType.Extend).Or(Parse.String("<").Return(ArrowType.Dependency)).Or(Parse.Return(ArrowType.None))
            from line in Parse.Char('-').Or(Parse.Char('.')).AtLeastOnce().Token().Text()
            from rightArrow in Parse.String("|>").Return(ArrowType.Extend).Or(Parse.String(">").Return(ArrowType.Dependency)).Or(Parse.Return(ArrowType.None))
            from rightNode in Parse.LetterOrDigit.AtLeastOnce().Token().Text()
            select new EdgeNode(leftNode, rightNode, leftArrow, rightArrow);

        /// <summary>
        /// エッジ群を表すパーサです。
        /// </summary>
        private readonly static Parser<IEnumerable<IExpression>> EdgesParser =
            EdgeParser.AtLeastOnce().Token();

        /// <summary>
        /// クラス群を表すパーサです。
        /// </summary>
        private readonly static Parser<IEnumerable<IExpression>> ClassesDiagramParser =
            ClassDiagram.Token().AtLeastOnce();

        /// <summary>
        /// クラス群とエッジ群をまとめたパーサです。
        /// </summary>
        private readonly static Parser<DiagramParam> DiagramParser =
            from expression in ClassDiagram.Or(EdgeParser).AtLeastOnce().Token()
            select new DiagramParam(expression);

        /// <summary>
        /// ctor
        /// </summary>
        public InputParser()
        {
        }

        /// <summary>
        /// 入力された文字列をクラス図オブジェクトにパースします。
        /// </summary>
        /// <param name="input"> DSLで書かれた文字列 </param>
        /// <exception cref="ArgumentException"> 文字列がクラス図オブジェクトにパースできなかった場合、発生します。</exception>
        /// <returns>クラス図オブジェクト </returns>
        public IRootNode ParseDiagram(string input)
        {
            try
            {
                return ClassDiagram.Parse(input) as IRootNode;
            }
            catch (ParseException ex)
            {
                throw new ArgumentException("入力が正しくありません。", ex);
            }
        }

        #region テスト用メソッド

        /// <summary>
        /// クラスノードの集合体 + エッジノードの集合体を解析するメソッド
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal DiagramParam ParseDiagrams(string input)
        {
            try
            {
                return DiagramParser.Parse(input);
            }
            catch (ParseException ex)
            {
                throw new ArgumentException("入力が正しくありません。", ex);
            }
        }

        /// <summary>
        /// エッジノードを解析するメソッド
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal EdgeNode ParseEdge(string input)
        {
            return EdgeParser.Parse(input) as EdgeNode;
        }

        #endregion
    }
}
