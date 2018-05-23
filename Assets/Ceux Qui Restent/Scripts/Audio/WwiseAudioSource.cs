using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    [System.Serializable]
    public class WwiseAudioSource : MonoBehaviour
    {
        #region Attributes
        public bool playAtStart = false;
        public bool playAudioAsset = false;
        public AK.Wwise.Event wwiseEvent = null;
        public int categoryID = 0;
        public string audioAssetID = "";

        private AudioManager audioManager;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }

        private void Update()
        {
            if (playAtStart)
            {
                playAtStart = false;
                Play();
            }
        }
        #endregion

        #region Methods
        public void Play()
        {
            if (audioManager == null)
                audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
            if (playAudioAsset)
                audioManager.PlayAudioAsset(gameObject, categoryID, audioAssetID);
            else
                audioManager.PlayWwiseEvent(gameObject, wwiseEvent);
        }

        public void PlayEvent(AK.Wwise.Event _newEvent)
        {
            wwiseEvent = _newEvent;
            Play();
        }
        #endregion
    }
}
