using UnityEngine;

namespace CeuxQuiRestent.UI
{
    public class LanguageChooser : MonoBehaviour
    {
        #region Attributes
        public Language language;
        #endregion

        #region Setters
        public void SetLanguageFR()
        {
            language.SetLanguage(LanguageName.Francais);
        }

        public void SetLanguageEN()
        {
            language.SetLanguage(LanguageName.English);
        }
        #endregion
    }

}
