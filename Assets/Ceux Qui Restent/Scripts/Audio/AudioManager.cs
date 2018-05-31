using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CeuxQuiRestent.Audio
{
    public class PlayingRTPC
    {
        public int wwiseEventID;
        public uint postedEventID;
        public string rtpcName;
        public GameObject sender;
        public float closest;
        public float farthest;

        public PlayingRTPC(int _wwiseEventID, uint _postedEventID, string _rtpcName, GameObject _sender, float _closest, float _farthest)
        {
            wwiseEventID = _wwiseEventID;
            postedEventID = _postedEventID;
            rtpcName = _rtpcName;
            sender = _sender;
            closest = _closest;
            farthest = _farthest;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        #region Attributes
        public AudioRepository audioRepository;

        private Text subtitleDisplayer;
        private float textDuration;
        private GameObject lastStoredVoiceline_object;
        private GameObject player;
        private List<PlayingRTPC> playingRTCPs = new List<PlayingRTPC>();
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            subtitleDisplayer = GameObject.FindGameObjectWithTag("SubtitleDisplayer").GetComponent<Text>();
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
                AkSoundEngine.SetRTPCValue(playingRTCPs[pr].rtpcName, DistanceToRTPCValue(distance, playingRTCPs[pr].closest, playingRTCPs[pr].farthest));
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
                        case AudioType.Voiceline_Help:
                            if (lastStoredVoiceline_object != null || textDuration > -2f)
                                return 0;
                            else
                            {
                                if (audioAsset.subtitle != "" && audioAsset.subtitleDuration > 0)
                                {
                                    subtitleDisplayer.text = audioAsset.subtitle;
                                    textDuration = audioAsset.subtitleDuration;
                                }
                                lastStoredVoiceline_object = sender;
                                return audioAsset.wwiseEvent.Post(sender);
                            }
                        case AudioType.MemoryVoice:
                            Debug.LogWarning("Use PlayRTPCAudioAsset() instead for play Memory Voices.");
                            break;
                        default:
                            break;
                    }
                }
            }

            return 0;
        }

        public uint PlayRTPCAudioAsset(GameObject sender, int _categoryID, string _audioAssetID, float closest, float farthest)
        {
            AudioAsset audioAsset = audioRepository.FindAudioAsset(_categoryID, _audioAssetID);
            float distance = Vector3.Distance(player.transform.position, sender.transform.position);
            uint postedEventID = audioAsset.wwiseEvent.Post(sender);
            playingRTCPs.Add(new PlayingRTPC(audioAsset.wwiseEvent.ID, postedEventID, audioAsset.editorNotes, sender, closest, farthest));
            AkSoundEngine.SetRTPCValue(audioAsset.editorNotes, DistanceToRTPCValue(distance, closest, farthest));
            return postedEventID;
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

        public float DistanceToRTPCValue(float _distance, float closest, float farthest)
        {
            float rtpcValue = Remap(_distance, closest, farthest, 100.0f, 0.0f);
            rtpcValue = Mathf.Clamp(rtpcValue, 0.0f, 100.0f);
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
