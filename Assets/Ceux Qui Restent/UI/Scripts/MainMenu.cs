using UnityEngine;
using UnityEngine.Events;

namespace CeuxQuiRestent.UI
{
    public class MainMenu : MonoBehaviour
    {
        #region Attributes
        public UnityEvent play_event;
        #endregion

        #region Methods
        public void Play()
        {
            if (play_event != null)
                play_event.Invoke();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}

