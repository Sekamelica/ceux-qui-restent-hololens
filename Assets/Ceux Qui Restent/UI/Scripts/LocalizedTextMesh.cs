using UnityEngine;

namespace CeuxQuiRestent.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMesh))]
    public class LocalizedTextMesh : MonoBehaviour
    {
        #region Attributes
        public Language language;
        public string textFR;
        public string textEN;

        private TextMesh textMesh;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            textMesh = GetComponent<TextMesh>();
            if (language != null)
                textMesh.text = (language.language == LanguageName.Francais) ? textFR : textEN;
            else
                textMesh.text = textFR;
        }

#if UNITY_EDITOR
        void Update()
        {
            textMesh = GetComponent<TextMesh>();
            if (language != null)
                textMesh.text = (language.language == LanguageName.Francais) ? textFR : textEN;
            else
                textMesh.text = textFR;
        }
#endif
        #endregion
    }
}
