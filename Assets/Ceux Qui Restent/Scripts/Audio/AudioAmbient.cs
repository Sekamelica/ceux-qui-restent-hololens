using CeuxQuiRestent.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    public class AudioAmbient : MonoBehaviour
    {
        #region Attributes
        public float farthestDistance = 4;
        public float closestDistance = 1;
        public WwiseAudioSource sound;
        private uint postedEventID;
        private bool playing = false;
        private Transform player;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void FixedUpdate()
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= farthestDistance && !playing)
            {
                if (sound != null)
                    postedEventID = sound.PlayRTPC(closestDistance, farthestDistance);
                playing = true;
            }
            if (distance > farthestDistance)
            {
                if (sound != null && postedEventID != 0)
                    sound.Stop(postedEventID);
                playing = false;
            }
        }
        #endregion

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, closestDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, farthestDistance);
        }
#endif
    }

}
