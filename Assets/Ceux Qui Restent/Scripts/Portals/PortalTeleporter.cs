using UnityEngine;
using Utility;

namespace CeuxQuiRestent.Portals
{
    [RequireComponent(typeof(Collider))]
    public class PortalTeleporter : MonoBehaviour
    {
        #region Attributes
        private RoomManager roomManager;
        [SerializeField]
        private ActionExecuter actionsToDo;
        [SerializeField]
        private bool repeatableActions = false;
        [SerializeField]
        private PortalDestination destination = PortalDestination.Future;
        private GameObject portalRenderer;
        private bool actionsDone = false;
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
                Links.Linker linker = other.gameObject.GetComponent<Links.Linker>();
                if (!linker.HasTakePortal())
                {
                    Vector3 portalToPlayer = (other.gameObject.transform.position - transform.position);
                    float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
                    if (dotProduct > 0)
                    {
                        linker.EnterPortal(portalRenderer);
                        if (destination == PortalDestination.Future)
                            roomManager.NextRoom();
                        else if (destination == PortalDestination.Past)
                            roomManager.PreviousRoom();
                        linker.ExitPortal(portalRenderer);
                        if (actionsToDo != null)
                        {
                            if (!actionsDone || repeatableActions)
                            {
                                actionsToDo.StartActions();
                                actionsDone = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Getters & Setters
        public void SetDestination(PortalDestination _destination)
        {
            destination = _destination;
        }

        public void SetRenderer(GameObject _renderer)
        {
            portalRenderer = _renderer;
        }
        #endregion
    }

}
