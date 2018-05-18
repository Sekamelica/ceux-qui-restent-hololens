using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK;

namespace CeuxQuiRestent
{
    public class SubtitleHolder : MonoBehaviour
    {
        #region Attributes
        [Multiline]
        public string subtitle = "Text";
        public float duration = 1;
        public string soundName = "";
        #endregion

        #region Methods
        public void Play()
        {
            GameObject subtitleDisplayer = GameObject.FindGameObjectWithTag("SubtitleDisplayer");
            subtitleDisplayer.GetComponent<SubtitleDisplayer>().DisplaySubtitle(subtitle, duration, soundName, gameObject);
        }
        #endregion
    }
}

