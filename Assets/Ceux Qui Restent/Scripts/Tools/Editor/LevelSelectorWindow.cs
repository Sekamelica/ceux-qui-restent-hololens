#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using HoloToolkit.Unity.SpatialMapping;

namespace CeuxQuiRestent.Tools
{
    public class LevelSelectorWindow : EditorWindow
    {
        #region Attributes
        private RoomManager roomManager;
        private int selectedRoomID = 0;
        private int selectedLinkablesStateID = 0;
        private Room currentRoom;
        #endregion

        #region Methods
        [MenuItem("Window/Ceux Qui Restent/Level Selector")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            LevelSelectorWindow window = (LevelSelectorWindow)EditorWindow.GetWindow(typeof(LevelSelectorWindow), false, "Level Selector");
            window.Show();
            window.name = "Level Selector";
        }

        void OnGUI()
        {
            Color defaultBackgroundColor = GUI.backgroundColor;

            if (Initialize())
            {
                EditorGUILayout.LabelField("Mode:");
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = new Color(0.6f, 0.7f, 0.92f);
                if (GUILayout.Button("Level Design Mode"))
                {
                    Camera[] cameras = FindObjectsOfType(typeof(Camera)) as Camera[];
                    foreach (Camera cam in cameras)
                        cam.fieldOfView = 70;
                    roomManager.SetLevelDesignMode();
                }
                GUI.backgroundColor = new Color(0.5f, 0.92f, 0.92f);
                if (GUILayout.Button("Build Mode"))
                {
                    Camera[] cameras = FindObjectsOfType(typeof(Camera)) as Camera[];
                    foreach (Camera cam in cameras)
                        cam.fieldOfView = 16;
                    roomManager.SetBuildMode();
                    SpatialMappingManager[] spatialMappingManagers = Resources.FindObjectsOfTypeAll<SpatialMappingManager>();
                    GameObject spatialMappingManager = null;
                    if (spatialMappingManagers.Length > 0)
                        spatialMappingManager = spatialMappingManagers[0].gameObject;
                    if (spatialMappingManager != null)
                    {
                        spatialMappingManager.SetActive(true);
                        if (spatialMappingManager.GetComponent<ObjectSurfaceObserver>() != null)
                        {
                            if (Application.isPlaying)
                                Destroy(spatialMappingManager.GetComponent<ObjectSurfaceObserver>());
                            else
                                DestroyImmediate(spatialMappingManager.GetComponent<ObjectSurfaceObserver>());

                        }
                    }
                }
                GUI.backgroundColor = new Color(0.92f, 0.9f, 0.55f);
                if (GUILayout.Button(new GUIContent("Enable Object Observer", "Used in editor to create a obfuscation mesh of the room. This mesh is also used for the scanning effect."), GUILayout.MaxWidth(150)))
                {
                    SpatialMappingManager[] spatialMappingManagers = Resources.FindObjectsOfTypeAll<SpatialMappingManager>();
                    GameObject spatialMappingManager = null;
                    if (spatialMappingManagers.Length > 0)
                        spatialMappingManager = spatialMappingManagers[0].gameObject;
                    if (spatialMappingManager != null)
                    {
                        spatialMappingManager.SetActive(true);
                        ObjectSurfaceObserver objectSurfaceObserver = (spatialMappingManager.GetComponent<ObjectSurfaceObserver>() != null) ? spatialMappingManager.GetComponent<ObjectSurfaceObserver>() : spatialMappingManager.AddComponent<ObjectSurfaceObserver>();
                        objectSurfaceObserver.RoomModel = roomManager.GetLevelDesignMesh();
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

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
                        if (!Application.isPlaying)
                            if (linkablesRepository.childCount > 0)
                                for (int c = 0; c < linkablesRepository.childCount; c++)
                                    linkablesRepository.GetChild(c).gameObject.SetActive(c == 0);
                    }
                }
            }
            selectedRoomID = roomID;
            currentRoom = roomManager.JumpToRoom(roomID);
            Selection.activeGameObject = roomManager.rooms[roomID].gameObject;
            GameObject levelRoom = GameObject.FindGameObjectWithTag("LevelRoom");
            if (levelRoom != null)
                levelRoom.transform.position = currentRoom.transform.position;
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
                GameObject levelRoom = GameObject.FindGameObjectWithTag("LevelRoom");
                if (levelRoom != null)
                    levelRoom.transform.position = linkablesRepository.GetChild(linkablesStateID).gameObject.transform.position;
                EditorApplication.ExecuteMenuItem("Edit/Frame Selected");
            }
            GUI.backgroundColor = defaultBackgroundColor;

        }
        #endregion
    }
}
#endif
