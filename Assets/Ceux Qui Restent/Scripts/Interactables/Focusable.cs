using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using CeuxQuiRestent.UI;
    
namespace CeuxQuiRestent.Interactables
{
    public class Focusable : MonoBehaviour, IFocusable
    {
        #region Attributes
        [System.NonSerialized]
        public bool interactableFromAnyDistance = false;
        public UnityEvent onFocusEnterEvent;
        public UnityEvent onFocusExitEvent;
        public GameObject focusSound;

        private TechnicianCursor cursor;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechnicianCursor>();
        }

        private void OnDisable()
        {
            OnFocusExit();
        }
        #endregion

        #region Focusable Methods
        public void OnFocusEnter()
        {
            if (focusSound != null)
                GameObject.Instantiate(focusSound, transform.position, Quaternion.identity, null);
            onFocusEnterEvent.Invoke();
            cursor.FocusObject(transform, interactableFromAnyDistance);
        }

        public void OnFocusExit()
        {
            try
            {
                onFocusExitEvent.Invoke();
                cursor.StopFocus(transform);
            } catch (System.Exception exc)
            {

            }
        }
        #endregion
    }

}
