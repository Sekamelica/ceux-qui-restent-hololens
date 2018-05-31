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
        private bool stopped = false;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void FixedUpdate()
        {
            if (stopped)
                return;

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

        public void Stop()
        {
            if (sound != null && postedEventID != 0)
                sound.Stop(postedEventID);
            stopped = true;
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

        #region Debug Methods
        public void IncreaseClosestDistance()
        {
            closestDistance += 0.5f;
        }

        public void DecreaseClosestDistance()
        {
            closestDistance -= 0.5f;
        }

        public void IncreaseFarthestDistance()
        {
            farthestDistance += 0.5f;
        }

        public void DecreaseFarthestDistance()
        {
            farthestDistance -= 0.5f;
        }

        public float GetClosestDistance()
        {
            return closestDistance;
        }

        public float GetFarthestDistance()
        {
            return farthestDistance;
        }
        #endregion
    }

}
