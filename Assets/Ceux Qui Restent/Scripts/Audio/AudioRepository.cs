using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Audio
{
    [CreateAssetMenu(fileName = "New Audio Repository", menuName = "CeuxQuiRestent/Audio Repository")]
    public class AudioRepository : ScriptableObject
    {
        #region Attributes
        [SerializeField]
        private List<AudioCategory> audioCategories = new List<AudioCategory>();
        #endregion

        #region Methods
        public AudioAsset FindAudioAsset(int _categoryID, string _audioAssetID)
        {
            if (_categoryID >= 0 && _categoryID < audioCategories.Count)
                for (int awe = 0; awe < audioCategories[_categoryID].audioAssets.Count; awe++)
                    if (audioCategories[_categoryID].audioAssets[awe].uniqueID == _audioAssetID)
                        return audioCategories[_categoryID].audioAssets[awe];

            return null;
        }

        public List<string> FindCategoryNames()
        {
            List<string> categoryNames = new List<string>();

            for (int ac = 0; ac < audioCategories.Count; ac++)
                categoryNames.Add(audioCategories[ac].categoryName);

            return categoryNames;
        }

        public int FindEventIndex(string _eventID, List<string> _eventIDs)
        {
            for (int eID = 0; eID < _eventIDs.Count; eID++)
                if (_eventIDs[eID] == _eventID)
                    return eID;

            return 0;
        }

        public List<string> FindAudioAssetIDs(int _categoryID)
        {
            List<string> audioAssetIDs = new List<string>();

            if (_categoryID >= 0 && _categoryID < audioCategories.Count)
                for (int aa = 0; aa < audioCategories[_categoryID].audioAssets.Count; aa++)
                    audioAssetIDs.Add(audioCategories[_categoryID].audioAssets[aa].uniqueID);

            return audioAssetIDs;
        }

        public int FindAudioAssetIndex(string _audioAssetID, List<string> _audioAssetIDs)
        {
            for (int eID = 0; eID < _audioAssetIDs.Count; eID++)
                if (_audioAssetIDs[eID] == _audioAssetID)
                    return eID;

            return 0;
        }
        #endregion

        #region Getters & Setters
        public List<AudioCategory> GetAudioCategories()
        {
            return audioCategories;
        }

        public void SetAudioCategories(List<AudioCategory> _audioCategories)
        {
            audioCategories = _audioCategories;
        }
        #endregion
    }

}
