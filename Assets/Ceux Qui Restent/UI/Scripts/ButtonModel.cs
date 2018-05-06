using UnityEngine;

namespace CeuxQuiRestent.UI
{
    [CreateAssetMenu(fileName = "Button Model", menuName = "CeuxQuiRestent/Button Model")]
    public class ButtonModel : ScriptableObject
    {
        [Header("Main Settings")]
        public Sprite normal;
        public Sprite over;
        public TextMeshModel textModel;

        [Space]
        [Header("Scale Settings")]
        public bool changeScale = false;
        public Vector3 scale = new Vector3(0.05f, 0.05f, 1);
    }
}
