using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace CeuxQuiRestent
{
    public class Focusable : MonoBehaviour, IFocusable
    {
        private TechicianCursor cursor;

        private void Start()
        {
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>();
        }

        public void OnFocusEnter()
        {
            cursor.FocusObject(transform);
        }

        public void OnFocusExit()
        {
            cursor.StopFocus(transform);
        }
    }

}
