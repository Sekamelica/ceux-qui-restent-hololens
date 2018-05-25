using CeuxQuiRestent.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerAmbientAudio : MonoBehaviour {
    public WwiseAudioSource sound;
    private uint postedEventID;

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
}
