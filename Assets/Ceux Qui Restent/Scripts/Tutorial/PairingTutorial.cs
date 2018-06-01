using UnityEngine;
using UnityEngine.Events;
using CeuxQuiRestent.Links;
using CeuxQuiRestent.Interactables;
using CeuxQuiRestent.UI;

namespace CeuxQuiRestent.Tutorial
{
    public class PairingTutorial : MonoBehaviour
    {
        #region Attributes
        [Header("Reproducable events")]
        public UnityEvent enterFocusFragment_event;
        public UnityEvent exitFocusFragment_event;

        private bool focusing = false;

        [Space]
        [Header("Unique events")]
        public float focusFirstFragment_time = 0.25f;
        public UnityEvent focusFirstFragment_event;
        public UnityEvent interactWithFragment_event;
        public UnityEvent focusSecondFragment_event;

        private bool focusFirstFragment_delayEnabled = false;
        private float focusFirstFragment_time_current = 0;
        private bool focusFirstFragment_eventDone = false;
        private bool interactWithFragment_eventDone = false;
        private bool focusSecondFragment_eventDone = false;
        private Linkable lastFragmentYouInteractWith;
        private Linker linker;
        private TechnicianCursor cursor;
        private bool displayIcon = false;
        #endregion

        #region Methods
        private void Start()
        {
            linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
            cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechnicianCursor>();
        }

        private void Update()
        {
            if (linker == null)
                linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
            if (cursor == null)
                cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechnicianCursor>();

            // Short delay before triggering the help voiceline when focusing the first linkable
            if (!focusFirstFragment_eventDone)
            {
                if (focusFirstFragment_delayEnabled)
                {
                    focusFirstFragment_time_current += Time.deltaTime;
                    if (focusFirstFragment_time_current >= focusFirstFragment_time)
                    {
                        focusFirstFragment_event.Invoke();
                        focusFirstFragment_eventDone = true;
                        focusFirstFragment_delayEnabled = false;
                    }
                }
            }

            if (focusing && !displayIcon && cursor.GetReadyToInteract())
            {
                enterFocusFragment_event.Invoke();
                displayIcon = true;
            }

            if (focusing && displayIcon && !cursor.GetReadyToInteract())
            {
                exitFocusFragment_event.Invoke();
                displayIcon = false;
            }

            if (!focusing && displayIcon)
            {
                exitFocusFragment_event.Invoke();
                displayIcon = false;
            }
        }

        public void FocusFragment(Linkable fragment)
        {
            if (fragment.CanBeLinked())
            {
                focusing = true;
                if (linker.IsLinking())
                {
                    if (lastFragmentYouInteractWith != null)
                    {
                        if (!focusSecondFragment_eventDone && fragment != lastFragmentYouInteractWith)
                        {
                            focusSecondFragment_event.Invoke();
                            focusSecondFragment_eventDone = true;
                        }
                    }
                }
                else
                {
                    if (!focusFirstFragment_eventDone)
                    {
                        focusFirstFragment_time_current = 0;
                        focusFirstFragment_delayEnabled = true;
                    }
                }
            }
        }

        public void ExitFocusFragment()
        {
            focusing = false;
            focusFirstFragment_delayEnabled = false;
        }

        public void InteractFragment(Linkable fragment)
        {
            if (fragment.CanBeLinked())
            {
                focusing = false;
                lastFragmentYouInteractWith = fragment;
                if (!interactWithFragment_eventDone)
                {
                    interactWithFragment_event.Invoke();
                    interactWithFragment_eventDone = true;
                }
            }
        }
        #endregion
    }
}

