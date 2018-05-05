using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Utility;

namespace CeuxQuiRestent
{
    [System.Serializable]
    [RequireComponent(typeof(Focusable))]
    [RequireComponent(typeof(Collider))]
    public class Linkable : MonoBehaviour, IInputClickHandler
    {
        // Public Attributes
        public float energyMaximumIncrease = 2.5f;
        public Linkable pair;
        public ActionExecuter actionsToDo;
        
        // Private attributes        
        private bool alreadyLinked = false;
        private Linker linker;

        #region Input Management
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Interact();
        }
        #endregion

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

        #region Getters & Setters
        public bool IsAlreadyLinked()
        {
            return alreadyLinked;
        }
        #endregion
    }

}