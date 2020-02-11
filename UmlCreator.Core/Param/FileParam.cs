using System;
using System.Collections.Generic;
using System.Text;

namespace UmlCreator.Core.Param
{
    internal class FileParam
    {
        /// <summary>
        /// コマンドラインでパースされたパラメータ
        /// </summary>
        public CommandLineParam Param { get; private set; }

        /// <summary>
        /// クラス図を生成するソースコード
        /// </summary>
        public string DiagramSource { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="param"> コマンドラインでパースされたパラメータ </param>
        /// <param name="diagramSource"> クラス図を生成するソースコード </param>
        public FileParam(CommandLineParam param, string diagramSource)
        {
            Param = param ?? throw new ArgumentNullException(nameof(param));
            DiagramSource = diagramSource ?? throw new ArgumentNullException(nameof(diagramSource));
        }
    }
}
