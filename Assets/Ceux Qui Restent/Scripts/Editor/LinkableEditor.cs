using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent
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
            DoActionsAfterXSeconds newActionsToDo = EditorGUILayout.ObjectField("Actions to do", linkable.actionsToDo, typeof(DoActionsAfterXSeconds), true) as DoActionsAfterXSeconds;
            if(newActionsToDo != linkable.actionsToDo)
            {
                linkable.actionsToDo = newActionsToDo;
                if (linkable.pair != null)
                    linkable.pair.actionsToDo = newActionsToDo;
            }

            linkable.energyMaximumIncrease = EditorGUILayout.FloatField("Energy Increase", linkable.energyMaximumIncrease);

            Linkable newPair = EditorGUILayout.ObjectField("Pair", linkable.pair, typeof(Linkable), true) as Linkable;
            if (newPair != linkable.pair)
            {
                // Destroy potential old pairs
                if (linkable.pair != null)
                {
                    linkable.EditorDrawLink(Color.red, 0.4f);
                    linkable.pair.pair = null;
                }
                if (newPair.pair != null)
                {
                    newPair.EditorDrawLink(Color.red, 0.4f);
                    newPair.pair.pair = null;
                }
                // Create new pairs
                newPair.pair = linkable;
                linkable.pair = newPair;
            }
            
            Linkable[] linkables = FindObjectsOfType<Linkable>();
            foreach (Linkable lkbl in linkables)
                if (lkbl != linkable)
                    lkbl.EditorDrawLink(Color.yellow, 0.2f);
            linkable.EditorDrawLink(Color.green, 0.2f);

            serializedObject.ApplyModifiedProperties(); 
            EditorUtility.SetDirty(target);
        }

        void OnDrawGizmos()
        {
            if (linkable.pair != null)
                Handles.Label(Vector3.Lerp(linkable.gameObject.transform.position, linkable.pair.gameObject.transform.position, 0.5f), Vector3.Distance(linkable.gameObject.transform.position, linkable.pair.gameObject.transform.position).ToString());
        }
    }
}
#endif
