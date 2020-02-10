using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmlCreator.Core.Parser
{
    public class CommandParser
    {

        public string InputPath { get; private set; }
        public string OutputPath { get; private set; }
        public OutputType OutputType { get; private set; }

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

        private readonly static Parser<CommandParser> Parser =
            from input in InputParser
            from output in Sprache.Parse.Regex("-o").Then(x => OutputParser)
            select new CommandParser(input, output);


        public static CommandParser Parse(string input)
        {
            var result = Parser.Parse(input);
            return result;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public CommandParser(string input, string output)
        {
            InputPath = input;
            OutputPath = output;
        }
    }
}
