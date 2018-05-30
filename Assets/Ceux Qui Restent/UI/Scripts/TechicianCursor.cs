using UnityEngine;
using UnityEngine.UI;

namespace CeuxQuiRestent.UI
{
    public class TechicianCursor : MonoBehaviour
    {
        #region Attributes
        // Public attributes
        public float distanceInteraction = 2;
        public Sprite tracker_normal;
        public Sprite tracker_interact_too_far;
        public Sprite tracker_interact;

        // Private attributes
        private bool focusing = false;
        private bool readyToInteract = false;
        private Transform technician;
        private Image image;
        private Transform focusedObject;
        private bool interactableFromAnyDistance = false;
        private Animator animator;
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Start()
        {
            technician = GameObject.FindGameObjectWithTag("MainCamera").transform;
            image = GetComponent<Image>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            animator.SetBool("ShouldFocus", focusing);
            image.SetNativeSize();
            if (focusing)
            {
                if (interactableFromAnyDistance)
                {
                    animator.SetBool("EnhancedFocus", true);
                    readyToInteract = true;
                    animator.SetBool("Focus", false);
                }
                else
                {
                    Ray ray = new Ray(technician.position, (technician.position + technician.forward) - technician.position);
                    RaycastHit rayHit = new RaycastHit();
                    if (Physics.Raycast(ray, out rayHit, 40, LayerMask.GetMask(new string[] { LayerMask.LayerToName(focusedObject.gameObject.layer) })))
                    {
                        if (rayHit.distance <= distanceInteraction)
                        {
                            animator.SetBool("EnhancedFocus", true);
                            readyToInteract = true;
                            animator.SetBool("Focus", false);
                        }
                        else
                        {
                            animator.SetBool("EnhancedFocus", false);
                            readyToInteract = false;
                            animator.SetBool("Focus", true);
                        }
                    }
                    else
                        StopFocus(focusedObject);
                }
            }
            else
            {
                animator.SetBool("EnhancedFocus", false);
                animator.SetBool("Focus", false);
                readyToInteract = false;
            }
        }
        #endregion

        #region Methods
        public void CursorClickAnimation()
        {
            animator.SetTrigger("Click");
        }

        public void FocusObject(Transform _focusedObject, bool _interactableFromAnyDistance)
        {
            interactableFromAnyDistance = _interactableFromAnyDistance;
            focusedObject = _focusedObject;
            focusing = true;
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

        public bool GetReadyToInteract()
        {
            return readyToInteract;
        }
        #endregion
    }

}