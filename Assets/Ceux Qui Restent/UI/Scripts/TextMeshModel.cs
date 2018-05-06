using UnityEngine;

namespace CeuxQuiRestent.UI
{
    [CreateAssetMenu(fileName = "New TextMesh Model", menuName = "CeuxQuiRestent/TextMesh Model")]
    public class TextMeshModel : ScriptableObject
    {
        [Header("Main Settings")]
        public int fontSize = 50;
        public float characterSize = 0.03f;
        public float offsetZ = -0.01f;

        [Space]
        [Header("Font")]
        public FontSettings fontSettings;

        [Space]
        [Header("Anchor and Alignments")]
        public bool changeAnchorAndAligment = false;
        public TextAnchor anchor = TextAnchor.MiddleCenter;
        public TextAlignment alignment = TextAlignment.Center;

        [Space]
        [Header("Object Scaling")]
        public bool changeScale = false;
        public Vector3 scale = Vector3.one;
    }
}
