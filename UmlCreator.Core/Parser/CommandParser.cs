using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UmlCreator.Core.Param;

namespace UmlCreator.Core.Parser
{
    public class CommandParser
    {
        private readonly static Parser<string> NormalInputParser =
            from input in Sprache.Parse.CharExcept(' ').AtLeastOnce().Token().Text()
            select input;

        private readonly static Parser<string> QuoteInputParser =
            from openQuote in Sprache.Parse.Char('"')
            from input in Sprache.Parse.CharExcept('"').AtLeastOnce().Token().Text()
            from closeQuote in Sprache.Parse.Char('"')
            select input;

        /// <summary>
        /// 入力パスのパーサー
        /// </summary>
        private readonly static Parser<string> InputParser =
            QuoteInputParser.Or(NormalInputParser);

        private readonly static Parser<string> NormalOutputParser =
            from output in Sprache.Parse.CharExcept(' ').AtLeastOnce().Token().Text()
            select output;

        private readonly static Parser<string> QuoteOutputParser =
            from openQuote in Sprache.Parse.Char('"')
            from output in Sprache.Parse.CharExcept('"').AtLeastOnce().Token().Text()
            from closeQuote in Sprache.Parse.Char('"')
            select output;

        /// <summary>
        /// 出力パスのパーサー
        /// </summary>
        private readonly static Parser<string> OutputParser =
            QuoteOutputParser.Or(NormalOutputParser);

        private readonly static Parser<OutputType> OutputTypeOption =
            from option in Sprache.Parse.String("--png").Or(Sprache.Parse.String("--ascii")).Token().Text().Optional()
            select option.IsEmpty ? default : option.Get() == "--png" ? OutputType.Image : OutputType.Ascii;

        private readonly static Parser<CommandLineParam> Parser =
            from input in InputParser
            from output in Sprache.Parse.String("-o").Token().Then(x => OutputParser)
            from options in OutputTypeOption
            select new CommandLineParam(input, output, options);


        public static CommandLineParam Parse(string input)
        {
            var result = Parser.Parse(input);
            return result;
        }
    }
}
