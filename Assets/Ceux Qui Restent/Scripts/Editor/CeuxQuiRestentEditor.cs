#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CeuxQuiRestent
{
    public class CeuxQuiRestentEditor : EditorWindow
    {
        #region Attributes
        private RoomManager roomManager;
        private int selectedRoomID = 0;
        private int selectedLinkablesStateID = 0;
        private Room currentRoom;
        #endregion

        #region Methods
        [MenuItem("Window/Level Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            CeuxQuiRestentEditor window = (CeuxQuiRestentEditor)EditorWindow.GetWindow(typeof(CeuxQuiRestentEditor));
            window.Show();
        }

        void OnGUI()
        {
            Color defaultBackgroundColor = GUI.backgroundColor;

            if (Initialize())
            {
                if (roomManager.rooms.Length > 0)
                {
                    EditorGUILayout.LabelField("Select Room:");
                    EditorGUILayout.BeginHorizontal();
                    for (int r = 0; r < roomManager.rooms.Length; r++)
                    {
                        GUI.backgroundColor = ((r == selectedRoomID) ? new Color(0.8f, 0.94f, 0.56f) : defaultBackgroundColor);
                        if (GUILayout.Button(roomManager.rooms[r].gameObject.name))
                            SelectRoom(r);
                    }
                    GUI.backgroundColor = defaultBackgroundColor;
                    EditorGUILayout.EndHorizontal();

                    try
                    {
                        if (currentRoom != null)
                        {
                            if (currentRoom.GetLinkablesLayouts() != null)
                            {
                                Transform linkablesRepository = currentRoom.GetLinkablesLayouts();
                                if (linkablesRepository.childCount > 0)
                                {
                                    EditorGUILayout.Space();
                                    EditorGUILayout.LabelField("Select Current Linkable State:");
                                    EditorGUILayout.BeginHorizontal();
                                    for (int c = 0; c < linkablesRepository.childCount; c++)
                                        SelectLinkablesState(c, linkablesRepository);
                                    GUI.backgroundColor = defaultBackgroundColor;
                                    EditorGUILayout.EndHorizontal();
                                }

                            }
                        }
                    }
                    catch (System.Exception exc)
                    {

                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("RoomManager not found in the scene.");
            }
        }

        public bool Initialize()
        {
            roomManager = (RoomManager)GameObject.FindObjectOfType(typeof(RoomManager));
            return (roomManager != null);
        }

        public void SelectRoom(int roomID)
        {
            if (roomID != selectedRoomID)
            {
                selectedLinkablesStateID = 0;
                currentRoom = roomManager.JumpToRoom(selectedRoomID);
                if (currentRoom != null)
                {
                    if (currentRoom.GetLinkablesLayouts() != null)
                    {
                        Transform linkablesRepository = currentRoom.GetLinkablesLayouts();
                        if (linkablesRepository.childCount > 0)
                            for (int c = 0; c < linkablesRepository.childCount; c++)
                                linkablesRepository.GetChild(c).gameObject.SetActive(c == 0);
                    }
                }
            }
            selectedRoomID = roomID;
            currentRoom = roomManager.JumpToRoom(roomID);
            Selection.activeGameObject = roomManager.rooms[roomID].gameObject;
            EditorApplication.ExecuteMenuItem("Edit/Frame Selected");
            if (currentRoom != null)
            {
                if (currentRoom.GetLinkablesLayouts() != null)
                {
                    Transform linkablesRepository = currentRoom.GetLinkablesLayouts();
                    if (linkablesRepository.childCount > 0)
                        SelectLinkablesState(selectedLinkablesStateID, linkablesRepository);
                }
            }
        }

        public void SelectLinkablesState(int linkablesStateID, Transform linkablesRepository)
        {
            Color defaultBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = ((linkablesStateID == selectedLinkablesStateID) ? new Color(0.8f, 0.94f, 0.56f) : defaultBackgroundColor);
            if (GUILayout.Button(linkablesRepository.GetChild(linkablesStateID).gameObject.name))
            {
                selectedLinkablesStateID = linkablesStateID;
                for (int c2 = 0; c2 < linkablesRepository.childCount; c2++)
                    linkablesRepository.GetChild(c2).gameObject.SetActive((linkablesStateID == c2));
                Selection.activeGameObject = linkablesRepository.GetChild(linkablesStateID).gameObject;
                EditorApplication.ExecuteMenuItem("Edit/Frame Selected");
            }
            GUI.backgroundColor = defaultBackgroundColor;

        }
        #endregion
    }
}
#endif
