using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    [CreateAssetMenu(fileName = "New Audio Repository", menuName = "CeuxQuiRestent/Audio Repository")]
    public class AudioRepository : ScriptableObject
    {
        private List<AudioAssetCategory> audioAssetsCategories = new List<AudioAssetCategory>();

        public List<AudioAssetCategory> GetAudioAssetsCategories()
        {
            return audioAssetsCategories;
        }

        public void SetAudioAssetsCategories(List<AudioAssetCategory> _audioAssetsCategories)
        {
            audioAssetsCategories = _audioAssetsCategories;
        }
    }

}
