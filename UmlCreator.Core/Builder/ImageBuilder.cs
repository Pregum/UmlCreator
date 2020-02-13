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

namespace UmlCreator.Core.Builder
{
    internal class ImageBuilder : IBuilder<Bitmap>
    {
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

            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            Bitmap bmp = new Bitmap(500, 500, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < diagram.RootNodes.Count; i++)
            {
                IRootNode root = diagram.RootNodes[i];

                AddRootNode(graph, root);
            }

            for (int i = 0; i < diagram.Edges.Count; i++)
            {
                EdgeNode edge = diagram.Edges[i];

                AddEdgeNode(graph, edge);
            }

            //Microsoft.Msagl.Miscellaneous.LayoutHelpers.CalculateLayout(graph.GeometryGraph, graph.LayoutAlgorithmSettings, new Microsoft.Msagl.Core.CancelToken());


            //graph.Write("test.png");

            // todo: ここで、レイアウトの自動調整と画像の保存ができる方法を探す。

            return bmp;
        }

        private void AddEdgeNode(Graph graph, EdgeNode edge)
        {
            Edge newEdge = graph.AddEdge(edge.SourceNodeName, edge.TargetNodeName);
        }

        private static void AddRootNode(Graph graph, IRootNode root)
        {
            Node node = graph.AddNode(root.Name);
            node.UserData = root;
            node.LabelText = root.Name;
            node.Attr.XRadius = 0.1;
            node.Attr.YRadius = 0.1;
            node.DrawNodeDelegate = DrawNode;
            node.NodeBoundaryDelegate = GetNodeBoundary;
        }

        private static ICurve GetNodeBoundary(Node node)
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

        private static bool DrawNode(Node node, object graphics)
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
