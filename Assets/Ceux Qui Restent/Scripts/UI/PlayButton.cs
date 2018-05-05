using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace CeuxQuiRestent
{
    public class PlayButton : MonoBehaviour, IInputClickHandler, IInputHandler
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Interact()
        {

        }

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
    }
}
