using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;

namespace CeuxQuiRestent
{
    [RequireComponent(typeof(Focusable))]
    public class Interactible : MonoBehaviour, IInputClickHandler
    {
        public bool interactableFromAnyDistance = false;
        public UnityEvent onInteractEvent;

        private float distanceInteraction;
        private Transform technician;
        private TechicianCursor cursor;

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
    }

}
