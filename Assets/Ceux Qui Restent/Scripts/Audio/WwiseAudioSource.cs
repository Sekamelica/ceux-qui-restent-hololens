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
        public bool autoRename = true;

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
        public uint Play()
        {
            if (audioManager == null)
                audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
            if (playAudioAsset)
                return audioManager.PlayAudioAsset(gameObject, categoryID, audioAssetID);
            else
                return audioManager.PlayWwiseEvent(gameObject, wwiseEvent);
        }

        public void PlayDelegate()
        {
            Play();
        }

        public void Stop(uint _postedEventID)
        {
            audioManager.StopEventID(_postedEventID);
        }

        public void PlayEvent(AK.Wwise.Event _newEvent)
        {
            wwiseEvent = _newEvent;
            Play();
        }
        #endregion
    }
}
