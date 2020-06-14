namespace Dreamteck.Splines.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public class DreamteckSplinesEditor : SplineEditor
    {
        public SplineComputer spline = null;

        public DreamteckSplinesEditor(SplineComputer spline, string name) : base (spline.transform, name)
        {
            this.spline = spline;
            evaluate = spline.Evaluate;
            evaluateAtPoint = spline.Evaluate;
            evaluatePosition = spline.EvaluatePosition;
            calculateLength = spline.CalculateLength;
            travel = spline.Travel;
            undoHandler = HandleUndo;
            mainModule.onBeforeDeleteSelectedPoints += OnBeforeDeleteSelectedPoints;
            mainModule.onDuplicatePoint += OnDuplicatePoint;
            if (spline.isNewlyCreated)
            {
                if (SplinePrefs.startInCreationMode)
                {
                    open = true;
                    ToggleModule(0);
                }
                spline.isNewlyCreated = false;
            }
            Refresh();
        }

        private void OnDuplicatePoint(int[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                spline.ShiftNodes(points[i], spline.pointCount - 1, 1);
            }
        }

        private void OnBeforeDeleteSelectedPoints()
        {
            string nodeString = "";
            List <Node> deleteNodes = new List<Node>();
            for (int i = 0; i < selectedPoints.Count; i++)
            {
                spline.DisconnectNode(selectedPoints[i]);
                Node node = spline.GetNode(selectedPoints[i]);
                if (node != null && node.GetConnections().Length == 0)
                {
                    deleteNodes.Add(node);
                    if (nodeString != "") nodeString += ", ";
                    string trimmed = node.name.Trim();
                    if (nodeString.Length + trimmed.Length > 80) nodeString += "...";
                    else nodeString += node.name.Trim();
                }
            }

            if (deleteNodes.Count > 0)
            {
                string message = "The following nodes:\r\n" + nodeString + "\r\n were only connected to the currently selected points. Would you like to remove them from the scene?";
                if (EditorUtility.DisplayDialog("Remove nodes?", message, "Yes", "No"))
                {
                    for (int i = 0; i < deleteNodes.Count; i++) Undo.DestroyObjectImmediate(deleteNodes[i].gameObject);
                }
            }

            int min = spline.pointCount - 1;
            for (int i = 0; i < selectedPoints.Count; i++)
            {
                if (selectedPoints[i] < min) min = selectedPoints[i];
            }
            for (int i = min+1; i < spline.pointCount; i++)
            {
                Node node = spline.GetNode(i);
                if(node != null)
                {
                    int pointsDeletedBefore = 0;
                    for (int j = 0; j < selectedPoints.Count; j++)
                    {
                        if (selectedPoints[j] >= min) pointsDeletedBefore++;
                    }
                    spline.ShiftNodes(i, spline.pointCount-1, -pointsDeletedBefore);
                }
            }
        }


        protected override void OnModuleList(List<PointModule> list)
        {
            list.Add(new DSCreatePointModule(this));
            list.Add(new DeletePointModule(this));
            list.Add(new PointMoveModule(this));
            list.Add(new PointRotateModule(this));
            list.Add(new PointScaleModule(this));
            list.Add(new PointNormalModule(this));
            list.Add(new PointMirrorModule(this));
#if DREAMTECK_SPLINES
            list.Add(new PrimitivesModule(this));
#endif
        }

        public override void Destroy()
        {
            base.Destroy();
            UpdateSpline();
        }

        public override void DrawInspector()
        {
            Refresh();
            base.DrawInspector();
            UpdateSpline();
        }

        public override void DrawScene()
        {
            Refresh();
            base.DrawScene();
            UpdateSpline();
        }

        public override void BeforeSceneGUI(SceneView current)
        {
            Refresh();
            base.BeforeSceneGUI(current);
            UpdateSpline();
        }

        public void Refresh()
        {
            points = spline.GetPoints();
            isClosed = spline.isClosed;
            splineType = spline.type;
            sampleRate = spline.sampleRate;
            is2D = spline.is2D;
            color = spline.editorPathColor;
        }

        public void UpdateSpline()
        {
            if (spline == null) return;
            if (!isClosed && spline.isClosed) spline.Break();
            else if(spline.isClosed && points.Length < 4)
            {
                spline.Break();
                isClosed = false;
            }
            spline.SetPoints(points);
            if (isClosed && !spline.isClosed) spline.Close();
            spline.type = splineType;
            spline.sampleRate = sampleRate;
            spline.is2D = is2D;
            spline.EditorUpdateConnectedNodes();
        }

        void HandleUndo(string title)
        {
            Undo.RecordObject(spline, title);
        }
    }
}
