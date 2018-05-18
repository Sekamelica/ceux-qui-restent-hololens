using UnityEngine;
using System.Collections.Generic;
using CeuxQuiRestent.Interactables;

namespace CeuxQuiRestent.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Focusable))]
    [RequireComponent(typeof(Interactible))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParametricButton : MonoBehaviour
    {
        #region Attributes
        [Tooltip("The model (style) of the Button.")]
        public ButtonModel model;

        private SpriteRenderer spriteRenderer;
        private List<ParametricTextMesh> parametricTextMeshes = new List<ParametricTextMesh>();
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Awake()
        {
            UpdateVisuals();
        }

#if UNITY_EDITOR
        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying)
                UpdateVisuals();
        }
#endif
        #endregion

        #region Methods
        public void GetComponents()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            parametricTextMeshes.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.GetComponent<TextMesh>() && child.GetComponent<MeshRenderer>())
                {
                    if (child.GetComponent<ParametricTextMesh>())
                        parametricTextMeshes.Add(child.GetComponent<ParametricTextMesh>());
                    else
                        parametricTextMeshes.Add(child.AddComponent<ParametricTextMesh>());
                }
            }
        }

        public void UpdateVisuals()
        {
            GetComponents();
            if (model != null)
            {
                // Sprite
                spriteRenderer.sprite = model.normal;

                // Scale
                if (model.changeScale)
                {
                    if (transform.parent != null)
                    {
                        if (transform.lossyScale != model.scale)
                        {
                            Vector3 diviser = Vector3.one;
                            Transform buttonParent = transform.parent;
                            diviser = new Vector3(diviser.x * buttonParent.localScale.x, diviser.y * buttonParent.localScale.y, diviser.z * buttonParent.localScale.z);
                            while (buttonParent.parent != null)
                            {
                                buttonParent = buttonParent.parent;
                                diviser = new Vector3(diviser.x * buttonParent.localScale.x, diviser.y * buttonParent.localScale.y, diviser.z * buttonParent.localScale.z);
                            }
                            if (diviser.x != 0 && diviser.y != 0 && diviser.z != 0)
                                transform.localScale = new Vector3(model.scale.x / diviser.x, model.scale.y / diviser.y, model.scale.z / diviser.z);
                        }
                    }
                    else
                        transform.localScale = model.scale;
                }

                // Text
                if (model.textModel != null)
                {
                    for (int i = 0; i < parametricTextMeshes.Count; i++)
                    {
                        parametricTextMeshes[i].model = model.textModel;
                        parametricTextMeshes[i].UpdateVisuals();
                    }
                }
            }
        }

        public void OnFocusEnter()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (model != null && spriteRenderer != null)
                spriteRenderer.sprite = model.over;
        }

        public void OnFocusExit()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (model != null && spriteRenderer != null)
                spriteRenderer.sprite = model.normal;
        }
        #endregion
    }

}