using UnityEngine;

namespace CeuxQuiRestent.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(TextMesh))]
    public class ParametricTextMesh : MonoBehaviour
    {

        [Tooltip("The model (style) of the TextMesh.")]
        public TextMeshModel model;

        private TextMesh textMesh;
        private MeshRenderer meshRenderer;

        // Use this for initialization
        void Awake()
        {
            GetComponents();
            UpdateVisuals();
        }

#if UNITY_EDITOR
        // Update is called once per frame
        void Update()
        {
            GetComponents();
            UpdateVisuals();
        }
#endif

        public void GetComponents()
        {
            textMesh = GetComponent<TextMesh>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void UpdateVisuals()
        {
            GetComponents();
            if (model != null)
            {
                // Main Settings
                textMesh.offsetZ = model.offsetZ;
                textMesh.characterSize = model.characterSize;
                textMesh.fontSize = model.fontSize;

                // Font
                if (model.fontSettings != null)
                {
                    textMesh.font = model.fontSettings.font;
                    meshRenderer.material = model.fontSettings.fontMaterial;
                }

                // Anchor & Alignment
                if (model.changeAnchorAndAligment)
                {
                    textMesh.anchor = model.anchor;
                    textMesh.alignment = model.alignment;
                }

                // Scale
                if (model.changeScale)
                {
                    if (transform.parent != null)
                    {
                        if (transform.lossyScale != model.scale)
                        {
                            Vector3 diviser = Vector3.one;
                            Transform textMeshParent = transform.parent;
                            diviser = new Vector3(diviser.x * textMeshParent.localScale.x, diviser.y * textMeshParent.localScale.y, diviser.z * textMeshParent.localScale.z);
                            while (textMeshParent.parent != null)
                            {
                                textMeshParent = textMeshParent.parent;
                                diviser = new Vector3(diviser.x * textMeshParent.localScale.x, diviser.y * textMeshParent.localScale.y, diviser.z * textMeshParent.localScale.z);
                            }
                            if (diviser.x != 0 && diviser.y != 0 && diviser.z != 0)
                                transform.localScale = new Vector3(model.scale.x / diviser.x, model.scale.y / diviser.y, model.scale.z / diviser.z);
                        }
                    }
                    else
                        transform.localScale = model.scale;
                }
            }
        }
    }

}