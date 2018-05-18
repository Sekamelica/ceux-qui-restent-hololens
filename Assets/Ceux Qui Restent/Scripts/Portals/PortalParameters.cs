using UnityEngine;

namespace CeuxQuiRestent.Portals
{
    [CreateAssetMenu(fileName = "New Portal Parameters", menuName = "CeuxQuiRestent/Portal Parameters")]
    public class PortalParameters : ScriptableObject
    {
        [Header("RenderTexture Materials")]
        public Material futureMaterial;
        public Material pastMaterial;
    }
}
