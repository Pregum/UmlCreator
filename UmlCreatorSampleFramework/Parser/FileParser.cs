using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UmlCreator.Core.Param;

namespace UmlCreator.Core.Parser
{
    internal class FileParser
    {

        /// <summary>
        /// ファイルからクラス図のソースを読み込みます。
        /// </summary>
        /// <param name="param"> コマンドラインから入力されたパラメータ </param>
        /// <exception cref="System.IO.IOException"> 入力ファイルが存在しない場合に発生します。 </exception>
        /// <exception cref="System.ArgumentNullException"> 引数がnullの場合発生します。 </exception>
        /// <returns></returns>
        public static FileParam Parse(CommandLineParam param)
        {
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!File.Exists(param.InputPath))
            {
                throw new IOException($"not found {nameof(param.InputPath)}");
            }

            //using var sr = new StreamReader(param.InputPath);
            using (var sr = new StreamReader(param.InputPath))
            {
                string classDiagramSource = sr.ReadToEnd();
                return new FileParam(param, classDiagramSource);
            }
        }
    }
}
