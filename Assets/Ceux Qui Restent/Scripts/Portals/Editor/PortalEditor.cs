using UnityEngine;
using CeuxQuiRestent.Portals;
#if UNITY_EDITOR
using UnityEditor;


namespace CeuxQuiRestent.Tools
{
    [CustomEditor(typeof(Portal))]
    public class PortalEditor : Editor
    {
        Portal portal;

        void OnEnable()
        {
            portal = target as Portal;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Generate destination portal"))
                portal.GenerateDestinationPortal();
            portal.UpdateTeleporterAndRenderer();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif