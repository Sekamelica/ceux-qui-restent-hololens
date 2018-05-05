﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AK;

namespace CeuxQuiRestent
{
    [RequireComponent(typeof(TextMesh))]
    public class SubtitleDisplayer : MonoBehaviour
    {
        private TextMesh text;
        private float textDuration;

        private void Start()
        {
            text = GetComponent<TextMesh>();
        }

        private void Update()
        {
            textDuration -= Time.deltaTime;
            if (textDuration <= 0)
                text.text = "";
        }

        public void DisplaySubtitle(string subtitle, float time, string soundName, GameObject origin)
        {
            text.text = subtitle;
            textDuration = time;
            if (soundName != null && soundName != "")
                AkSoundEngine.PostEvent(soundName, origin);
        }
    }

}
