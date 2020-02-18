using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Drawing;
using System.Linq;
using UmlCreator.Core.Diagram;
using UmlCreator.Core.Param;

namespace UmlCreator.Core.Builder
{
    internal class ImageBuilder : IBuilder<Bitmap>
    {
        private static readonly int _imageSize = 500;
        private static readonly int _fontSize = 12;
        private readonly Font _measureFont = new Font("Arial", _fontSize, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);

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

            var graph = new Microsoft.Msagl.Drawing.Graph("graph");

            var bmp = new Bitmap(_imageSize, _imageSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < diagram.RootNodes.Count; i++)
            {
                IRootNode rootNode = diagram.RootNodes[i];

                AddRootNode(graph, rootNode);
            }

            for (int i = 0; i < diagram.Edges.Count; i++)
            {
                EdgeNode edge = diagram.Edges[i];

                AddEdgeNode(graph, edge);
            }

            GraphRenderer graphRenderer = new GraphRenderer(graph);

            graphRenderer.CalculateLayout();

            graphRenderer.Render(bmp);

            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "out_diagram.png");
            bmp.Save(filePath);
            Console.WriteLine($"saving {filePath} ...");

            return bmp;
        }

        private void AddEdgeNode(Graph graph, EdgeNode edge)
        {
            graph.AddEdge(edge.SourceNodeName, edge.TargetNodeName);
        }

        private void AddRootNode(Graph graph, IRootNode rootNode)
        {
            Node node = graph.AddNode(rootNode.Name);
            node.UserData = rootNode;
            node.LabelText = rootNode.Name;
            node.Attr.XRadius = 0.1;
            node.Attr.YRadius = 0.1;
            node.DrawNodeDelegate = DrawNode;
            node.NodeBoundaryDelegate = GetNodeBoundary;
        }

        private ICurve GetNodeBoundary(Node node)
        {
            IRootNode root = node.UserData as IRootNode;
            int maxLength = Math.Max(root.FullName.Length, Math.Max(root.HasDataNodes ? root.DataNodes.Max(n => n.FullName.Length) : 0
                , root.HasBehaviorNodes ? root.BehaviorNodes.Max(n => n.FullName.Length) : 0));
            int margin = 10;
            double width = margin + maxLength * 15;
            var g = Graphics.FromImage(new Bitmap(1, 1));
            var drawingSize = g.MeasureString(root.Name, _measureFont);
            //double height = (1 + root.DataNodes.Count + root.BehaviorNodes.Count) * 15;
            double height = (1 + root.DataNodes.Count + root.BehaviorNodes.Count) * drawingSize.Height;

            return CurveFactory.CreateRectangle(width, height, new Microsoft.Msagl.Core.Geometry.Point());
        }

        private bool DrawNode(Node node, object graphics)
        {
            Graphics g = (Graphics)graphics;
            IRootNode root = node.UserData as IRootNode;
            using (var m = g.Transform)
            {
                using (var saveM = m.Clone())
                {

                    using (var m2 = new System.Drawing.Drawing2D.Matrix(1, 0, 0, -1, 0, 2 * (float)(node.GeometryNode.Center.Y)))
                    {
                        m.Multiply(m2);
                    }

                    g.Transform = m;
                    // node内の描画処理の実装
                    var left = (float)(node.GeometryNode.Center.X - node.GeometryNode.Width / 2);
                    var top = (float)(node.GeometryNode.Center.Y - node.GeometryNode.Height / 2);
                    var rootNameSize = g.MeasureString(root.Name, _measureFont);
                    var actualHeight = rootNameSize.Height * (1 + root.DataNodes.Count + root.BehaviorNodes.Count);
                    // 枠の描画
                    g.DrawRectangle(Pens.Black, left, top, (float)node.GeometryNode.Width, actualHeight);

                    // クラス名の描画
                    g.DrawString(root.Name
                        , _measureFont
                        , Brushes.Black
                        , new PointF((float)(node.GeometryNode.Center.X - rootNameSize.Width / 2)
                        , top));

                    // クラス名と属性の区切り線の描画
                    g.DrawLine(Pens.Black, left, top + rootNameSize.Height, (float)node.BoundingBox.Right, top + rootNameSize.Height);


                    // 属性の描画
                    if (root.HasDataNodes)
                    {
                        for (int i = 0; i < root.DataNodes.Count; i++)
                        {
                            Console.WriteLine($"{root.Name}.DataNodes drawing... ");
                            INode dataNode = root.DataNodes[i];
                            g.DrawString(dataNode.FullName, _measureFont, Brushes.Black, new PointF(left, top + (1 + i) * rootNameSize.Height));
                        }
                    }

                    // 操作の描画
                    if (root.HasBehaviorNodes)
                    {
                        for (int i = 0; i < root.BehaviorNodes.Count; i++)
                        {
                            Console.WriteLine($"{root.Name}.BehaviorNodes drawing... ");
                            IOperationalNode opNode = root.BehaviorNodes[i];
                            g.DrawString(opNode.FullName, _measureFont, Brushes.Black, new PointF(left, top + (1 + root.DataNodes.Count + i) * rootNameSize.Height));
                        }
                    }

                    g.Transform = saveM;

                    return true;
                }
            }
        }
    }
}
