using UnityEngine;

namespace CeuxQuiRestent.UI
{
    public enum LanguageName
    {
        Francais,
        English
    }

    [CreateAssetMenu(fileName = "New Language", menuName = "CeuxQuiRestent/Language")]
    public class Language : ScriptableObject
    {
        public LanguageName language;

        public void SetLanguage(LanguageName _language)
        {
            language = _language;
        }
    }
}

