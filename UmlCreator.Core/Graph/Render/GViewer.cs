using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace UmlCreator.Core.Graph.Render
{
    internal class GViewer : IViewer
    {
        internal static double Dpi = GetDotsPerInch();
        internal static double dpix;
        internal static double dpiy;

        public double CurrentScale => throw new NotImplementedException();

        // XXX: IViewerインターフェース実装途中
        public bool NeedToCalculateLayout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IViewerObject ObjectUnderMouseCursor => throw new NotImplementedException();

        public ModifierKeys ModifierKeys => throw new NotImplementedException();

        public IEnumerable<IViewerObject> Entities => throw new NotImplementedException();

        public double DpiX => throw new NotImplementedException();

        public double DpiY => throw new NotImplementedException();

        public double LineThicknessForEditing => throw new NotImplementedException();

        public bool LayoutEditingEnabled => throw new NotImplementedException();

        public bool InsertingEdge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double UnderlyingPolylineCircleRadius => throw new NotImplementedException();

        public Microsoft.Msagl.Drawing.Graph Graph { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IViewerGraph ViewerGraph => throw new NotImplementedException();

        public double ArrowheadLength => throw new NotImplementedException();

        public PlaneTransformation Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<EventArgs> ViewChangeEvent;
        public event EventHandler<MsaglMouseEventArgs> MouseDown;
        public event EventHandler<MsaglMouseEventArgs> MouseMove;
        public event EventHandler<MsaglMouseEventArgs> MouseUp;
        public event EventHandler<ObjectUnderMouseCursorChangedEventArgs> ObjectUnderMouseCursorChanged;
        public event EventHandler GraphChanged;

        private static double GetDotsPerInch()
        {
            Graphics g = Graphics.FromImage(new Bitmap(""));
            return Math.Max(dpix = g.DpiX, dpiy = g.DpiY);
        }

        public void AddEdge(IViewerEdge edge, bool registerForUndo)
        {
            throw new NotImplementedException();
        }

        public void AddNode(IViewerNode node, bool registerForUndo)
        {
            throw new NotImplementedException();
        }

        public IViewerEdge CreateEdgeWithGivenGeometry(Microsoft.Msagl.Drawing.Edge drawingEdge)
        {
            throw new NotImplementedException();
        }

        public IViewerNode CreateIViewerNode(Microsoft.Msagl.Drawing.Node drawingNode, Microsoft.Msagl.Core.Geometry.Point center, object visualElement)
        {
            throw new NotImplementedException();
        }

        public IViewerNode CreateIViewerNode(Microsoft.Msagl.Drawing.Node drawingNode)
        {
            throw new NotImplementedException();
        }

        public void DrawRubberEdge(EdgeGeometry edgeGeometry)
        {
            throw new NotImplementedException();
        }

        public void DrawRubberLine(MsaglMouseEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void DrawRubberLine(Microsoft.Msagl.Core.Geometry.Point point)
        {
            throw new NotImplementedException();
        }

        public void Invalidate(IViewerObject objectToInvalidate)
        {
            throw new NotImplementedException();
        }

        public void Invalidate()
        {
            throw new NotImplementedException();
        }

        public void OnDragEnd(IEnumerable<IViewerObject> changedObjects)
        {
            throw new NotImplementedException();
        }

        public void PopupMenus(params Tuple<string, VoidDelegate>[] menuItems)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdge(IViewerEdge edge, bool registerForUndo)
        {
            throw new NotImplementedException();
        }

        public void RemoveNode(IViewerNode node, bool registerForUndo)
        {
            throw new NotImplementedException();
        }

        public void RemoveSourcePortEdgeRouting()
        {
            throw new NotImplementedException();
        }

        public void RemoveTargetPortEdgeRouting()
        {
            throw new NotImplementedException();
        }

        public IViewerEdge RouteEdge(Microsoft.Msagl.Drawing.Edge drawingEdge)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Msagl.Core.Geometry.Point ScreenToSource(MsaglMouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SetSourcePortForEdgeRouting(Microsoft.Msagl.Core.Geometry.Point portLocation)
        {
            throw new NotImplementedException();
        }

        public void SetTargetPortForEdgeRouting(Microsoft.Msagl.Core.Geometry.Point portLocation)
        {
            throw new NotImplementedException();
        }

        public void StartDrawingRubberLine(Microsoft.Msagl.Core.Geometry.Point startingPoint)
        {
            throw new NotImplementedException();
        }

        public void StopDrawingRubberEdge()
        {
            throw new NotImplementedException();
        }

        public void StopDrawingRubberLine()
        {
            throw new NotImplementedException();
        }
    }
}