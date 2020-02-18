using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace UmlCreator.Core.Graph.Render
{
    internal class DGraph
    {
        Microsoft.Msagl.Drawing.Graph _drawingGraph;
        private readonly Dictionary<IComparable, DNode> _nodeMap = new Dictionary<IComparable, DNode>();

        private readonly List<DEdge> _edges = new List<DEdge>();
        public List<DEdge> Edges => _edges;

        internal void DrawGraph(Graphics graphics)
        {
            // TODO: add to drawing of database for debugging only ref: https://github.com/microsoft/automatic-graph-layout/blob/master/GraphLayout/tools/GraphViewerGDI/DGraph.cs
            if (_drawingGraph.Attr.Border > 0) DrawGraphBorder(_drawingGraph.Attr.Border, graphics);

            // we need to draw the edited edges last
            DEdge dEdgeSelectedForEditing = null;

            foreach (Microsoft.Msagl.Drawing.Subgraph subgraph in _drawingGraph.RootSubgraph.AllSubgraphsWidthFirstExcludingSelf())
            {
                DrawNode(graphics, _nodeMap[subgraph.Id]);
            }

            foreach (DEdge dEdge in Edges)
            {
                if (!dEdge.SelectedForEditing)
                {
                    DrawEdge(graphics, dEdge);
                }
                else // there must be no more than one edge selected for editing
                {
                    dEdgeSelectedForEditing = dEdge;
                }

                foreach (DNode dNode in _nodeMap.Values)
                {
                    if (!(dNode.DrawingNode is Microsoft.Msagl.Drawing.Subgraph))
                    {
                        DrawNode(graphics, dNode);
                    }

                    // draw the selected edge
                    if (dEdgeSelectedForEditing != null)
                    {
                        DrawEdge(graphics, dEdgeSelectedForEditing);
                        DrawUnderlyingPolyline(graphics, dEdgeSelectedForEditing);
                    }

                    // XXX: unable to implement GViewer because GViewer extend System.Windows.Forms.Form. 
                }
            }
        }

        private void DrawUnderlyingPolyline(Graphics graphics, DEdge editedEdge)
        {
            Microsoft.Msagl.Core.Geometry.SmoothedPolyline underlyingPolyline = editedEdge.DrawingEdge.GeometryEdge.UnderlyingPolyline;
            if (underlyingPolyline != null)
            {
                var pen = new Pen(editedEdge.Color, (float)editedEdge.DrawingEdge.Attr.LineWidth);
                IEnumerator<Microsoft.Msagl.Core.Geometry.Point> en = underlyingPolyline.GetEnumerator();
                en.MoveNext();
                PointF p = P2P(en.Current);
                while (en.MoveNext())
                {
                    graphics.DrawLine(pen, p, p = P2P(en.Current));

                    foreach (Microsoft.Msagl.Core.Geometry.Point p2 in underlyingPolyline)
                    {
                        DrawCircleAroungPolylineCorner(graphics, p2, pen, editedEdge.RadiusOfPolylineCorner);
                    }
                }
            }
        }

        private void DrawCircleAroungPolylineCorner(Graphics graphics, Microsoft.Msagl.Core.Geometry.Point p2, Pen pen, double radius)
        {
            graphics.DrawEllipse(pen, (float)(p2.X - radius), (float)(p2.Y - radius), (float)(2 * radius), (float)(2 * radius));
        }

        private PointF P2P(Microsoft.Msagl.Core.Geometry.Point p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        private void DrawEdge(Graphics graphics, DEdge dEdge)
        {
            Microsoft.Msagl.Drawing.Edge drawingEdge = dEdge.DrawingEdge;
            if (!drawingEdge.IsVisible || drawingEdge.GeometryEdge == null)
            {
                return;
            }

            Microsoft.Msagl.Drawing.Edge edge = dEdge.DrawingEdge;
            if (edge.DrawEdgeDelegate != null)
            {
                if (edge.DrawEdgeDelegate(edge, graphics))
                {
                    return; // the client draws instead
                }
            }

            dEdge.GraphicsPath ??= Draw.CreateGraphicsPath(dEdge.Edge.GeometryEdge.Curve);

            EdgeAttr attr = drawingEdge.Attr;

            using var myPen = new Pen(dEdge.Color, (float)attr.LineWidth);
            foreach (Style style in attr.Styles)
            {
                Draw.AddStyleForPen(dEdge, myPen, style);
            }

            try
            {
                if (dEdge.GraphicsPath != null)
                {
                    graphics.DrawPath(myPen, dEdge.GraphicsPath);
                }
            }
            catch
            {
                // sometimes on Vista it throws an out of memory exception without any obvious reason
            }

            Draw.DrawEdgeArrows(graphics, drawingEdge, dEdge.Color, myPen);
            if (dEdge.DrawingEdge.GeometryEdge.Label != null)
            {
                Draw.DrawLabel(graphics, dEdge.Label);
            }

            // IMPL:impl TEST_MSAGL define 
        }

        internal void DrawNode(Graphics g, DNode dnode)
        {
            Microsoft.Msagl.Drawing.Node node = dnode.DrawingNode;
            if (node.IsVisible == false)
            {
                return;
            }

            if (node.DrawNodeDelegate != null)
            {
                if (node.DrawNodeDelegate(node, g))
                {
                    return; // the client draws instead
                }
            }

            // node comes with non-initialized attribute - should not be drawn
            if (node.GeometryNode == null || node.GeometryNode.BoundaryCurve == null)
            {
                return;
            }

            NodeAttr attr = node.Attr;

            using var pen = new Pen(dnode.Color, (float)attr.LineWidth);
            foreach (Style style in attr.Styles)
            {
                Draw.AddStyleForPen(dnode, pen, style);
            }


            // TODO: ノードの形を実装したい時はここをのShape列挙型に処理を追加する
            switch (attr.Shape)
            {
                case Shape.DoubleCircle:
                    Draw.DrawDoubleCircle(g, pen, dnode);
                    break;
                case Shape.Box:
                    Draw.DrawBox(g, pen, dnode);
                    break;
                case Shape.Diamond:
                    Draw.DrawDiamond(g, pen, dnode);
                    break;
                case Shape.Point:
                    Draw.DrawEllipse(g, pen, dnode);
                    break;
                case Shape.Plaintext:
                    {
                        break;
                        // do nothing
                    }
                case Shape.Octagon:
                case Shape.House:
                case Shape.InvHouse:
                case Shape.Ellipse:
                case Shape.DrawFromGeometry:
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
                    Draw.DrawFromMsaglCurve(g, pen, dnode);
                    break;
                default:
                    Draw.DrawEllipse(g, pen, dnode);
                    break;
            }
            Draw.DrawLabel(g, dnode.Label);
        }

        private void DrawGraphBorder(int borderWidth, Graphics graphics)
        {
            using var myPen = new Pen(Draw.MsaglColorToDrawingColor(_drawingGraph.Attr.Color), (float)borderWidth);
            graphics.DrawRectangle(myPen
                , (float)_drawingGraph.Left
                , (float)_drawingGraph.Bottom
                , (float)_drawingGraph.Width
                , (float)_drawingGraph.Height);
        }
    }
}