using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent
{
    public enum PortalDestination
    {
        NextRoom,
        PreviousRoom
    }

    [RequireComponent(typeof(Collider))]
    public class Portal : MonoBehaviour
    {
        #region Attributes
        public PortalDestination destination = PortalDestination.NextRoom;

        private RoomManager roomManager;
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Start()
        {
            roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                roomManager.portalValue++;
                if (roomManager.portalValue % 3 == 0)
                {
                    if (destination == PortalDestination.NextRoom)
                        roomManager.NextRoom();
                    else if (destination == PortalDestination.PreviousRoom)
                        roomManager.PreviousRoom();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                roomManager.portalValue++;
            }
        }
        #endregion
    }

}
