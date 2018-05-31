using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    public class AudioPlayAtRange : MonoBehaviour
    {
        #region Attributes
        public float range = 3;

        private Transform player;
        private bool done = false;
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (done)
                return;

            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").transform;

            if (Vector3.Distance(transform.position, player.position) < range)
            {
                WwiseAudioSource was = gameObject.GetComponent<WwiseAudioSource>();
                if (was != null)
                    was.Play();
                done = true;
            }
        }
        #endregion

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, range);
        }
#endif
    }
}

