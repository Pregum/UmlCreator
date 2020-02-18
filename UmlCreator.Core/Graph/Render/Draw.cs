using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace UmlCreator.Core.Graph.Render
{
    /// <summary>
    /// exposes some drawing functionality
    /// </summary>
    internal sealed class Draw
    {
        /// <summary>
        /// a color converter
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <returns></returns>
        internal static System.Drawing.Color MsaglColorToDrawingColor(Microsoft.Msagl.Drawing.Color backgroundColor)
        {
            return System.Drawing.Color.FromArgb(backgroundColor.A, backgroundColor.R, backgroundColor.G, backgroundColor.B);
        }

        internal static void DrawPrecalculatedLayoutObject(Graphics graphics, object layedOutGraph)
        {
            var dg = layedOutGraph as DGraph;
            dg?.DrawGraph(graphics);
        }

        internal static void DrawEllipse(Graphics g, Pen pen, DNode dNode)
        {
            var drNode = dNode.DrawingNode;
            NodeAttr nodeAttr = drNode.Attr;
            var width = (float)drNode.Width;
            var height = (float)drNode.Height;
            var x = (float)(drNode.Pos.X - width / 2.0);
            var y = (float)(drNode.Pos.Y - height / 2.0);

            DrawEllipseOnPosition(dNode, nodeAttr, g, x, y, width, height, pen);
        }

        private static void DrawEllipseOnPosition(DNode dNode, NodeAttr nodeAttr, Graphics g, float x, float y, float width, float height, Pen pen)
        {
            if (NeedToFill(dNode.FillColor))
            {
                g.FillEllipse(new SolidBrush(dNode.FillColor), x, y, width, height);
            }
            if (nodeAttr.Shape == Shape.Point)
            {
                g.FillEllipse(new SolidBrush(pen.Color), x, y, width, height);
            }

            g.DrawEllipse(pen, x, y, width, height);
        }

        private static bool NeedToFill(System.Drawing.Color fillColor)
        {
            return fillColor.A != 0; // the color is not transparent
        }

        internal static GraphicsPath CreateGraphicsPath(ICurve iCurve)
        {
            var graphicsPath = new GraphicsPath();
            if (iCurve == null)
            {
                return null;
            }

            if (iCurve is Curve c)
            {
                HandleCurve(c, graphicsPath);
            }
            else if (iCurve is LineSegment ls)
            {
                graphicsPath.AddLine(PointF(ls.Start), PointF(ls.End));
            }
            else if (iCurve is CubicBezierSegment seg)
            {
                graphicsPath.AddBezier(PointF(seg.B(0)), PointF(seg.B(1)), PointF(seg.B(2)), PointF(seg.B(3)));
            }
            else if (iCurve is Ellipse ellipse)
            {
                AddEllipseSeg(graphicsPath, ellipse);
            }
            else if (iCurve is Polyline poly)
            {
                HandlePolyline(poly, graphicsPath);
            }
            else
            {
                var rr = (RoundedRect)iCurve;
                HandleCurve(rr.Curve, graphicsPath);
            }

            return graphicsPath;
        }

        private static void HandlePolyline(Polyline poly, GraphicsPath graphicsPath)
        {
            graphicsPath.AddLines(poly.Select(PointF).ToArray());
            if (poly.Closed)
            {
                graphicsPath.CloseFigure();
            }
        }

        private static void AddEllipseSeg(GraphicsPath graphicsPath, Ellipse el)
        {
            double sweepAngle;
            Microsoft.Msagl.Core.Geometry.Rectangle box;
            float startAngle;
            GetGdiArcDimensions(el, out startAngle, out sweepAngle, out box);

            graphicsPath.AddArc((float)box.Left,
                (float)box.Bottom,
                (float)box.Width,
                (float)box.Height,
                startAngle,
                (float)sweepAngle);
        }

        static readonly double ToDegreesMultiplier = 180 / Math.PI;

        internal static float dashSize = 0.05f; // inches

        private static double doubleCircleOffsetRatio = 0.9;

        internal static double DoubleCircleOffsetRatio => doubleCircleOffsetRatio;

        /// <summary>
        /// it is a very tricky function, please change carefully
        /// </summary>
        /// <param name="el"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <param name="box"></param>
        public static void GetGdiArcDimensions(Ellipse el, out float startAngle, out double sweepAngle, out Microsoft.Msagl.Core.Geometry.Rectangle box)
        {
            box = el.FullBox();
            startAngle = EllipseStandardAngle(el, el.ParStart);
            bool orientedCcw = el.OrientedCounterclockwise();
            if (Math.Abs(Math.Abs(el.ParEnd - el.ParStart) - Math.PI * 2) < 0.001) // we have a full ellipse
            {
                sweepAngle = 360;
            }
            else
            {
                sweepAngle = (orientedCcw ? Microsoft.Msagl.Core.Geometry.Point.Angle(el.Start, el.Center, el.End)
                                          : Microsoft.Msagl.Core.Geometry.Point.Angle(el.End, el.Center, el.Start));
            }
            if (!orientedCcw)
            {
                sweepAngle = -sweepAngle;
            }
        }

        private static float EllipseStandardAngle(Ellipse el, double angle)
        {
            Microsoft.Msagl.Core.Geometry.Point p = Math.Cos(angle) * el.AxisA + Math.Sin(angle) * el.AxisB;
            return (float)(Math.Atan2(p.Y, p.X) * ToDegreesMultiplier);
        }

        internal static PointF PointF(Microsoft.Msagl.Core.Geometry.Point point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }

        internal static void AddStyleForPen(DObject dObj, Pen myPen, Style style)
        {
            if (style == Style.Dashed)
            {
                myPen.DashStyle = DashStyle.Dash;

                if (dObj.DashPatternArray == null)
                {
                    float f = dObj.DashSize();
                    dObj.DashPatternArray = new[] { f, f };
                }

                myPen.DashPattern = dObj.DashPatternArray;

                myPen.DashOffset = dObj.DashPatternArray[0];
            }
            else if (style == Style.Dotted)
            {
                myPen.DashStyle = DashStyle.Dash;
                if (dObj.DashPatternArray == null)
                {
                    float f = dObj.DashSize();
                    dObj.DashPatternArray = new[] { 1, f };
                }
                myPen.DashPattern = dObj.DashPatternArray;
            }
        }

        internal static void DrawDiamond(Graphics g, Pen pen, DNode dnode)
        {
            var drNode = dnode.DrawingNode;
            NodeAttr nodeAttr = drNode.Attr;

            double w2 = drNode.Width / 2.0f;
            double h2 = drNode.Height / 2.0f;
            double cx = drNode.Pos.X;
            double cy = drNode.Pos.Y;
            var ps = new[]
            {
                 new PointF((float) cx - (float) w2, (float) cy) // left vertex
                ,new PointF((float) cx, (float) cy + (float) h2) // bottom vertex
                ,new PointF((float) cx + (float) w2, (float) cy) // right vertex
                ,new PointF((float) cx, (float) cy - (float) h2) // top vertex
            };

            if (NeedToFill(dnode.FillColor))
            {
                System.Drawing.Color fc = FillColor(nodeAttr);
                g.FillPolygon(new SolidBrush(fc), ps);
            }

            g.DrawPolygon(pen, ps);
        }

        internal static void DrawFromMsaglCurve(Graphics g, Pen pen, DNode dnode)
        {
            var drNode = dnode.DrawingNode;
            NodeAttr attr = dnode.DrawingNode.Attr;
            var iCurve = drNode.GeometryNode.BoundaryCurve;
            if (iCurve is Curve c)
            {
                DrawCurve(dnode, c, g, pen);
            }
            else if (iCurve is Ellipse ellipse)
            {
                double w = ellipse.AxisA.X;
                double h = ellipse.AxisB.Y;
                DrawEllipseOnPosition(dnode, dnode.DrawingNode.Attr, g, (float)(ellipse.Center.X - w),
                    (float)(ellipse.Center.Y - h), (float)w * 2, (float)h * 2, pen);
            }
            else if (iCurve is Polyline poly)
            {
                var path = new GraphicsPath();
                path.AddLines(poly.Select(p => new Point((int)p.X, (int)p.Y)).ToArray());
                path.CloseAllFigures();
                if (NeedToFill(dnode.FillColor))
                {
                    g.FillPath(new SolidBrush(dnode.FillColor), path);
                }
                g.DrawPath(pen, path);
            }
            else if (iCurve is RoundedRect roundedRect)
            {
                DrawCurve(dnode, roundedRect.Curve, g, pen);
            }
        }

        private static void DrawCurve(DNode dnode, Curve c, Graphics g, Pen pen)
        {
            var path = new GraphicsPath();
            foreach (ICurve seg in c.Segments)
            {
                AddSegToPath(seg, ref path);
            }

            if (NeedToFill(dnode.FillColor))
            {
                g.FillPath(new SolidBrush(dnode.FillColor), path);
            }

            g.DrawPath(pen, path);
        }

        private static void AddSegToPath(ICurve seg, ref GraphicsPath path)
        {
            if (seg is LineSegment line)
            {
                path.AddLine(PointF(line.Start), PointF(line.End));
            }
            else if (seg is CubicBezierSegment cb)
            {
                path.AddBezier(PointF(cb.B(0)), PointF(cb.B(1)), PointF(cb.B(2)), PointF(cb.B(3)));
            }
            else if (seg is Ellipse ellipse)
            {
                double cx = ellipse.Center.X;
                double cy = ellipse.Center.Y;
                double w = ellipse.AxisA.X * 2;
                double h = ellipse.AxisA.Y * 2;
                double sweep = ellipse.ParEnd - ellipse.ParStart;

                if (sweep < 0)
                {
                    sweep += Math.PI * 2;
                    const double toDegree = 180 / Math.PI;
                    path.AddArc((float)(cx - w / 2), (float)(cy - h / 2), (float)w, (float)h,
                        (float)(ellipse.ParStart * toDegree), (float)(sweep * toDegree));
                }
            }
        }

        internal static void DrawBox(Graphics g, Pen pen, DNode dnode)
        {
            var drNode = dnode.DrawingNode;
            NodeAttr nodeAttr = drNode.Attr;
            if (nodeAttr.XRadius == 0 || nodeAttr.YRadius == 0)
            {
                double x = drNode.GeometryNode.Center.X - drNode.Width / 2.0f;
                double y = drNode.GeometryNode.Center.Y - drNode.Height / 2.0f;

                if (NeedToFill(dnode.FillColor))
                {
                    System.Drawing.Color fc = FillColor(nodeAttr);
                    g.FillRectangle(new SolidBrush(fc), (float)x, (float)y, (float)drNode.Width,
                                    (float)drNode.Height);
                }

                g.DrawRectangle(pen, (float)x, (float)y, (float)drNode.Width, (float)drNode.Height);
            }
            else
            {
                var width = (float)drNode.Width;
                var height = (float)drNode.Height;
                var xRadius = (float)nodeAttr.XRadius;
                var yRadius = (float)nodeAttr.YRadius;
                using (var path = new GraphicsPath())
                {
                    FillTheGraphicsPath(drNode, width, height, ref xRadius, ref yRadius, path);

                    if (NeedToFill(dnode.FillColor))
                    {
                        g.FillPath(new SolidBrush(dnode.FillColor), path);
                    }


                    g.DrawPath(pen, path);
                }
            }
        }

        private static void FillTheGraphicsPath(Microsoft.Msagl.Drawing.Node drNode, float width, float height, ref float xRadius, ref float yRadius, GraphicsPath path)
        {
            NodeAttr nodeAttr = drNode.Attr;
            float w = (width / 2);
            if (xRadius > w)
            {
                xRadius = w;
            }
            float h = (height / 2);
            if (yRadius > h)
            {
                yRadius = h;
            }
            var x = (float)drNode.GeometryNode.Center.X;
            var y = (float)drNode.GeometryNode.Center.Y;
            float ox = w - xRadius;
            float oy = h - yRadius;
            float top = y + h;
            float bottom = y - h;
            float left = x - w;
            float right = x + w;

            const float PI = 180;
            if (ox > 0)
            {
                path.AddLine(x - ox, bottom, x + ox, bottom);
            }
            path.AddArc(x + ox - xRadius, y - oy - yRadius, 2 * xRadius, 2 * yRadius, 1.5f * PI, 0.5f * PI);

            if (oy > 0)
            {
                path.AddLine(right, y - oy, right, y + oy);
            }
            path.AddArc(x + ox - xRadius, y + oy - yRadius, 2 * xRadius, 2 * yRadius, 0, 0.5f * PI);

            if (ox > 0)
            {
                path.AddLine(x + ox, top, x - ox, top);
            }
            path.AddArc(x - ox - xRadius, y + oy - yRadius, 2 * xRadius, 2 * yRadius, 0.5f * PI, 0.5f * PI);

            if (oy > 0)
            {
                path.AddLine(left, y + oy, left, y - oy);
            }
            path.AddArc(x - ox - xRadius, y - oy - yRadius, 2 * xRadius, 2 * yRadius, PI, 0.5f * PI);
        }

        private static System.Drawing.Color FillColor(NodeAttr nodeAttr)
        {
            return MsaglColorToDrawingColor(nodeAttr.FillColor);
        }

        // don't know what to do about the throw-catch block
        internal static void DrawLabel(Graphics graphics, DLabel label)
        {
            if (label == null || label.DrawingLabel.Width == 0)
            {
                return;
            }

            var rectF = GetLabelRect(label);
            try
            {
                DrawStringInRectCenter(graphics, new SolidBrush(MsaglColorToDrawingColor(label.DrawingLabel.FontColor)), label.Font, label.DrawingLabel.Text, rectF);
            }
            catch 
            {
            }
            // XXX: 実装途中
        }

        private static RectangleF GetLabelRect(DLabel label)
        {
            if (label.DrawingLabel.Owner is Subgraph subgraph)
            {
                var cluster = (Cluster)subgraph.GeometryNode;
                var rb = cluster.RectangularBoundary;
                double cy = cluster.BoundingBox.Top;
                double cx = rb.Rect.Left + rb.Rect.Width / 2;
                cy -= subgraph.Attr.ClusterLabelMargin switch
                {
                    LgNodeInfo.LabelPlacement.Top => rb.TopMargin / 2,
                    LgNodeInfo.LabelPlacement.Bottom => rb.BottomMargin / 2,
                    LgNodeInfo.LabelPlacement.Left => rb.LeftMargin / 2,
                    LgNodeInfo.LabelPlacement.Right => rb.RightMargin / 2,
                    _ => throw new InvalidOperationException(),
                };
                var size = label.DrawingLabel.Size;
                return new RectangleF(
                    (float)(cx - size.Width / 2)
                   , (float)(cy - size.Height / 2)
                   , (float)size.Width
                   , (float)size.Height);
            }
            else
            {
                var rectF = new RectangleF(
                    (float)label.DrawingLabel.Left
                   , (float)label.DrawingLabel.Bottom
                   , (float)label.DrawingLabel.Size.Width
                   , (float)label.DrawingLabel.Size.Height);
                return rectF;
            }
        }

        internal static void DrawDoubleCircle(Graphics g, Pen pen, DNode dnode)
        {
            var drNode = dnode.DrawingNode;
            NodeAttr nodeAttr = drNode.Attr;

            double x = drNode.GeometryNode.Center.X - drNode.GeometryNode.Width / 2.0f;
            double y = drNode.GeometryNode.Center.Y - drNode.GeometryNode.Height / 2.0f;
            if (NeedToFill(dnode.FillColor))
            {
                g.FillEllipse(new SolidBrush(dnode.FillColor), (float)x, (float)y, (float)drNode.Width, (float)drNode.Height);
            }

            g.DrawEllipse(pen, (float)x, (float)y, (float)drNode.Width, (float)drNode.Height);
            var w = (float)drNode.Width;
            var h = (float)drNode.Height;
            float m = Math.Max(w, h);
            float coeff = (float)1.0 - (float)(DoubleCircleOffsetRatio);
            x += coeff * m / 2.0;
            y += coeff * m / 2.0;
            g.DrawEllipse(pen, (float)x, (float)y, w - coeff * m, h - coeff * m);
        }

        internal static void DrawEdgeArrows(Graphics graphics, Microsoft.Msagl.Drawing.Edge edge, System.Drawing.Color edgeColor, Pen myPen)
        {
            ArrowAtTheEnd(graphics, edge, edgeColor, myPen);
            ArrowAtTheBeginning(graphics, edge, edgeColor, myPen);
        }

        private static void ArrowAtTheBeginning(Graphics graphics, Microsoft.Msagl.Drawing.Edge edge, System.Drawing.Color edgeColor, Pen myPen)
        {
            if (edge.GeometryEdge != null && edge.Attr.ArrowAtSource)
            {
                DrawArrowAtThebeginningWithControlPoints(graphics, edge, edgeColor, myPen);
            }
        }

        private static void DrawArrowAtThebeginningWithControlPoints(Graphics graphics, Microsoft.Msagl.Drawing.Edge edge, System.Drawing.Color edgeColor, Pen myPen)
        {
            if (edge.EdgeCurve != null)
            {
                if (edge.Attr.ArrowheadAtSource == ArrowStyle.None)
                {
                    DrawLine(graphics, myPen, edge.EdgeCurve.Start, edge.ArrowAtSourcePosition);
                }
                else
                {
                    using var sb = new SolidBrush(edgeColor);
                    DrawArrow(graphics, sb, edge.EdgeCurve.Start, edge.ArrowAtSourcePosition, edge.Attr.LineWidth, edge.Attr.ArrowheadAtSource);
                }
            }
        }

        private static void ArrowAtTheEnd(Graphics graphics, Microsoft.Msagl.Drawing.Edge edge, System.Drawing.Color edgeColor, Pen myPen)
        {
            if (edge.GeometryEdge != null && edge.Attr.ArrowAtTarget)
            {
                DrawArrowAtTheEndWithControlPoints(graphics, edge, edgeColor, myPen);
            }
        }

        private static void DrawArrowAtTheEndWithControlPoints(Graphics graphics, Microsoft.Msagl.Drawing.Edge edge, System.Drawing.Color edgeColor, Pen myPen)
        {
            if (edge.EdgeCurve != null)
            {
                if (edge.Attr.ArrowheadAtTarget == ArrowStyle.None)
                {
                    DrawLine(graphics, myPen, edge.EdgeCurve.End, edge.ArrowAtTargetPosition);
                }
                else
                {
                    using var sb = new SolidBrush(edgeColor);
                    DrawArrow(graphics, sb, edge.EdgeCurve.End, edge.ArrowAtTargetPosition, edge.Attr.LineWidth, edge.Attr.ArrowheadAtTarget);
                }
            }
        }

        private static void DrawArrow(Graphics graphics, SolidBrush sb, Microsoft.Msagl.Core.Geometry.Point end, Microsoft.Msagl.Core.Geometry.Point arrowAtTargetPosition, double lineWidth, ArrowStyle arrowheadAtTarget)
        {
            throw new NotImplementedException();
            // XXX: 未実装
        }

        private static void DrawLine(Graphics graphics, Pen myPen, Microsoft.Msagl.Core.Geometry.Point end, Microsoft.Msagl.Core.Geometry.Point arrowAtTargetPosition)
        {
            throw new NotImplementedException();
            // XXX: 未実装
        }

        private static void HandleCurve(Curve c, GraphicsPath graphicsPath)
        {
            foreach (ICurve seg in c.Segments)
            {
                if (seg is CubicBezierSegment cubic)
                {
                    graphicsPath.AddBezier(PointF(cubic.B(0)), PointF(cubic.B(1)), PointF(cubic.B(2)), PointF(cubic.B(3)));
                }
                else if (seg is LineSegment ls)
                {
                    graphicsPath.AddLine(PointF(ls.Start), PointF(ls.End));
                }
                else
                {
                    var el = seg as Ellipse;
                    AddEllipseSeg(graphicsPath, el);
                }
            }
        }
    }
}