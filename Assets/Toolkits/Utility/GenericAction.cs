using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public enum GenericActionKind
    {
        Instantiate,
        Enable,
        Disable,
        EnableDisable,
        Destroy,
        PlayParticleSystem,
        PlayAudioSource,
        StartActionsExecuter,
        PlayWWiseAudioSource,
        StartEventExecuter,
        AppearLinkable,
        DisappearLinkable,
        StartCinematicMode,
        EndCinematicMode
    }

    [System.Serializable]
    public class GenericAction
    {
        // Attributes
        [SerializeField]
        private GenericActionKind actionKind;
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private float secondsToWait = 0;
        [SerializeField]
        private bool done = false;

        private float secondsToWaitOrigin;
        
        // Constructors
        public GenericAction()
        {
            this.actionKind = GenericActionKind.Instantiate;
            this.target = null;
        }

        public GenericAction(GenericActionKind _actionKind)
        {
            this.actionKind = _actionKind;
        }

        public GenericAction(GenericActionKind _actionKind, GameObject _target)
        {
            this.actionKind = _actionKind;
            this.target = _target;
        }

        // Methods
        public void SaveSecondsToWaitOrigin()
        {
            secondsToWaitOrigin = secondsToWait;
        }

        public void ResetSecondsToWait()
        {
            secondsToWait = secondsToWaitOrigin;
        }

        public void ResetIsDone()
        {
            done = false;
        }

        public void Execute(Transform master)
        {
            if (secondsToWait > 0)
                secondsToWait -= Time.deltaTime;
            if (secondsToWait <= 0)
            {
                secondsToWait = 0;
                switch (actionKind)
                {
                    case GenericActionKind.Instantiate:
                        if (target != null)
                            GameObject.Instantiate(target, master.position, Quaternion.identity, master);
                        break;
                    case GenericActionKind.Enable:
                        if (target != null)
                            target.SetActive(true);
                        break;
                    case GenericActionKind.Disable:
                        if (target != null)
                            target.SetActive(false);
                        break;
                    case GenericActionKind.EnableDisable:
                        if (target != null)
                            target.SetActive(!target.activeSelf);
                        break;
                    case GenericActionKind.Destroy:
                        if (target != null)
                            GameObject.Destroy(target);
                        break;
                    case GenericActionKind.PlayParticleSystem:
                        if (target != null)
                            if (target.gameObject.GetComponent<ParticleSystem>())
                                target.gameObject.GetComponent<ParticleSystem>().Play();
                        break;
                    case GenericActionKind.PlayAudioSource:
                        if (target != null)
                            if (target.gameObject.GetComponent<AudioSource>())
                                target.gameObject.GetComponent<AudioSource>().Play();
                        break;
                    case GenericActionKind.StartActionsExecuter:
                        if (target != null)
                        {
                            if (target.gameObject.GetComponent<ActionExecuter>())
                            {
                                target.gameObject.GetComponent<ActionExecuter>().ResetCounter();
                                target.gameObject.GetComponent<ActionExecuter>().StartActions();
                            }
                        }
                        break;
                    case GenericActionKind.StartEventExecuter:
                        if (target != null)
                            if (target.gameObject.GetComponent<EventExecuter>())
                                target.gameObject.GetComponent<EventExecuter>().Execute();
                        break;
                    case GenericActionKind.PlayWWiseAudioSource:
                        if (target != null)
                            if (target.gameObject.GetComponent<CeuxQuiRestent.Audio.WwiseAudioSource>())
                                target.gameObject.GetComponent<CeuxQuiRestent.Audio.WwiseAudioSource>().Play();
                        break;
                    case GenericActionKind.AppearLinkable:
                        if (target != null)
                            if (target.gameObject.GetComponent<CeuxQuiRestent.Interactables.Linkable>())
                                target.gameObject.GetComponent<CeuxQuiRestent.Interactables.Linkable>().AppearAnimation();
                        break;
                    case GenericActionKind.DisappearLinkable:
                        if (target != null)
                            if (target.gameObject.GetComponent<CeuxQuiRestent.Interactables.Linkable>())
                                target.gameObject.GetComponent<CeuxQuiRestent.Interactables.Linkable>().DisappearAnimation();
                        break;
                    case GenericActionKind.StartCinematicMode:
                        if (GameObject.FindGameObjectWithTag("Player") != null)
                            GameObject.FindGameObjectWithTag("Player").GetComponent<CeuxQuiRestent.Tutorial.Help>().StartCinematicMode();
                        break;
                    case GenericActionKind.EndCinematicMode:
                        if (GameObject.FindGameObjectWithTag("Player") != null)
                            GameObject.FindGameObjectWithTag("Player").GetComponent<CeuxQuiRestent.Tutorial.Help>().EndCinematicMode();
                        break;
                    default:
                        break;
                }
                done = true;
            }
        }

        // Getters & Setters
        public GenericActionKind GetActionKind()
        {
            return actionKind;
        }

        public void SetActionKind(GenericActionKind _actionKind)
        {
            actionKind = _actionKind;
        }

        public GameObject GetTarget()
        {
            return target;
        }

        public void SetTarget(GameObject _target)
        {
            target = _target;
        }

        public float GetSecondsToWait()
        {
            return secondsToWait;
        }

        public void SetSecondsToWait(float _secondsToWait)
        {
            secondsToWait = _secondsToWait;
        }

        public bool IsDone()
        {
            return done;
        }
    }

}
