using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace UmlCreator.Core.Graph.Render
{
    public sealed class GraphRenderer
    {
        object layedOutGraph;

        private Microsoft.Msagl.Drawing.Graph _graph;

        public GraphRenderer(Microsoft.Msagl.Drawing.Graph drGraph)
        {
            _graph = drGraph;
        }

        public void CalculateLayout()
        {
            // FIXME: ここはヘルパークラスを使うようにしてGViewerを使用しないようにする。
            throw new NotImplementedException();
        }

        /// <summary>
        /// renders the graph on the image
        /// </summary>
        /// <param name="image"></param>
        public void Render(Image image)
        {
            if (image != null)
            {
                Render(Graphics.FromImage(image), 0, 0, image.Width, image.Height);
            }
        }

        /// <summary>
        /// renders the graph inside of the rectangle xleft, ytop, width, height
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void Render(Graphics graphics, int left, int top, int width, int height)
        {
            Render(graphics, new System.Drawing.Rectangle(left, top, width, height));
        }

        /// <summary>
        /// renders the graph inside of the rectangle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rectangle"></param>
        private void Render(Graphics graphics, Rectangle rectangle)
        {
            if (graphics != null)
            {
                if (layedOutGraph == null)
                {
                    CalculateLayout();
                }

                double s = Math.Min(rectangle.Width / _graph.Width, rectangle.Height / _graph.Height);
                double xoffset = rectangle.Left + 0.5 * rectangle.Width - s * (_graph.Left + 0.5 * _graph.Width);
                double yoffset = rectangle.Top + 0.5 * rectangle.Height + s * (_graph.Bottom + 0.5 *_graph.Height);
                using var sb = new SolidBrush(Draw.MsaglColorToDrawingColor(_graph.Attr.BackgroundColor));
                graphics.FillRectangle(sb, rectangle);

                using var m = new System.Drawing.Drawing2D.Matrix((float)s, 0, 0, (float)-s, (float)xoffset, (float)yoffset);
                graphics.Transform = m;

                Draw.DrawPrecalculatedLayoutObject(graphics, layedOutGraph);
            }
        }
    }
}
