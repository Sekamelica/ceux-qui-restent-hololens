using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using CeuxQuiRestent.UI;
    
namespace CeuxQuiRestent.Gameplay
{
    public class Focusable : MonoBehaviour, IFocusable
    {
        #region Attributes
        [System.NonSerialized]
        public bool interactableFromAnyDistance = false;
        public UnityEvent onFocusEnterEvent;
        public UnityEvent onFocusExitEvent;

        private TechicianCursor cursor;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
                return;
            onFocusExitEvent.Invoke();
            if (cursor == null)
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
            cursor.StopFocus(transform);
        }
        #endregion

        #region Focusable Methods
        public void OnFocusEnter()
        {
            onFocusEnterEvent.Invoke();
            if (cursor == null)
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
            cursor.FocusObject(transform, interactableFromAnyDistance);
        }

        public void OnFocusExit()
        {
            onFocusExitEvent.Invoke();
            if (cursor == null)
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
            cursor.StopFocus(transform);
        }
        #endregion
    }

}
