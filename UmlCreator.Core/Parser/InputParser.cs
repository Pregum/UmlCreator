using Sprache;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Diagram;

namespace UmlCreator.Core.Parser
{
    internal class InputParser
    {
        /// <summary>
        /// クラスを表すパーサです。
        /// </summary>
        /// <example> "class Hoge { }</example>
        private readonly static Parser<IRootNode> classDiagram =
            from @class in Parse.String("class").Token().Text()
            from className in Parse.LetterOrDigit.AtLeastOnce().Token().Text()
            from openParenthesense in Parse.Char('{').Token()
            from nodes in dataNode.Or(behaviorNode).Many().Token()
            from closeParenthesense in Parse.Char('}').Token()
            select new ClassNode(className, className, AccessLevel.Package, nodes);

        /// <summary>
        /// フィールドメンバを表すパーサです。
        /// </summary>
        /// <example> "+ name:string" </example>
        private readonly static Parser<INode> dataNode =
        from modifier in (Parse.Char('-').Return(AccessLevel.Private)
            .Or(Parse.Char('~').Return(AccessLevel.Package))
            .Or(Parse.Char('#').Return(AccessLevel.Protected))
            .Or(Parse.Char('+').Return(AccessLevel.Public))
            .Or(Parse.Return(AccessLevel.Package))).Token()
        from name in Parse.ChainOperator(Parse.Char('_'), Parse.LetterOrDigit.Many().Token().Text(), (lhs, rhs, op) => lhs + op)
        from colon in Parse.Char(':').Token()
        from type in Parse.LetterOrDigit.Many().Token().Text()
        select new FieldNode(name, type, modifier);

        private readonly static Parser<INode> ArgumentNode =
            from name in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token().Token().Text()
            from delimiter in Parse.Char(':')
            from type in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token().Token().Text()
            select new ArgumentNode(name, type);

        /// <summary>
        /// メソッドを表すパーサです。
        /// </summary>
        /// <example> "# GetName():string"</example>
        private readonly static Parser<INode> behaviorNode =
        from modifier in (Parse.Char('-').Return(AccessLevel.Private)
            .Or(Parse.Char('~').Return(AccessLevel.Package))
            .Or(Parse.Char('#').Return(AccessLevel.Protected))
            .Or(Parse.Char('+').Return(AccessLevel.Public))
            .Or(Parse.Return(AccessLevel.Package))).Token()
            //from name in Parse.ChainOperator(Parse.Char('_'), Parse.LetterOrDigit.Many().Token().Text(), (lhs, rhs, op) => lhs + op)
        from name in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token().Text()
        from openBlaces in Parse.Char('(').Token()
        // args
        from arguments in ArgumentNode.DelimitedBy(Parse.Char(',')).Or(Parse.Return(default(IEnumerable<INode>)))
        from closeBlaces in Parse.Char(')').Token()
        from colon in Parse.Char(':').Token()
        from type in Parse.LetterOrDigit.Many().Token().Text()
        select new MethodNode(name + openBlaces + closeBlaces, type, modifier, arguments);

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
                return classDiagram.Parse(input);
            }
            catch (ParseException e)
            {
                e.ToString();
                throw new ArgumentException("入力が正しくありません。", e);
            }
        }
    }
}
