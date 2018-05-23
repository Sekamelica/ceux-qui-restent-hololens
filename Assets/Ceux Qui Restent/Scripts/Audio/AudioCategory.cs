using System.Collections.Generic;

namespace CeuxQuiRestent.Audio
{
    [System.Serializable]
    public class AudioCategory
    {
        #region Attributes
        public string categoryName = "New Audio Category";
        public List<AudioAsset> audioAssets = new List<AudioAsset>();

        public bool editorDisplayEvents = true;
        public bool editorDisplayAssets = true;
        #endregion
    }
}

