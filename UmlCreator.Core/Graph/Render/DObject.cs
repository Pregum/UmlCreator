using System;

namespace UmlCreator.Core.Graph.Render
{
    public abstract class DObject
    {
        internal float[] DashPatternArray { get;  set; }

        internal abstract float DashSize();
    }
}