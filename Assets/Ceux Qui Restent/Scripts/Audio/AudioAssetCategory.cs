using System.Collections.Generic;

namespace CeuxQuiRestent.Audio
{
    [System.Serializable]
    public class AudioAssetCategory
    {
        public string categoryName = "New Asset Category";
        public List<AudioAsset> audioAssets = new List<AudioAsset>();
    }
}

