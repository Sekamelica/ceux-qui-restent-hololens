using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Utility;

namespace CeuxQuiRestent
{
    [System.Serializable]
    [RequireComponent(typeof(Collider))]
    public class Linkable : MonoBehaviour, IInputClickHandler, IInputHandler
    {
        // Public Attributes
        public float energyMaximumIncrease = 2.5f;
        public Linkable pair;
        public ActionExecuter actionsToDo;
        
        // Private attributes        
        private bool alreadyLinked = false;
        private Linker linker;

        #region Linkable Methods
        /// <summary>
        /// Called when you try to interact with this Linkable.
        /// </summary>
        public void Interact()
        {
            if (!alreadyLinked && pair != null)
                linker.LinkableClick(transform.position, gameObject, pair.gameObject);
        }

        /// <summary>
        /// When the linkable is linked to his pair.
        /// </summary>
        public void Linked()
        {
            alreadyLinked = true;
            if (actionsToDo != null)
            {
                actionsToDo.ResetCounter();
                actionsToDo.StartActions();
            }
        }

#if UNITY_EDITOR
        public void EditorDrawLink(Color col, float time)
        {
            if (pair != null)
                Debug.DrawLine(transform.position, pair.gameObject.transform.position, col, time);
        }
#endif
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
        }
        #endregion

        #region Input Management
        /// <summary>
        /// Simple click on object
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Interact();
        }

        /// <summary>
        /// When the click start to be pressed
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputDown(InputEventData eventData)
        {

        }

        /// <summary>
        /// When the click is released.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputUp(InputEventData eventData)
        {

        }
        #endregion

        #region Getters & Setters
        public bool IsAlreadyLinked()
        {
            return alreadyLinked;
        }
        #endregion
    }

}