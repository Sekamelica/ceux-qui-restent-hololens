namespace CeuxQuiRestent.Audio
{
    [System.Serializable]
    public class AudioAsset
    {
        #region Attributes
        public AudioType type = AudioType.Voiceline_Narrator;
        public string WwiseEventName = "";
        public string subtitle = "";
        public float subtitleDuration = 1;
        
        public bool editorDisplay = true;
        public string editorNotes = "";
        #endregion
    }

}
