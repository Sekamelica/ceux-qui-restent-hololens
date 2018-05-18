using UnityEngine;

namespace CeuxQuiRestent.Portals
{
    [RequireComponent(typeof(Collider))]
    public class PortalTeleporter : MonoBehaviour
    {
        #region Attributes
        private RoomManager roomManager;
        [SerializeField]
        private PortalDestination destination = PortalDestination.Future;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Vector3 portalToPlayer = (other.gameObject.transform.position - transform.position);
                float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
                if (dotProduct > 0)
                {
                    if (destination == PortalDestination.Future)
                        roomManager.NextRoom();
                    else if (destination == PortalDestination.Past)
                        roomManager.PreviousRoom();
                }
            }
        }
        #endregion

        #region Getters & Setters
        public void SetDestination(PortalDestination _destination)
        {
            destination = _destination;
        }
        #endregion
    }

}
