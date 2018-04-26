using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [System.Serializable]
    public class ActionExecuter : MonoBehaviour
    {
        // Attributes
        [SerializeField]
        private string actionsName = "Actions List";
        [SerializeField]
        private bool started = false;
        [SerializeField]
        private float secondsToWait = 0;
        [SerializeField]
        private bool loop = false;
        [SerializeField]
        private List<GenericAction> actions = new List<GenericAction>();

        private float seconds = 0;

        private void Start()
        {
            foreach (GenericAction action in actions)
            {
                action.SaveSecondsToWaitOrigin();
            }
        }

        void Update()
        {
            if(started)
            {
                if (seconds < secondsToWait)
                    seconds += Time.deltaTime;
            	if(seconds >= secondsToWait)
            	{
                    bool allActionsAreDone = true;
                    for (int a = 0; a < actions.Count; a++)
                    {
                        if (a > 0)
                        {
                            if (actions[a - 1].IsDone() && !actions[a].IsDone())
                                actions[a].Execute(transform);
                        }
                        else if (!actions[a].IsDone())
                            actions[a].Execute(transform);
                        if (!actions[a].IsDone())
                            allActionsAreDone = false;
                    }
                    if (allActionsAreDone)
                    {
                        StopActions();
                        if (loop)
                            started = true;
                    }
            	}
	    }
        }

        #region Methods
        public void StartActions()
        {
            seconds = 0;
            started = true;
        }

        public void StopActions()
        {
            seconds = 0;
            started = false;
            foreach (GenericAction action in actions)
            {
                action.ResetSecondsToWait();
                action.ResetIsDone();
            }
        }

        public void ResetCounter()
        {
            seconds = 0;
        }
        #endregion

        #region Getters & Setters
        public string GetActionsName()
        {
            return actionsName;
        }

        public void SetActionsName(string _actionsName)
        {
            actionsName = _actionsName;
        }

        public bool GetStarted()
        {
            return started;
        }

        public void SetStarted(bool _started)
        {
            started = _started;
        }

        public float GetSecondsToWait()
        {
            return secondsToWait;
        }

        public void SetSecondsToWait(float _secondsToWait)
        {
            secondsToWait = _secondsToWait;
        }

        public bool GetLoop()
        {
            return loop;
        }

        public void SetLoop(bool _loop)
        {
            loop = _loop;
        }

        public List<GenericAction> GetActions()
        {
            return actions;
        }

        public void SetActions(List<GenericAction> _actions)
        {
            actions = _actions;
        }
        #endregion
    }

}
