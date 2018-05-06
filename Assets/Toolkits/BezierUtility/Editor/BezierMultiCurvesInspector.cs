using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

namespace BezierUtility
{
    [CustomEditor(typeof(BezierMultiCurves))]
    public class BezierMultiCurvesInspector : Editor
    {
        private bool displayPoints = true;
        private bool displayModifier = true;
        private bool displayExtremities = true;
        private BezierMultiCurves curves;
        private Transform handleTransform;
        private Quaternion handleRotation;

        //	private void OnSceneGUI () {
        //		curve = target as BezierCurve;
        //		handleTransform = curve.transform;
        //		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        //
        //		Vector3 p0 = ShowPoint(0);
        //		Vector3 p1 = ShowPoint(1);
        //		Vector3 p2 = ShowPoint(2);
        //		Vector3 p3 = ShowPoint(3);
        //
        //		Handles.color = Color.gray;
        //		Handles.DrawLine(p0, p1);
        //		Handles.DrawLine(p2, p3);
        //
        //		Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        //	}

        private const int lineSteps = 10;
        private const float directionScale = 0.5f;

        private void OnSceneGUI()
        {
            curves = target as BezierMultiCurves;
            handleTransform = curves.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            if (curves.points.Length < 4)
                curves.Reset();

            List<Vector3> curvesPoints = new List<Vector3>();
            for (int i = 0; i < curves.points.Length; i++)
            {
                if ((i % 4) - 3 == 0)
                {
                    int c = (i - 3) / 4;
                    if(c % 2 == 0)
                    {
                        if (i + 4 < curves.points.Length)
                            curves.points[i + 4] = curves.points[i];
                    }
                    else
                    {
                        if (i + 1 < curves.points.Length)
                            curves.points[i + 1] = curves.points[i-3];
                    }
                    
                }
                if ((i % 7 != 0) && (i % 8 != 0))
                    curvesPoints.Add(ShowPoint(i));
            }

            for(int p = 0; p < curvesPoints.Count; p += 4)
            {
                if(p+3 < curvesPoints.Count)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(curvesPoints[p], curvesPoints[p + 1]);
                    Handles.color = Color.blue;
                    Handles.DrawLine(curvesPoints[p + 2], curvesPoints[p + 3]);
                    Handles.DrawBezier(curvesPoints[p], curvesPoints[p + 3], curvesPoints[p + 1], curvesPoints[p + 2], Color.yellow, null, 2f);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            displayPoints = EditorGUILayout.Toggle(new GUIContent("Display Points"), displayPoints);
            displayExtremities = EditorGUILayout.Toggle(new GUIContent("Display Extremities"), displayExtremities);
            displayModifier = EditorGUILayout.Toggle(new GUIContent("Display Modifiers"), displayModifier);
            serializedObject.ApplyModifiedProperties();
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(curves.points[index]);
            EditorGUI.BeginChangeCheck();
            if (displayPoints)
            {
                if( ((index + 3) % 4 == 0) || ((index + 2) % 4 == 0) )
                {
                    if (displayModifier)
                        point = Handles.DoPositionHandle(point, handleRotation);
                }
                else if (displayExtremities)
                    point = Handles.DoPositionHandle(point, handleRotation);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curves, "Move Point");
                EditorUtility.SetDirty(curves);
                curves.points[index] = handleTransform.InverseTransformPoint(point);
            }
            return point;
        }
    }
}
#endif