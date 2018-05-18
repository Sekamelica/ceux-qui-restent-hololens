using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using CeuxQuiRestent.UI;

namespace CeuxQuiRestent.Interactables
{
    [RequireComponent(typeof(Focusable))]
    public class Interactible : MonoBehaviour, IInputClickHandler
    {
        #region Attributes
        // Public attributes
        public bool interactableFromAnyDistance = false;
        public UnityEvent onInteractEvent;

        // Private attributes
        private float distanceInteraction;
        private Transform technician;
        private TechicianCursor cursor;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            if (!interactableFromAnyDistance)
            {
                technician = GameObject.FindGameObjectWithTag("MainCamera").transform;
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
                distanceInteraction = cursor.distanceInteraction;
            }
            GetComponent<Focusable>().interactableFromAnyDistance = interactableFromAnyDistance;
        }
        #endregion

        #region Methods
        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (onInteractEvent != null)
            {
                if (!interactableFromAnyDistance)
                {
                    Ray ray = new Ray(technician.position, transform.position - technician.position);
                    RaycastHit rayHit = new RaycastHit();
                    if (Physics.Raycast(ray, out rayHit, 30, LayerMask.GetMask(new string[] { LayerMask.LayerToName(transform.gameObject.layer) })))
                    {
                        if (rayHit.distance <= distanceInteraction)
                            onInteractEvent.Invoke();
                    }
                }
                else
                    onInteractEvent.Invoke();
            }
        }
        #endregion
    }

}
