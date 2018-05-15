using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class EventExecuter : MonoBehaviour
    {
        #region Attributes
        public bool autoStart = false;
        public UnityEvent eventToExecute;
        #endregion

        #region Methods
        private void Start()
        {
            if (autoStart)
                Execute();
        }

        public void Execute()
        {
            if (eventToExecute != null)
                eventToExecute.Invoke();
        }
        #endregion
    }
}
