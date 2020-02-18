using Microsoft.Msagl.Drawing;
using System.Drawing;

namespace UmlCreator.Core.Graph.Render
{
    internal class DNode : DObject
    {
        private float _dashSize;
        public Microsoft.Msagl.Drawing.Node DrawingNode { get; set; }
        public System.Drawing.Color Color => Draw.MsaglColorToDrawingColor(DrawingNode.Attr.Color);

        public System.Drawing.Color FillColor => Draw.MsaglColorToDrawingColor(DrawingNode.Attr.FillColor);

        /// <summary>
        /// gets / sets the rendered label of the object
        /// </summary>
        public DLabel Label { get; set; }

        internal override float DashSize()
        {
            if (_dashSize > 0)
            {
                return _dashSize;
            }
            var w = (float)DrawingNode.Attr.LineWidth;
            if (w < 0)
            {
                return 1;
            }
            var dashSizeInPoints = (float)(Draw.dashSize * GViewer.Dpi);
            return _dashSize = dashSizeInPoints / w;
        }
    }
}