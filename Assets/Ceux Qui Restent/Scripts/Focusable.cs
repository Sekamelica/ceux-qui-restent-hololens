using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
    
namespace CeuxQuiRestent
{
    public class Focusable : MonoBehaviour, IFocusable
    {
        private TechicianCursor cursor;
        public UnityEvent onFocusEnterEvent;
        public UnityEvent onFocusExitEvent;

        private void Start()
        {
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
        }

        public void OnFocusEnter()
        {
            onFocusEnterEvent.Invoke();
            cursor.FocusObject(transform);
        }

        public void OnFocusExit()
        {
            onFocusExitEvent.Invoke();
            cursor.StopFocus(transform);
        }
    }

}
