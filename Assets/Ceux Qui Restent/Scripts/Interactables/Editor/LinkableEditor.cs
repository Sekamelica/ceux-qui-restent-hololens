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
            // Set Actions
            ActionExecuter newActionsToDo = EditorGUILayout.ObjectField("Actions to do", linkable.actionsToDo, typeof(ActionExecuter), true) as ActionExecuter;
            if(newActionsToDo != linkable.actionsToDo)
            {
                linkable.actionsToDo = newActionsToDo;
                if (linkable.pair != null)
                    linkable.pair.actionsToDo = newActionsToDo;
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Name", linkable.gameObject.name);
            EditorGUI.EndDisabledGroup();
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
            
            serializedObject.ApplyModifiedProperties(); 
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
