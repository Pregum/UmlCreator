using System.Drawing;

namespace UmlCreator.Core.Graph.Render
{
    public sealed class DLabel : DObject
    {
        public Microsoft.Msagl.Drawing.Label DrawingLabel { get;  set; }
        public Font Font { get; set; }

        internal override float DashSize()
        {
            return 1; // it is never used
        }
    }
}