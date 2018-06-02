namespace CeuxQuiRestent.Audio
{
    [System.Serializable]
    public class AudioAsset
    {
        #region Attributes
        public string uniqueID = "";

        public AudioType type = AudioType.Voiceline_Narrator;
        public AK.Wwise.Event wwiseEvent = null;
        public string subtitleFR = "";
        public string subtitleEN = "";
        public float subtitleDuration = 1;

        public bool editorDisplay = true;
        public string editorNotes = "";
        #endregion

        #region Constructor(s)
        public AudioAsset()
        {
            uniqueID = AudioUtility.GetUniqueID();
        }
        #endregion
    }

}
