using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;

namespace CeuxQuiRestent
{
    [RequireComponent(typeof(Focusable))]
    public class Interactible : MonoBehaviour, IInputClickHandler
    {
        public UnityEvent onInteractEvent;

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (onInteractEvent != null)
                onInteractEvent.Invoke();
        }
    }

}
