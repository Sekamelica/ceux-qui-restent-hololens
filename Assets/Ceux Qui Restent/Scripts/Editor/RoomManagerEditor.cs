using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent
{
    [CustomEditor(typeof(RoomManager))]
    public class RoomManagerEditor : Editor
    {
        RoomManager roomManager;

        void OnEnable()
        {
            roomManager = target as RoomManager;
        }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif