using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace CeuxQuiRestent
{
    public class Linkable : MonoBehaviour, IInputClickHandler, IInputHandler
    {

        Linker linker;

        // Use this for initialization
        void Start()
        {
            linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            linker.LinkableClick(transform.position);
        }
        public void OnInputDown(InputEventData eventData)
        {
            //linker.LinkableClick(transform.position);
        }
        public void OnInputUp(InputEventData eventData)
        {
            //linker.LinkableClick(transform.position);
        }
    }

}