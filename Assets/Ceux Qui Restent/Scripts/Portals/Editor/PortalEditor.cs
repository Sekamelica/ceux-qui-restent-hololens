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
            portal.destination = (PortalDestination)EditorGUILayout.EnumPopup("Destination", portal.destination);
            EditorGUILayout.Space();
            portal.portalRenderer = EditorGUILayout.ObjectField("Renderer", portal.portalRenderer, typeof(MeshRenderer), true) as MeshRenderer;
            portal.portalTeleporter = EditorGUILayout.ObjectField("Teleporter", portal.portalTeleporter, typeof(PortalTeleporter), true) as PortalTeleporter;
            EditorGUILayout.Space();
            portal.portalParameters = EditorGUILayout.ObjectField("Parameters", portal.portalParameters, typeof(PortalParameters), true) as PortalParameters;
            EditorGUILayout.Space();
            Portal newPortalDestination = EditorGUILayout.ObjectField("Portal Destination", portal.portalDestination, typeof(Portal), true) as Portal;
            if (newPortalDestination != null)
            {
                newPortalDestination.destination = (portal.destination == PortalDestination.Future) ? PortalDestination.Past : PortalDestination.Future;
                newPortalDestination.transform.rotation = portal.transform.rotation;
                newPortalDestination.transform.Rotate(Vector3.up, 180);
                newPortalDestination.transform.position = portal.transform.position + (Vector3.up * 500 * ((portal.destination == PortalDestination.Future) ? 1 : -1));
                newPortalDestination.portalDestination = portal;
                newPortalDestination.UpdateTeleporterAndRenderer();
            }
            portal.portalDestination = newPortalDestination;
            /*
            if (GUILayout.Button("Generate destination portal"))
                portal.GenerateDestinationPortal();*/
            portal.UpdateTeleporterAndRenderer();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif