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
        public AK.Wwise.Event focusSound = null;

        private TechicianCursor cursor;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
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
            {
                Audio.AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<Audio.AudioManager>();
                audioManager.PlayWwiseEvent(gameObject, focusSound);
            }
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
