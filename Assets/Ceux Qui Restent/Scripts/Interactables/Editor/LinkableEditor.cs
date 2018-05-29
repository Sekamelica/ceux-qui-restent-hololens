using UnityEngine;
using Utility;
using CeuxQuiRestent.Interactables;
using CeuxQuiRestent.Links;
using CeuxQuiRestent.Audio;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Tools
{
    [CustomEditor(typeof(Linkable))]
    public class LinkableEditor : Editor
    {
        Linkable linkable;
        private Transform handleTransform;
        private Quaternion handleRotation;

        void OnEnable()
        {
            linkable = target as Linkable;
        }

        private void OnSceneGUI()
        {
            handleTransform = linkable.transform;
            handleRotation = UnityEditor.Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
            ShowPoint();
        }

        private void ShowPoint()
        {
            Vector3 point = handleTransform.TransformPoint(linkable.linkStartOffset);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                point -= linkable.transform.position;
                Undo.RecordObject(linkable, "Move LinkStartPoint");
                EditorUtility.SetDirty(linkable);
                linkable.linkStartOffset = point;//handleTransform.InverseTransformPoint(point) - linkable.transform.position;
                //linkable.linkStartOffset = point - linkable.transform.position;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Logic fields", EditorStyles.boldLabel);

            // Set Link Start Point
            //linkable.linkStartPosition = EditorGUILayout.Vector3Field("Link Start Point", linkable.linkStartPosition);
            
            linkable.linkStartOffset = EditorGUILayout.Vector3Field("Link Start Offset", linkable.linkStartOffset);
            linkable.linkStartPosition = linkable.transform.position + linkable.linkStartOffset;
            //linkable.linkStartPosition = EditorGUILayout.Vector3Field("Link Start Position", linkable.linkStartPosition);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset offset"))
                linkable.linkStartOffset = Vector3.zero;
            if (GUILayout.Button("Set offset on Capsule center"))
            {
                CapsuleCollider capsCollider = linkable.gameObject.GetComponent<CapsuleCollider>();
                if (capsCollider != null)
                    linkable.linkStartOffset = capsCollider.center;
                else
                    Debug.Log("[Linkable Editor] - Capsule Collider not found !");
            }
            EditorGUILayout.EndHorizontal();            

            // Set Actions
            ActionExecuter newActionsToDo = EditorGUILayout.ObjectField("Actions to do", linkable.actionsToDo, typeof(ActionExecuter), true) as ActionExecuter;
            if(newActionsToDo != linkable.actionsToDo)
            {
                linkable.actionsToDo = newActionsToDo;
                if (linkable.pair != null)
                    linkable.pair.actionsToDo = newActionsToDo;
            }
            
            // Set New Pair
            Linkable newPair = EditorGUILayout.ObjectField("Pair", linkable.pair, typeof(Linkable), true) as Linkable;
            if (newPair != linkable.pair)
            {
                // Destroy potential old pairs
                if (linkable.pair != null)
                {
                    Debug.DrawLine(linkable.gameObject.transform.position, linkable.pair.gameObject.transform.position, Color.red, 0.4f);
                    linkable.pair.pair = null;
                }

                if (newPair != null)
                {
                    if (newPair.pair != null)
                    {
                        Debug.DrawLine(newPair.gameObject.transform.position, newPair.pair.gameObject.transform.position, Color.red, 0.4f);
                        newPair.pair.pair = null;
                    }

                    // Create new pairs
                    newPair.pair = linkable;
                    linkable.pair = newPair;
                    linkable.pair.actionsToDo = linkable.actionsToDo;
                }
            }

            linkable.energy = EditorGUILayout.ObjectField("Energy", linkable.energy, typeof(Energy), true) as Energy;

            if (linkable.pair != null)
            {
                linkable.pair.actionsToDo = linkable.actionsToDo;
                if (linkable.pair.pair != null)
                    linkable.pair.pair = null;
                linkable.pair.pair = linkable;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visual fields", EditorStyles.boldLabel);

            MeshRenderer newMeshRenderer = EditorGUILayout.ObjectField("Model", linkable.model, typeof(MeshRenderer), true) as MeshRenderer;
            if (linkable.model != newMeshRenderer)
            {
                linkable.ChangeModel(newMeshRenderer);
            }
            Material newMaterial = EditorGUILayout.ObjectField("Material Normal", linkable.materialNormal, typeof(Material), true) as Material;
            if (newMaterial != linkable.materialNormal)
                linkable.ChangeMaterial(newMaterial);

            linkable.materialHover = EditorGUILayout.ObjectField("Material Hover", linkable.materialHover, typeof(Material), true) as Material;

            linkable.appearDisappearAnimationTime = EditorGUILayout.FloatField("Animation time", linkable.appearDisappearAnimationTime);

            serializedObject.ApplyModifiedProperties(); 
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
