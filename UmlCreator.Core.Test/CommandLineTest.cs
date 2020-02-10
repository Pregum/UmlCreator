using System;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Parser;
using Xunit;

namespace UmlCreator.Core.Test
{
    public class CommandLineTest
    {
        [Theory(DisplayName ="入力ファイルパスのテスト")]
        [InlineData(@"C:\tmp\input_test.pu -o C:\tmp\out")]
        public void InputTest1(string inputPath)
        {
            var parser = CommandParser.Parse(inputPath);
            Assert.Equal(@"C:\tmp\input_test.pu", parser.InputPath);
        }

        [Theory(DisplayName ="入力ファイルパスの半角スペーステスト")]
        [InlineData(@"""C:\tmp\input test.pu"" -o C:\tmp\out")]
        public void InputTest2(string inputPath)
        {
            var parser = CommandParser.Parse(inputPath);
            Assert.Equal(@"C:\tmp\input test.pu", parser.InputPath);
        }

        [Theory(DisplayName ="入力ファイルパスの記号テスト")]
        [InlineData(@"C:\tmp\input-test.pu -o C:\tmp\out")]
        public void InputTest3(string inputPath)
        {
            var parser = CommandParser.Parse(inputPath);
            Assert.Equal(@"C:\tmp\input-test.pu", parser.InputPath);
        }

        [Theory(DisplayName ="入力ファイルパスの記号テスト2")]
        [InlineData(@"C:\tmp\..\input_test.pu -o C:\tmp\out")]
        public void InputTest4(string inputPath)
        {
            var parser = CommandParser.Parse(inputPath);
            Assert.Equal(@"C:\tmp\..\input_test.pu", parser.InputPath);
        }

        [Theory(DisplayName ="入力ファイルの相対パステスト1")]
        [InlineData(@"tmp\input_test.pu -o C:\tmp\out")]
        public void RelativeInputPathTest1(string inputPath)
        {
            var parser = CommandParser.Parse(inputPath);
            Assert.Equal(@"tmp\input_test.pu", parser.InputPath);
        }

        [Theory(DisplayName ="出力パスのテスト1")]
        [InlineData(@"tmp\input_test.pu -o C:\tmp\out")]
        public void OutputPathTest1(string val)
        {
            var parser = CommandParser.Parse(val);
            Assert.Equal(@"C:\tmp\out", parser.OutputPath);
        }

        [Theory(DisplayName ="出力パスのテスト2")]
        [InlineData(@"tmp\input_test.pu -o C:\tmp\out  ")]
        public void OutputPathTest2(string val)
        {
            var parser = CommandParser.Parse(val);
            Assert.Equal(@"C:\tmp\out", parser.OutputPath);
        }

        [Theory(DisplayName ="オプションのテスト1")]
        [InlineData(@"tmp\input_test.pu o- C:\tmp\out  ")]
        public void OptionTest1(string val)
        {
            Assert.Throws<Sprache.ParseException>(() => CommandParser.Parse(val));
        }

        [Theory(DisplayName ="出力パスがない時のテスト")]
        [InlineData(@"tmp\input_test.pu -o ")]
        public void OutputPathNothingTest1(string val)
        {
            Assert.Throws<Sprache.ParseException>(() => CommandParser.Parse(val));
        }

        [Theory(DisplayName ="入力パスがない時のテスト")]
        [InlineData(@"-o C:\tmp\out  ")]
        public void InputPathNothingTest1(string val)
        {
            Assert.Throws<Sprache.ParseException>(() => CommandParser.Parse(val));
        }

        // オプションの判定テスト
        [Theory(DisplayName ="オプションの判定テスト2(png)")]
        [InlineData(@"tmp\input_test.pu -o C:\tmp\out --png")]
        public void OptionTest2(string val)
        {
            var parser = CommandParser.Parse(val);
            Assert.Equal(OutputType.Image, parser.OutputType);
        }

        [Theory(DisplayName ="オプションの判定テスト3(ascii)")]
        [InlineData(@"tmp\input_test.pu -o C:\tmp\out --ascii")]
        public void OptionTest3(string val)
        {
            var parser = CommandParser.Parse(val);
            Assert.Equal(OutputType.Ascii, parser.OutputType);
        }

        // オプションのみ記載した場合のテスト
        [Theory(DisplayName ="オプションの判定テスト4")]
        [InlineData(@"tmp\input_test.pu --ascii")]
        public void OptionTest4(string val)
        {
            Assert.Throws<Sprache.ParseException>(() => CommandParser.Parse(val));
        }
    }
}
