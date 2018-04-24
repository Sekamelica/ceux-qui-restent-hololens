using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            //DrawDefaultInspector();
            linkable.energyMaximumIncrease = EditorGUILayout.FloatField("Energy Increase", linkable.energyMaximumIncrease);
            Linkable newPair = EditorGUILayout.ObjectField("Pair", linkable.GetPair(), typeof(Linkable), true) as Linkable;
            if (newPair != linkable.GetPair())
            {
                // Destroy potential old pairs
                if (linkable.GetPair() != null)
                {
                    linkable.DrawLinkEditor(Color.red, 0.4f);
                    (linkable.GetPair()).SetPair(null);
                }
                if (newPair.GetPair() != null)
                {
                    newPair.DrawLinkEditor(Color.red, 0.4f);
                    (newPair.GetPair()).SetPair(null);
                }
                // Create new pairs
                newPair.SetPair(linkable);
                linkable.SetPair(newPair);
            }
            
            Linkable[] linkables = FindObjectsOfType<Linkable>();
            foreach (Linkable lkbl in linkables)
                if (lkbl != linkable)
                    lkbl.DrawLinkEditor(Color.yellow, 0.2f);
            linkable.DrawLinkEditor(Color.green, 0.2f);

            serializedObject.ApplyModifiedProperties(); 
            EditorUtility.SetDirty(target);
        }

        void OnDrawGizmos()
        {
            if (linkable.GetPair() != null)
                Handles.Label(Vector3.Lerp(linkable.gameObject.transform.position, (linkable.GetPair()).gameObject.transform.position, 0.5f), Vector3.Distance(linkable.gameObject.transform.position, (linkable.GetPair()).gameObject.transform.position).ToString());
        }
    }
}
#endif
