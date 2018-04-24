using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace CeuxQuiRestent
{
    [System.Serializable]
    public class Linkable : MonoBehaviour, IInputClickHandler, IInputHandler
    {
        public float energyMaximumIncrease = 2.5f;
        [SerializeField]
        private Linkable pair;
        [NonSerialized]
        public bool alreadyLinked = false;

        private Linker linker;

        // Use this for initialization
        void Start()
        {
            linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void DrawLinkEditor(Color col, float time)
        {
            if (pair != null)
                Debug.DrawLine(transform.position, pair.gameObject.transform.position, col, time);
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (!alreadyLinked && pair != null)
                linker.LinkableClick(transform.position, gameObject, pair.gameObject);
        }
        public void OnInputDown(InputEventData eventData)
        {
            //linker.LinkableClick(transform.position);
        }
        public void OnInputUp(InputEventData eventData)
        {
            //linker.LinkableClick(transform.position);
        }

        public Linkable GetPair()
        {
            return pair;
        }

        public void SetPair(Linkable _newPair)
        {
            pair = _newPair;
        }
    }

}