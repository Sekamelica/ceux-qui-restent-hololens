using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using BezierUtility;
using UnityEditor;

namespace PCG
{
    [CustomEditor(typeof(BezierMeshMultiCurves))]
    [CanEditMultipleObjects]
    public class BezierMeshMultiCurvesEditor : Editor
    {
        BezierMeshMultiCurves bezierMesh;
        void OnEnable()
        {
            bezierMesh = target as BezierMeshMultiCurves;
        }

        public override void OnInspectorGUI()
        {
            if (bezierMesh.scale.x == 0 || bezierMesh.scale.y == 0)
                bezierMesh.scale = new Vector2(1, 1);
            bezierMesh.echantillonage = EditorGUILayout.IntField(new GUIContent("Echantillonage"), bezierMesh.echantillonage);
            bezierMesh.material = EditorGUILayout.ObjectField(new GUIContent("Material"), bezierMesh.material, typeof(Material), false) as Material;
            bezierMesh.pointsAmount = EditorGUILayout.IntField(new GUIContent("Points amount"), bezierMesh.pointsAmount);
            bezierMesh.shape = (Shape)EditorGUILayout.EnumPopup("Shape:", bezierMesh.shape);
            bezierMesh.scale = EditorGUILayout.Vector2Field(new GUIContent("Scale"), bezierMesh.scale);
            bezierMesh.curve = EditorGUILayout.ObjectField("Curve:", bezierMesh.curve, typeof(BezierMultiCurves), true) as BezierMultiCurves;
            if (bezierMesh.curve == null)
                bezierMesh.curve = bezierMesh.gameObject.GetComponent<BezierMultiCurves>();
            
            if(GUILayout.Button("Generate"))
                bezierMesh.Regenerate();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif