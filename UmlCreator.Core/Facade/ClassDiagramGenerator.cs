using System;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Builder;
using UmlCreator.Core.Diagram;

using UmlCreator.Core.Parser;

namespace UmlCreator.Core.Facade
{
    public class ClassDiagramGenerator<T>
    {
        public IRootNode InputDiagram { get; private set; }
        public T OutputDiagram { get; private set; }
        private InputParser _parser;
        private IBuilder<T> _builder;

        internal ClassDiagramGenerator(IBuilder<T> builder)
        {
            _builder = builder;
            _parser = new InputParser();
        }

        public void GenerateClassDiagram(string diagramText)
        {
            InputDiagram = _parser.ParseDiagram(diagramText);
            OutputDiagram = _builder.MakeDiagram(InputDiagram);
        }
    }
}
