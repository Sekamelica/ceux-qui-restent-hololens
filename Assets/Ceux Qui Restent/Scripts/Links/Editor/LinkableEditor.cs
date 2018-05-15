using UnityEngine;
using Utility;

#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Gameplay
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
                    linkable.EditorDrawLink(Color.red, 0.4f);
                    linkable.pair.pair = null;
                }

                if (newPair != null)
                {
                    if (newPair.pair != null)
                    {
                        newPair.EditorDrawLink(Color.red, 0.4f);
                        newPair.pair.pair = null;
                    }

                    // Create new pairs
                    newPair.pair = linkable;
                    linkable.pair = newPair;
                    linkable.pair.actionsToDo = linkable.actionsToDo;
                }

            }

            // Display all pair links
            Linkable[] linkables = FindObjectsOfType<Linkable>();
            foreach (Linkable lkbl in linkables)
                if (lkbl != linkable)
                    lkbl.EditorDrawLink(Color.yellow, 0.2f);
            linkable.EditorDrawLink(Color.green, 0.2f);


            if (linkable.pair != null)
            {
                linkable.pair.actionsToDo = linkable.actionsToDo;
                if (linkable.pair.pair != null)
                    linkable.pair.pair = null;
                linkable.pair.pair = linkable;
            }
            
            // Display current pair data
            if (linkable.pair != null)
            {
                
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Pair datas:");
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                EditorGUI.BeginDisabledGroup(true);
                float unused_dist = EditorGUILayout.FloatField("Distance with pair", Vector3.Distance(linkable.gameObject.transform.position, linkable.pair.gameObject.transform.position));
                /*
                if (GUILayout.Button("Correct Pair"))
                {
                    linkable.pair.actionsToDo = linkable.actionsToDo;
                    linkable.pair.energyMaximumIncrease = linkable.energyMaximumIncrease;
                    if (linkable.pair.pair != null)
                        linkable.pair.pair = null;
                    linkable.pair.pair = linkable;
                }

                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;

                EditorGUI.BeginDisabledGroup(true);
                ActionExecuter unused_ac = EditorGUILayout.ObjectField("Actions to do", linkable.pair.actionsToDo, typeof(ActionExecuter), true) as ActionExecuter;
                float unused_en = EditorGUILayout.FloatField("Energy Increase", linkable.pair.energyMaximumIncrease);
                Linkable unused_pair = EditorGUILayout.ObjectField("Pair", linkable.pair.pair, typeof(Linkable), true) as Linkable;*/
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
            

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
