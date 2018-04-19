using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using BezierUtility;
using UnityEditor;

namespace PCG
{
    [CustomEditor(typeof(BezierMesh))]
    [CanEditMultipleObjects]
    public class BezierMeshEditor : Editor
    {
        BezierMesh bezierMesh;
        int framesElapsed = 0;
        int framesCD = 5;
        void OnEnable()
        {
            bezierMesh = target as BezierMesh;
            framesElapsed = 0;
        }

        public override void OnInspectorGUI()
        {
            framesElapsed++;

            if (bezierMesh.scale.x == 0 || bezierMesh.scale.y == 0)
                bezierMesh.scale = new Vector2(1, 1);
            bezierMesh.echantillonage = EditorGUILayout.IntField(new GUIContent("Echantillonage"), bezierMesh.echantillonage);
            bezierMesh.material = EditorGUILayout.ObjectField(new GUIContent("Material"), bezierMesh.material, typeof(Material), false) as Material;
            bezierMesh.pointsAmount = EditorGUILayout.IntField(new GUIContent("Points amount"), bezierMesh.pointsAmount);
            bezierMesh.shape = (Shape)EditorGUILayout.EnumPopup("Shape:", bezierMesh.shape);
            bezierMesh.scale = EditorGUILayout.Vector2Field(new GUIContent("Scale"), bezierMesh.scale);
            bezierMesh.curve = EditorGUILayout.ObjectField("Curve:", bezierMesh.curve, typeof(BezierCurve), true) as BezierCurve;
            if (bezierMesh.curve == null)
                bezierMesh.curve = bezierMesh.gameObject.GetComponent<BezierCurve>();
            

            if (framesElapsed >= framesCD)
            {
                framesElapsed = 0;
                bezierMesh.Regenerate();
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif