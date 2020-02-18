using System.Drawing;
using System.Drawing.Drawing2D;

namespace UmlCreator.Core.Graph.Render
{
    internal class DEdge : DObject
    {
        public bool SelectedForEditing { get; set; }

        private Microsoft.Msagl.Drawing.Edge _drawingEdge;
        public Microsoft.Msagl.Drawing.Edge DrawingEdge { get => _drawingEdge; set => _drawingEdge = value; }
        public Color Color => Draw.MsaglColorToDrawingColor(DrawingEdge.Attr.Color);
        /// <summary>
        /// the radius of circles drawing around polyline corners
        /// </summary>
        public double RadiusOfPolylineCorner { get; set; }
        /// <summary>
        /// Can be set to GraphicsPath of GDI
        /// </summary>
        public GraphicsPath GraphicsPath { get; set; }
        public Microsoft.Msagl.Drawing.Edge Edge => DrawingEdge;

        public DLabel Label { get; set; }

        private float _dashSize;
        internal override float DashSize()
        {
            if (_dashSize > 0)
            {
                return _dashSize;
            }
            var w = (float)DrawingEdge.Attr.LineWidth;
            var dashSizeInPoints = (float)(Draw.dashSize * GViewer.Dpi);
            return _dashSize = dashSizeInPoints / w;
        }
    }
}