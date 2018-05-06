using UnityEngine;

namespace CeuxQuiRestent.UI
{
    [CreateAssetMenu(fileName = "New Font Settings", menuName = "CeuxQuiRestent/Font Settings")]
    public class FontSettings : ScriptableObject
    {
        [Header("Font Settings")]
        public Font font;
        public Material fontMaterial;
    }
}