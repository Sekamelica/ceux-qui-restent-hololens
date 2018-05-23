using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Attributes
        public AudioRepository audioRepository;
        public TextMesh subtitleDisplayer;
        private float textDuration;
        private GameObject lastStoredVoiceline_object;
        #endregion

        #region MonoBehaviour Methods
        private void Update()
        {
            textDuration -= Time.deltaTime;
            if (textDuration <= 0)
                subtitleDisplayer.text = "";
        }
        #endregion

        #region Methods
        public void PlayAudioAsset(GameObject sender, int _categoryID, string _audioAssetID)
        {
            AudioAsset audioAsset = audioRepository.FindAudioAsset(_categoryID, _audioAssetID);

            if (audioAsset != null)
            {
                if (audioAsset.wwiseEvent != null)
                {
                    if (audioAsset.subtitle != "" && audioAsset.subtitleDuration > 0)
                    {
                        subtitleDisplayer.text = audioAsset.subtitle;
                        textDuration = audioAsset.subtitleDuration;
                    }
                    if (lastStoredVoiceline_object != null)
                        AkSoundEngine.StopAll(lastStoredVoiceline_object);
                    audioAsset.wwiseEvent.Post(sender);
                    lastStoredVoiceline_object = sender;
                }
            }
        }

        public void PlayWwiseEvent(GameObject sender, AK.Wwise.Event _event)
        {
            if (_event != null)
                _event.Post(sender);
        }
        #endregion
    }
}
