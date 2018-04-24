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
            Linkable newPair = EditorGUILayout.ObjectField("Pair", linkable.pair, typeof(Linkable), true) as Linkable;
            if (newPair != linkable.pair)
            {
                // Destroy potential old pairs
                if (linkable.pair != null)
                    linkable.pair.pair = null;
                if (newPair.pair != null)
                    newPair.pair.pair = null;
                // Create new pairs
                newPair.pair = linkable;
                linkable.pair = newPair;
            }

            if(linkable.pair != null)
                Debug.DrawLine(linkable.gameObject.transform.position, linkable.pair.gameObject.transform.position, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
