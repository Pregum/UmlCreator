using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Param;

using GeoGraph = Microsoft.Msagl.Core.Layout.GeometryGraph;
using GeoNode = Microsoft.Msagl.Core.Layout.Node;
using GeoEdge = Microsoft.Msagl.Core.Layout.Edge;
using Microsoft.Msagl.Miscellaneous;

namespace UmlCreator.Core.Builder
{
    internal class ImageBuilder : IBuilder<Bitmap>
    {
        private GeoGraph geoGraph;

        /// <summary>
        /// Bitmapでクラス図を生成します。
        /// </summary>
        /// <param name="diagram">根本のノード</param>
        /// <returns>クラス図</returns>
        Bitmap IBuilder<Bitmap>.MakeDiagram(DiagramParam diagram)
        {
            if (diagram is null)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            //Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            //Microsoft.Msagl.Drawing.Graph graph = CreateAndLayoutDrawingGraph(diagram);

            geoGraph = CreateAndLayoutGraph(diagram);
            Bitmap bmp = new Bitmap(500, 500, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            DrawGraph(g, new RectangleF(0, 0, bmp.Width, bmp.Height));

            // todo: ここで、レイアウトの自動調整と画像の保存ができる方法を探す。
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "out.png");
            Console.WriteLine($"{path} に保存中...");
            bmp.Save(path);

            return bmp;
        }

        private Graph CreateAndLayoutDrawingGraph(DiagramParam diagram)
        {
            double w = 30;
            double h = 20;

            Graph graph = new Graph();
            GeoGraph gGraph = new GeoGraph();
            for (int i = 0; i < diagram.RootNodes.Count; i++)
            {
                GeoNode geoNode = new GeoNode(CurveFactory.CreateRectangle(w, h, new Microsoft.Msagl.Core.Geometry.Point()), diagram.RootNodes[i].Name);
                gGraph.Nodes.Add(geoNode);
                AddRootNode(graph, diagram.RootNodes[i], geoNode);
            }

            for (int i = 0; i < diagram.Edges.Count; i++)
            {
                GeoEdge geoEdge = new GeoEdge(gGraph.FindNodeByUserData(diagram.Edges[i].SourceNodeName), gGraph.FindNodeByUserData(diagram.Edges[i].TargetNodeName));
                gGraph.Edges.Add(geoEdge);
                AddEdgeNode(graph, diagram.Edges[i], geoEdge);
            }

            var settings = new Microsoft.Msagl.Layout.Incremental.FastIncrementalLayoutSettings();
            graph.LayoutAlgorithmSettings = settings;
            return graph;
        }

        #region ref MSAGL.GraphLayout.UsingMdsLayoutSample : https://github.com/microsoft/automatic-graph-layout/tree/master/GraphLayout/Samples/UsingMDSLayoutSample
        private GeoGraph CreateAndLayoutGraph(DiagramParam diagram)
        {
            // todo: 後で変更できるようにする。
            double w = 30;
            double h = 20;

            GeoGraph graph = new GeoGraph();
            for (int i = 0; i < diagram.RootNodes.Count; i++)
            {
                GeoNode node = new GeoNode(CurveFactory.CreateRectangle(w, h, new Microsoft.Msagl.Core.Geometry.Point()), diagram.RootNodes[i].Name);
                graph.Nodes.Add(node);
            }

            for (int i = 0; i < diagram.Edges.Count; i++)
            {
                GeoEdge edge = new GeoEdge(graph.FindNodeByUserData(diagram.Edges[i].SourceNodeName), graph.FindNodeByUserData(diagram.Edges[i].TargetNodeName));
                graph.Edges.Add(edge);
            }

            var settings = new Microsoft.Msagl.Layout.Incremental.FastIncrementalLayoutSettings();
            LayoutHelpers.CalculateLayout(graph, settings, null);

            return graph;
        }

        private void DrawGraph(Graphics g, RectangleF drawingRectangleF)
        {
            SetGraphicsTransform(g, drawingRectangleF);
            Pen pen = new Pen(Brushes.Black);
            DrawNodes(pen, g);
            DrawEdges(pen, g);

        }

        private void DrawEdges(Pen pen, Graphics g)
        {
            foreach (GeoEdge edge in geoGraph.Edges)
            {
                DrawEdge(edge, pen, g);
            }
        }

        private void DrawEdge(GeoEdge edge, Pen pen, Graphics g)
        {
            ICurve curve = edge.Curve;
            Curve c = curve as Curve;
            if (c != null)
            {
                foreach (ICurve cur in c.Segments)
                {
                    LineSegment l = cur as LineSegment;
                    if (l != null)
                    {
                        g.DrawLine(pen, MsaglPointToDrawingPoint(l.Start), MsaglPointToDrawingPoint(l.End));
                    }

                    CubicBezierSegment cs = cur as CubicBezierSegment;
                    if (cs != null)
                    {
                        g.DrawBezier(pen, MsaglPointToDrawingPoint(cs.B(0)), MsaglPointToDrawingPoint(cs.B(1)), MsaglPointToDrawingPoint(cs.B(2)), MsaglPointToDrawingPoint(cs.B(3)));
                    }
                }

                if (edge.ArrowheadAtSource)
                {
                    DrawArrow(edge, pen, g, edge.Curve.Start, edge.EdgeGeometry.SourceArrowhead.TipPosition);
                }

                if (edge.ArrowheadAtTarget)
                {
                    DrawArrow(edge, pen, g, edge.Curve.End, edge.EdgeGeometry.TargetArrowhead.TipPosition);
                }
            }
            else
            {
                var l = curve as LineSegment;
                if (l != null)
                {
                    g.DrawLine(pen, MsaglPointToDrawingPoint(l.Start), MsaglPointToDrawingPoint(l.End));
                }
            }
        }

        private void DrawArrow(GeoEdge edge, Pen pen, Graphics g, Microsoft.Msagl.Core.Geometry.Point start, Microsoft.Msagl.Core.Geometry.Point end)
        {
            PointF[] points;
            float arrowAngle = 30;
            Microsoft.Msagl.Core.Geometry.Point dir = end - start;
            Microsoft.Msagl.Core.Geometry.Point h = dir;
            dir /= dir.Length;

            Microsoft.Msagl.Core.Geometry.Point s = new Microsoft.Msagl.Core.Geometry.Point(-dir.Y, dir.X);
            s *= h.Length * ((float)Math.Tan(arrowAngle * 0.5f * (Math.PI / 180.0)));
            points = new PointF[] { MsaglPointToDrawingPoint(start + s), MsaglPointToDrawingPoint(end), MsaglPointToDrawingPoint(start - s) };
            g.FillPolygon(pen.Brush, points);
        }

        private System.Drawing.Point MsaglPointToDrawingPoint(Microsoft.Msagl.Core.Geometry.Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }

        private void DrawNodes(Pen pen, Graphics g)
        {
            foreach (GeoNode node in geoGraph.Nodes)
            {
                DrawNode(node, pen, g);
            }
        }

        private void DrawNode(GeoNode node, Pen pen, Graphics g)
        {
            ICurve curve = node.BoundaryCurve;
            Ellipse el = curve as Ellipse;
            if (el != null)
            {
                Microsoft.Msagl.Core.Geometry.Rectangle box = el.BoundingBox;
                g.DrawEllipse(pen, new RectangleF((float)box.Left, (float)box.Bottom, (float)box.Width, (float)box.Height));
            }
            else
            {
                Curve cur = curve as Curve;
                foreach (ICurve seg in cur.Segments)
                {
                    LineSegment l = seg as LineSegment;
                    if (l != null)
                    {
                        g.DrawLine(pen, MsaglPointToDrawingPoint(l.Start), MsaglPointToDrawingPoint(l.End));
                    }
                }
            }
        }

        private void SetGraphicsTransform(Graphics graphics, RectangleF r)
        {
            var gr = geoGraph.BoundingBox;
            if (r.Height > 1 && r.Width > 1)
            {
                float scale = Math.Min(r.Width / (float)gr.Width, r.Height / (float)gr.Height);
                float g0 = (float)(gr.Left + gr.Right) / 2;
                float g1 = (float)(gr.Top + gr.Bottom) / 2;

                float c0 = (r.Left + r.Height) / 2;
                float c1 = (r.Top + r.Bottom) / 2;
                float dx = c0 - scale * g0;
                float dy = c1 + scale * g1;
                graphics.Transform = new System.Drawing.Drawing2D.Matrix(scale, 0, 0, -scale, dx, dy);
            }
        }

        #endregion


        private void AddEdgeNode(Graph graph, EdgeNode edge, GeoEdge geoEdge)
        {
            Microsoft.Msagl.Drawing.Edge newEdge = graph.AddEdge(edge.SourceNodeName, edge.TargetNodeName);
            newEdge.GeometryEdge = geoEdge;
        }

        private Node AddRootNode(Graph graph, IRootNode root, GeoNode geoNode)
        {
            Microsoft.Msagl.Drawing.Node node = graph.AddNode(root.Name);
            node.UserData = root;
            node.LabelText = root.Name;
            node.Attr.XRadius = 0.1;
            node.Attr.YRadius = 0.1;
            node.DrawNodeDelegate = DrawNodeDelegate;
            node.NodeBoundaryDelegate = GetNodeBoundaryDelegate;
            node.GeometryNode = geoNode;
            return node;
        }


        private static ICurve GetNodeBoundaryDelegate(Microsoft.Msagl.Drawing.Node node)
        {
            IRootNode root = node.UserData as IRootNode;
            int maxLength = Math.Max(root.FullName.Length,
                Math.Max(root.HasDataNodes ? root.DataNodes.Max(n => n.FullName.Length) : 0
                    , root.HasBehaviorNodes ? root.BehaviorNodes.Max(n => n.FullName.Length) : 0));
            int margin = 10;
            double width = margin + maxLength * 15;
            double height = (1 + root.DataNodes.Count + root.BehaviorNodes.Count) * 15;

            return CurveFactory.CreateRectangle(width, height, new Microsoft.Msagl.Core.Geometry.Point());
        }

        private static bool DrawNodeDelegate(Microsoft.Msagl.Drawing.Node node, object graphics)
        {
            Graphics g = (Graphics)graphics;
            IRootNode root = node.UserData as IRootNode;
            using var m = g.Transform;
            using var saveM = m.Clone();

            using var m2 = new System.Drawing.Drawing2D.Matrix(1, 0, 0, -1, 0, 2 * (float)(node.GeometryNode.Center.Y - node.GeometryNode.Width / 2));
            m.Multiply(m2);

            g.Transform = m;
            // node内の描画処理の実装
            using var font = new Font("Arial", 12, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            // クラス名の描画
            g.DrawString(root.Name
                , font
                , Brushes.Black
                , new PointF((float)(node.GeometryNode.Center.X - root.Name.Length / 2 * 10)
                , (float)(node.GeometryNode.Center.Y)));

            if (root.HasDataNodes)
            {
                for (int i = 0; i < root.DataNodes.Count; i++)
                {
                    Console.WriteLine($"{root.Name}.DataNodes drawing... ");
                    INode dataNode = root.DataNodes[i];
                    g.DrawString(dataNode.FullName
                        , font
                        , Brushes.Black
                        , new PointF((float)(node.GeometryNode.Center.X - node.GeometryNode.Width / 2)
                        , (float)(node.GeometryNode.Center.Y - node.GeometryNode.Height / 2)));
                }
            }

            if (root.HasBehaviorNodes)
            {
                for (int i = 0; i < root.BehaviorNodes.Count; i++)
                {
                    Console.WriteLine($" {root.Name}.BehaviorNodes drawing...");
                    IOperationalNode opNode = root.BehaviorNodes[i];
                    g.DrawString(opNode.FullName
                        , font
                        , Brushes.Black
                        , new PointF((float)(node.GeometryNode.Center.X - node.GeometryNode.Width / 2)
                        , (float)(node.GeometryNode.Center.Y - node.GeometryNode.Height / 2)));
                }
            }

            g.Transform = saveM;

            return true;
        }
    }
}
