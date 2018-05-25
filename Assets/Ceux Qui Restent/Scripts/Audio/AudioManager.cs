using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    public class PlayingRTCP
    {
        public int wwiseEventID;
        public uint postedEventID;

        public PlayingRTCP(int _wwiseEventID, uint _postedEventID)
        {
            wwiseEventID = _wwiseEventID;
            postedEventID = _postedEventID;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        #region Attributes
        public AudioRepository audioRepository;
        public TextMesh subtitleDisplayer;

        private float textDuration;
        private GameObject lastStoredVoiceline_object;
        private GameObject player;
        private List<PlayingRTCP> playingRTCPs = new List<PlayingRTCP>();
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

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
                    switch (audioAsset.type)
                    {
                        case AudioType.Voiceline_Narrator:
                            if (audioAsset.subtitle != "" && audioAsset.subtitleDuration > 0)
                            {
                                subtitleDisplayer.text = audioAsset.subtitle;
                                textDuration = audioAsset.subtitleDuration;
                            }
                            if (lastStoredVoiceline_object != null)
                                AkSoundEngine.StopAll(lastStoredVoiceline_object);
                            audioAsset.wwiseEvent.Post(sender);
                            lastStoredVoiceline_object = sender;
                            break;
                        case AudioType.MemoryVoice:
                            float distance = Vector3.Distance(player.transform.position, sender.transform.position);
                            bool postEvent = true;
                            for (int pr = 0; pr < playingRTCPs.Count; pr++)
                            {
                                if (playingRTCPs[pr].wwiseEventID == audioAsset.wwiseEvent.ID)
                                {
                                    postEvent = false;
                                    AkSoundEngine.SetRTPCValue(playingRTCPs[pr].postedEventID, distance);
                                }
                            }
                            if (postEvent)
                            {
                                uint postedEventID = audioAsset.wwiseEvent.Post(sender);
                                playingRTCPs.Add(new PlayingRTCP(audioAsset.wwiseEvent.ID, postedEventID));
                                AkSoundEngine.SetRTPCValue(postedEventID, distance);
                            }
                            break;
                        default:
                            break;
                    }
                    
                    
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
