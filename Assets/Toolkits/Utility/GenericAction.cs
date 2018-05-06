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
        PlaySoundEffect,
        StartActionsExecuter,
        DisplaySubtitle
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
                        GameObject.Instantiate(target, master.position, Quaternion.identity, master);
                        break;
                    case GenericActionKind.Enable:
                        target.SetActive(true);
                        break;
                    case GenericActionKind.Disable:
                        target.SetActive(false);
                        break;
                    case GenericActionKind.EnableDisable:
                        target.SetActive(!target.activeSelf);
                        break;
                    case GenericActionKind.Destroy:
                        GameObject.Destroy(target);
                        break;
                    case GenericActionKind.PlayParticleSystem:
                        if (target.gameObject.GetComponent<ParticleSystem>())
                            target.gameObject.GetComponent<ParticleSystem>().Play();
                        break;
                    case GenericActionKind.PlaySoundEffect:
                        if (target.gameObject.GetComponent<AudioSource>())
                            target.gameObject.GetComponent<AudioSource>().Play();
                        break;
                    case GenericActionKind.StartActionsExecuter:
                        if (target.gameObject.GetComponent<ActionExecuter>())
                        {
                            target.gameObject.GetComponent<ActionExecuter>().ResetCounter();
                            target.gameObject.GetComponent<ActionExecuter>().StartActions();
                        }
                        break;
                    case GenericActionKind.DisplaySubtitle:
                        if (target.gameObject.GetComponent<CeuxQuiRestent.SubtitleHolder>())
                        {
                            CeuxQuiRestent.SubtitleHolder subtitleHolder = target.gameObject.GetComponent<CeuxQuiRestent.SubtitleHolder>();
                            GameObject subtitleDisplayer = GameObject.FindGameObjectWithTag("SubtitleDisplayer");
                            subtitleDisplayer.GetComponent<CeuxQuiRestent.SubtitleDisplayer>().DisplaySubtitle(subtitleHolder.subtitle, subtitleHolder.duration, subtitleHolder.soundName, subtitleHolder.gameObject);
                        }
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
