using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Param
{
    public class CommandLineParam
    {
        /// <summary>
        /// 入力ファイルパス
        /// </summary>
        public string InputPath { get; private set; }

        /// <summary>
        /// 出力ファイルパス
        /// </summary>
        public string OutputPath { get; private set; }

        /// <summary>
        /// 出力形式
        /// </summary>
        public OutputType OutputType { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="inputPath"> 入力ファイルパス </param>
        /// <param name="outputPath"> 出力ファイルパス </param>
        /// <param name="outputType"> 出力形式 </param>
        public CommandLineParam(string inputPath, string outputPath, OutputType outputType)
        {
            InputPath = inputPath ?? throw new ArgumentNullException(nameof(inputPath));
            OutputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            OutputType = outputType;
        }
    }
}
