using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CeuxQuiRestent
{
    public class TechicianCursor : MonoBehaviour
    {
        public float distanceInteraction = 2;
        public Sprite tracker_normal;
        public Sprite tracker_interact_too_far;
        public Sprite tracker_interact;

        private bool focusing = false;
        private Transform technician;
        private Image image;
        private Transform focusedObject;
        private bool interactableFromAnyDistance = false;

        // Use this for initialization
        void Start()
        {
            technician = GameObject.FindGameObjectWithTag("MainCamera").transform;
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (focusing)
            {
                if (interactableFromAnyDistance)
                {
                    image.sprite = tracker_interact;
                }
                else
                {
                    Ray ray = new Ray(technician.position, focusedObject.position - technician.position);
                    RaycastHit rayHit = new RaycastHit();
                    if (Physics.Raycast(ray, out rayHit, 30, LayerMask.GetMask(new string[] { LayerMask.LayerToName(focusedObject.gameObject.layer) })))
                    {
                        if (rayHit.distance <= distanceInteraction)
                            image.sprite = tracker_interact;
                        else
                            image.sprite = tracker_interact_too_far;
                    }
                    else
                        StopFocus(focusedObject);
                }
            }
        }

        public void FocusObject(Transform _focusedObject, bool _interactableFromAnyDistance)
        {
            interactableFromAnyDistance = _interactableFromAnyDistance;
            focusedObject = _focusedObject;
            focusing = true;
            if (interactableFromAnyDistance)
            {
                image.sprite = tracker_interact;
            }
            else
            {
                Ray ray = new Ray(technician.position, focusedObject.position - technician.position);
                RaycastHit rayHit = new RaycastHit();
                if (Physics.Raycast(ray, out rayHit, 30, LayerMask.GetMask(new string[] { LayerMask.LayerToName(focusedObject.gameObject.layer) })))
                {
                    if (rayHit.distance <= distanceInteraction)
                        image.sprite = tracker_interact;
                    else
                        image.sprite = tracker_interact_too_far;
                }
                else
                    StopFocus(focusedObject);
            }
        }

        public void StopFocus(Transform _stopFocusedObject)
        {
            if (focusedObject != null)
            {
                if (_stopFocusedObject.gameObject == focusedObject.gameObject)
                {
                    image.sprite = tracker_normal;
                    focusedObject = null;
                    focusing = false;
                }
            }
        }
    }

}