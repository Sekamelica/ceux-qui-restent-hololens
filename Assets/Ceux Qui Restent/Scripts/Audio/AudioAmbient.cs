using CeuxQuiRestent.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbient : MonoBehaviour
{
    #region Attributes
    public float farthestDistance = 4;
    public float closestDistance = 1;
    public WwiseAudioSource sound;
    private uint postedEventID;
    #endregion

    #region MonoBehaviour Methods
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (sound != null)
                postedEventID = sound.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (sound != null && postedEventID != 0)
                sound.Stop(postedEventID);
        }
    }
    #endregion
}
