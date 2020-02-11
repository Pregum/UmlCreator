using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UmlCreator.Core.Builder;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Param;
using UmlCreator.Core.Parser;
using UmlCreator.Core.Serializer;

namespace UmlCreator.Core.Facade
{
    /// <summary>
    /// クラス図を生成するクラスです。
    /// </summary>
    /// <typeparam name="T"> 生成するクラス図の型です。 </typeparam>
    public class ClassDiagramGenerator<T>
    {
        /// <summary>
        /// パースされたクラス図
        /// </summary>
        public IRootNode InputDiagram { get; private set; }
        /// <summary>
        /// 生成されたクラス図
        /// </summary>
        public T OutputDiagram { get; private set; }

        private InputParser _parser;
        private IBuilder<T> _builder;
        private ISerializer<T> _serializer;

        internal ClassDiagramGenerator(IBuilder<T> builder, ISerializer<T> serializer)
        {
            _builder = builder;
            _serializer = serializer;
            _parser = new InputParser();
        }

        /// <summary>
        /// クラス図を生成します。
        /// </summary>
        /// <param name="diagramText"> 入力されたテキスト </param>
        public void GenerateClassDiagram(string diagramText)
        {
            InputDiagram = _parser.ParseDiagram(diagramText);
            OutputDiagram = _builder.MakeDiagram(InputDiagram);
        }

        /// <summary>
        /// クラス図を生成します。
        /// </summary>
        /// <param name="param"> コマンドラインから入力されたパラメータ </param>
        public void GenerateClassDiagram(CommandLineParam param)
        {
            FileParam fileParam = FileParser.Parse(param);
            var fileInfo = new FileInfo(fileParam.Param.OutputPath);

            InputDiagram = _parser.ParseDiagram(fileParam.DiagramSource);
            OutputDiagram = _builder.MakeDiagram(InputDiagram);
            _serializer.Serialize(OutputDiagram, fileInfo);

            // TODO: ログを追加する
        }
    }
}
