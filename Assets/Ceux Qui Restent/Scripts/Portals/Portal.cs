using UnityEngine;

namespace CeuxQuiRestent.Portals
{
    public class Portal : MonoBehaviour
    {
        #region Attributes
        public PortalDestination destination = PortalDestination.Future;
        [Space]
        public MeshRenderer portalRenderer;
        public PortalTeleporter portalTeleporter;
        [Space]
        public PortalParameters portalParameters;
        #endregion

        #region Methods
        public void UpdateTeleporterAndRenderer()
        {
            if (portalTeleporter != null)
                portalTeleporter.SetDestination(destination);
            if (portalParameters != null)
            {
                if (portalRenderer != null)
                {
                    if (destination == PortalDestination.Future)
                        if (portalParameters.futureMaterial != null)
                            portalRenderer.material = portalParameters.futureMaterial;
                    if (destination == PortalDestination.Past)
                        if (portalParameters.pastMaterial != null)
                            portalRenderer.material = portalParameters.pastMaterial;
                }
            }
        }

        public void GenerateDestinationPortal()
        {
            GameObject destinationPortalGameObject = GameObject.Instantiate(gameObject);
            destinationPortalGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 500 * ((destination == PortalDestination.Future) ? 1 : -1), transform.position.z);
            destinationPortalGameObject.transform.Rotate(Vector3.up, 180);

            Portal destinationPortal = destinationPortalGameObject.GetComponent<Portal>();
            destinationPortal.destination = ((destination == PortalDestination.Future) ? PortalDestination.Past : PortalDestination.Future);
            destinationPortal.UpdateTeleporterAndRenderer();
        }
        #endregion

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.cyan;
            style.fontSize = 16;
            UnityEditor.Handles.Label(transform.position + (Vector3.up * 1.5f), ((destination == PortalDestination.Future) ? "To Future" : "To Past"), style);
        }
#endif
    }

}
