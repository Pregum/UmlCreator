using System;
using System.Collections.Generic;
using System.Text;
using UmlCreator.Core.Diagram;

namespace UmlCreator.Core.Builder
{
    internal interface IBuilder<T>
    {
        T MakeDiagram(IRootNode rootNode);
    }
}
