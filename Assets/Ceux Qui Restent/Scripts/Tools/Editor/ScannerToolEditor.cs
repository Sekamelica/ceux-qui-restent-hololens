#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CeuxQuiRestent.Tools
{
    public class ScannerToolEditor : MonoBehaviour
    {
        [CustomEditor(typeof(ScannerTool))]
        public class LinkableEditor : Editor
        {
            private ScannerTool scannerTool;
            private Transform handleTransform;
            private Quaternion handleRotation;

            void OnEnable()
            {
                scannerTool = target as ScannerTool;
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                EditorGUILayout.Space();

                float scanCompletionPercent = Mathf.Clamp01(scannerTool.GetCurrentTime() / scannerTool.scanTime);
                EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), scanCompletionPercent, ((int)(scanCompletionPercent * 100)).ToString() + "%");
                if (!scannerTool.IsScanning())
                {
                    if (GUILayout.Button("Start Scan"))
                        scannerTool.StartScan();
                    if (scannerTool.IsScanDone())
                        if (GUILayout.Button("Reset Scan"))
                            scannerTool.ResetScan();
                }

                EditorGUILayout.Space();

                if (GUILayout.Button("Start Analysis"))
                {

                }

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            private void OnSceneGUI()
            {
                handleTransform = scannerTool.transform;
                handleRotation = UnityEditor.Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
                ShowPoint();
            }

            private void ShowPoint()
            {
                Vector3 point = handleTransform.TransformPoint(scannerTool.posOffset);
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    point -= scannerTool.transform.position;
                    Undo.RecordObject(scannerTool, "Move PlayerPosition");
                    EditorUtility.SetDirty(scannerTool);
                    scannerTool.posOffset = point;
                }
            }
        }
    }
}
#endif
