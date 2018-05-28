using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    public class PlayingRTCP
    {
        public int wwiseEventID;
        public uint postedEventID;
        public GameObject sender;

        public PlayingRTCP(int _wwiseEventID, uint _postedEventID, GameObject _sender)
        {
            wwiseEventID = _wwiseEventID;
            postedEventID = _postedEventID;
            sender = _sender;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        #region Attributes
        public AudioRepository audioRepository;

        private TextMesh subtitleDisplayer;
        private float textDuration;
        private GameObject lastStoredVoiceline_object;
        private GameObject player;
        private List<PlayingRTCP> playingRTCPs = new List<PlayingRTCP>();
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            subtitleDisplayer = GameObject.FindGameObjectWithTag("SubtitleDisplayer").GetComponent<TextMesh>();
        }

        private void Update()
        {
            // Subtitles
            textDuration -= Time.deltaTime;
            if (textDuration <= 0)
                subtitleDisplayer.text = "";

            // RTCP
            for (int pr = 0; pr < playingRTCPs.Count; pr++)
            {
                float distance = Vector3.Distance(player.transform.position, playingRTCPs[pr].sender.transform.position);
                AkSoundEngine.SetRTPCValue((uint)playingRTCPs[pr].wwiseEventID, DistanceToRTPCValue(distance));
            }
        }
        #endregion

        #region Methods
        public uint PlayAudioAsset(GameObject sender, int _categoryID, string _audioAssetID)
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
                            lastStoredVoiceline_object = sender;
                            return audioAsset.wwiseEvent.Post(sender);
                        case AudioType.MemoryVoice:
                            float distance = Vector3.Distance(player.transform.position, sender.transform.position);
                            uint postedEventID = audioAsset.wwiseEvent.Post(sender);
                            playingRTCPs.Add(new PlayingRTCP(audioAsset.wwiseEvent.ID, postedEventID, sender));
                            AkSoundEngine.SetRTPCValue((uint)audioAsset.wwiseEvent.ID, DistanceToRTPCValue(distance));
                            return postedEventID;
                        default:
                            break;
                    }
                }
            }

            return 0;
        }

        public void StopEventID(uint _postedEventID)
        {
            for (int i = playingRTCPs.Count - 1; i >= 0; i--)
            {
                if (playingRTCPs[i].postedEventID == _postedEventID)
                {
                    AkSoundEngine.StopAll(playingRTCPs[i].sender);
                    playingRTCPs.RemoveAt(i);
                }
            }
        }

        public float DistanceToRTPCValue(float _distance)
        {
            float near = 1.0f;
            float far = 4.0f;
            float rtpcValue = Remap(_distance, near, far, 100.0f, 0.0f);
            rtpcValue = Mathf.Clamp(rtpcValue, 0.0f, 100.0f);
            Debug.Log("Distance: " + _distance + " | RTCP: " + rtpcValue);
            return rtpcValue;
        }

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public uint PlayWwiseEvent(GameObject sender, AK.Wwise.Event _event)
        {
            if (_event != null)
                return _event.Post(sender);
            return 0;
        }
        #endregion
    }
}
