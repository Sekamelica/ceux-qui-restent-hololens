using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
    
namespace CeuxQuiRestent
{
    public class Focusable : MonoBehaviour, IFocusable
    {
        public bool interactableFromAnyDistance = false;
        public UnityEvent onFocusEnterEvent;
        public UnityEvent onFocusExitEvent;

        private TechicianCursor cursor;

        private void Start()
        {
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
        }

        public void OnFocusEnter()
        {
            if (onFocusEnterEvent != null)
                onFocusEnterEvent.Invoke();
            if (cursor == null)
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
            cursor.FocusObject(transform, interactableFromAnyDistance);
        }

        public void OnFocusExit()
        {
            if (onFocusExitEvent != null)
                onFocusExitEvent.Invoke();
            if (cursor == null)
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
            cursor.StopFocus(transform);
        }
    }

}
