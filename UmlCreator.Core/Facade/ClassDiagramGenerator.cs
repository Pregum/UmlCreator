using System;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Builder;
using UmlCreator.Core.Diagram;

using UmlCreator.Core.Parser;

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

        internal ClassDiagramGenerator(IBuilder<T> builder)
        {
            _builder = builder;
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
    }
}
