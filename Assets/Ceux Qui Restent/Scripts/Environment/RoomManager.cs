using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent
{
    public class RoomManager : MonoBehaviour
    {
        #region Attributes
        public int startingRoomID = 0;
        public Transform roomHolder;
        public Room[] rooms;
        
        private int currentRoomID = 0;
        [System.NonSerialized]
        public Room currentRoom;
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
            for (int r = 0; r < rooms.Length; r++)
            {
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
            if (currentRoomID + 1 < rooms.Length)
            {
                currentRoomID++;
                for (int r = 0; r < rooms.Length; r++)
                    rooms[r].transform.position = new Vector3(rooms[r].transform.position.x, rooms[r].transform.position.y - 500, rooms[r].transform.position.z);
            }
            currentRoom = rooms[currentRoomID];
            currentRoom.UseRoomPostProcessing();
        }

        public void PreviousRoom()
        {
            if (currentRoomID - 1 >= 0)
            {
                currentRoomID--;
                for (int r = 0; r < rooms.Length; r++)
                    rooms[r].transform.position = new Vector3(rooms[r].transform.position.x, rooms[r].transform.position.y + 500, rooms[r].transform.position.z);
            }
            currentRoom = rooms[currentRoomID];
            currentRoom.UseRoomPostProcessing();
        }

        public int GetCurrentRoomID()
        {
            return currentRoomID;
        }
        #endregion
    }

}
