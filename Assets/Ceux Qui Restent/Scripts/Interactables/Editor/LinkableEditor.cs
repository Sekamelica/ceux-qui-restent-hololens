using UnityEngine;
using Utility;
using CeuxQuiRestent.Interactables;
using CeuxQuiRestent.Links;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Tools
{
    [CustomEditor(typeof(Linkable))]
    public class LinkableEditor : Editor
    {
        Linkable linkable;

        void OnEnable()
        {
            linkable = target as Linkable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Logic fields", EditorStyles.boldLabel);

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
            
            linkable.ChangeModel(EditorGUILayout.ObjectField("Model", linkable.model, typeof(MeshRenderer), true) as MeshRenderer);
            linkable.ChangeMaterial(EditorGUILayout.ObjectField("Material", linkable.material, typeof(Material), true) as Material);
            linkable.appearDisappearAnimationTime = EditorGUILayout.FloatField("Animation time", linkable.appearDisappearAnimationTime);

            serializedObject.ApplyModifiedProperties(); 
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
