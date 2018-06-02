using UnityEngine;
using System.Collections.Generic;
using HoloToolkit.Unity.SpatialMapping;
using CeuxQuiRestent.Links;

namespace CeuxQuiRestent
{
    public class RoomManager : MonoBehaviour
    {
        #region Attributes
        public int startingRoomID = 0;
        public Transform roomHolder;
        public Room[] rooms;
        public GameObject levelDesignMeshPrefab;
        
        private int currentRoomID = 0;
        [System.NonSerialized]
        public Room currentRoom;
        [SerializeField]
        private List<GameObject> levelDesignMeshes = new List<GameObject>();
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            for (int r = 0; r < rooms.Length; r++)
            {
                rooms[r].gameObject.SetActive(false);
                rooms[r].transform.position = roomHolder.position + new Vector3(0, r * 500, 0);
                rooms[r].transform.rotation = transform.rotation;
                rooms[r].transform.localScale = Vector3.one;
            }
            while (currentRoomID < startingRoomID)
                NextRoom();
            currentRoom = rooms[currentRoomID];

            // Delete level room
            GameObject levelRoom = GameObject.FindGameObjectWithTag("LevelRoom");
            if (levelRoom != null)
                GameObject.Destroy(levelRoom);
        }
        #endregion

        #region RoomManager Methods
        public void InitializeRooms()
        {
            float defaultDistance = 1.85f;
            float distanceYOffset = 0;
            Linker linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
            float higherDistance = 0;
            for (int x = -5; x < 6; x++)
            {
                for (int y = -5; y < 6; y++)
                {
                    Vector3 origin = new Vector3(linker.target.position.x + x, linker.target.position.y, linker.target.position.z + y);
                    Ray ray = new Ray(origin, (origin - Vector3.up) - origin);
                    RaycastHit raycastHit = new RaycastHit();
                    if (Physics.Raycast(ray, out raycastHit, 30, LayerMask.GetMask(new string[] { "Spatial Mapping Mesh" })))
                        distanceYOffset = defaultDistance - raycastHit.distance;
                }
            }
            Debug.Log("Test ray");
            Debug.DrawLine(linker.target.position, new Vector3(linker.target.position.x, linker.target.position.y - higherDistance, linker.target.position.z), Color.red, 50);
            for (int r = 0; r < rooms.Length; r++)
            {
                rooms[r].gameObject.transform.position = new Vector3(rooms[r].gameObject.transform.position.x, rooms[r].gameObject.transform.position.y + distanceYOffset, rooms[r].gameObject.transform.position.z);
                rooms[r].gameObject.SetActive(true);
            }
        }

        public Room JumpToRoom(int _roomID)
        {
            for (int r = 0; r < rooms.Length; r++)
            {
                rooms[r].transform.position = roomHolder.position + new Vector3(0, r * 500, 0);
                rooms[r].transform.rotation = transform.rotation;
                rooms[r].transform.localScale = Vector3.one;
            }
            return rooms[_roomID];
        }

        public void NextRoom()
        {
            currentRoom.StopRoomMusic();
            if (currentRoomID + 1 < rooms.Length)
            {
                currentRoomID++;
                for (int r = 0; r < rooms.Length; r++)
                    rooms[r].transform.position = new Vector3(rooms[r].transform.position.x, rooms[r].transform.position.y - 500, rooms[r].transform.position.z);
            }
            currentRoom = rooms[currentRoomID];
            currentRoom.UseRoomPostProcessing();
            currentRoom.StartRoomMusic();
        }

        public void PreviousRoom()
        {
            currentRoom.StopRoomMusic();
            if (currentRoomID - 1 >= 0)
            {
                currentRoomID--;
                for (int r = 0; r < rooms.Length; r++)
                    rooms[r].transform.position = new Vector3(rooms[r].transform.position.x, rooms[r].transform.position.y + 500, rooms[r].transform.position.z);
            }
            currentRoom = rooms[currentRoomID];
            currentRoom.UseRoomPostProcessing();
            currentRoom.StartRoomMusic();
        }

        public int GetCurrentRoomID()
        {
            return currentRoomID;
        }

#if UNITY_EDITOR
        public void SetLevelDesignMode()
        {
            for (int ldm = levelDesignMeshes.Count - 1; ldm >= 0; ldm--)
            {
                if (Application.isPlaying)
                    Destroy(levelDesignMeshes[ldm]);
                else
                    DestroyImmediate(levelDesignMeshes[ldm]);
            }
            levelDesignMeshes.Clear();
            levelDesignMeshes = new List<GameObject>();
            for (int r = 0; r < rooms.Length + 1; r++)
                levelDesignMeshes.Add(GameObject.Instantiate(levelDesignMeshPrefab));
            for (int ldm = 0; ldm < levelDesignMeshes.Count; ldm++)
            {
                levelDesignMeshes[ldm].transform.parent = roomHolder;
                levelDesignMeshes[ldm].transform.SetPositionAndRotation(roomHolder.position - (Vector3.up * 500) + (ldm * (Vector3.up * 500)), roomHolder.rotation);
            }
            SpatialMappingManager[] spatialMappingManagers = Resources.FindObjectsOfTypeAll<SpatialMappingManager>();
            GameObject spatialMappingManager = null;
            if (spatialMappingManagers.Length > 0)
                spatialMappingManager = spatialMappingManagers[0].gameObject;
            if (spatialMappingManager != null)
                spatialMappingManager.SetActive(false);
        }

        public GameObject GetLevelDesignMesh()
        {
            if (levelDesignMeshes.Count > 0)
                return levelDesignMeshes[0];
            else
                return null;
        }

        public void SetBuildMode()
        {
            for (int ldm = levelDesignMeshes.Count - 1; ldm >= 0; ldm--)
            {
                if (Application.isPlaying)
                    Destroy(levelDesignMeshes[ldm]);
                else
                    DestroyImmediate(levelDesignMeshes[ldm]);
            }
            levelDesignMeshes.Clear();
            SpatialMappingManager[] spatialMappingManagers = Resources.FindObjectsOfTypeAll<SpatialMappingManager>();
            GameObject spatialMappingManager = null;
            if (spatialMappingManagers.Length > 0)
                spatialMappingManager = spatialMappingManagers[0].gameObject;
            if (spatialMappingManager != null)
                spatialMappingManager.SetActive(true);
        }
#endif
    #endregion
    }

}
