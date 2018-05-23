using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Utility;
using CeuxQuiRestent.Links;

namespace CeuxQuiRestent.Interactables
{
    [System.Serializable]
    [RequireComponent(typeof(Focusable))]
    [RequireComponent(typeof(Collider))]
    public class Linkable : MonoBehaviour, IInputClickHandler
    {
        #region Attributes
        // Public Attributes
        public Linkable pair;
        public ActionExecuter actionsToDo;
        public Energy energy;

        public MeshRenderer model;
        public Material material;
        public float appearDisappearAnimationTime = 2;
        public float changeModelAnimationTime = 3;
        
        // Private attributes        
        private bool alreadyLinked = false;
        private Linker linker;

        private float currentAnimationTime = 0;
        private bool animate = false;
        private bool appear = true;
        private float normalGradientTreshold = 2.9f;
        private float previousModelNormalGradientThreshold = 2.9f;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            StopHover();
            normalGradientTreshold = model.material.GetFloat("_GradientThreshold");
            AppearAnimation();
            linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
        }

        void Update()
        {
            if (animate)
            {
                currentAnimationTime += Time.deltaTime;
                float ease = Mathf.Clamp01(currentAnimationTime / appearDisappearAnimationTime);
                float gradientThresholdValue = (appear ? Mathf.Lerp(0, normalGradientTreshold, ease) : Mathf.Lerp(normalGradientTreshold, 0, ease));
                model.material.SetFloat("_GradientThreshold", gradientThresholdValue);
                if (currentAnimationTime >= appearDisappearAnimationTime)
                    animate = false;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (pair != null)
            {
                // Draw Link
                Debug.DrawLine(transform.position, pair.gameObject.transform.position, Color.yellow, 0.2f);

                // Label Energy
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.yellow;
                style.fontSize = 16;
                Vector3 labelPosition = Vector3.Lerp(gameObject.transform.position, pair.gameObject.transform.position, 0.5f);
                float energyCost = Vector3.Distance(gameObject.transform.position, pair.gameObject.transform.position);
                energyCost = ((float)((int)(energyCost * 100)) / 100.0f);
                string energyStr = "";
                if (energy != null)
                {
                    float[] energyLevels = energy.GetEnergyLevels();
                    for (int i = 0; i < energyLevels.Length && energyStr == ""; i++)
                        if (energyCost < energyLevels[i])
                            energyStr = "\nPalier n° " + i.ToString();
                    if (energyStr == "")
                    {
                        energyStr = "\nImpossible";
                        style.normal.textColor = Color.red;
                    }
                }
                UnityEditor.Handles.Label(labelPosition, energyCost.ToString() + " energy" + energyStr, style);
            }
        }
#endif
        #endregion

        #region Input Management
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Interact();
        }
        #endregion

        #region Linkable Methods
        public void AppearAnimation()
        {
            currentAnimationTime = 0;
            appear = true;
            animate = true;
        }

        public void DisappearAnimation()
        {
            currentAnimationTime = 0;
            appear = false;
            animate = true;
        }

        /// <summary>
        /// Called when you try to interact with this Linkable.
        /// </summary>
        public void Interact()
        {
            if (!alreadyLinked && pair != null)
                linker.LinkableClick(transform.position, gameObject, pair.gameObject);
        }

        /// <summary>
        /// When the linkable is linked to his pair.
        /// </summary>
        public void Linked()
        {
            alreadyLinked = true;
            if (actionsToDo != null)
            {
                actionsToDo.ResetCounter();
                actionsToDo.StartActions();
            }
        }

        public void ChangeModel(MeshRenderer _model)
        {
            model = _model;
            if (model != null)
                model.material = material;
        }
        public void ChangeMaterial(Material _material)
        {
            material = _material;
            if (model != null)
                model.material = material;
        }

        public void StartHover()
        {
            float shineValue = 1.8f;
            model.material.SetFloat("_Shine_R", shineValue);
            model.material.SetFloat("_Shine_G", shineValue);
            model.material.SetFloat("_Shine_B", shineValue);
            model.material.SetFloat("_Shine2_R", shineValue);
            model.material.SetFloat("_Shine2_G", shineValue);
            model.material.SetFloat("_Shine2_B", shineValue);
        }

        public void StopHover()
        {
            float shineValue = 0.4f;
            model.material.SetFloat("_Shine_R", shineValue);
            model.material.SetFloat("_Shine_G", shineValue);
            model.material.SetFloat("_Shine_B", shineValue);
            model.material.SetFloat("_Shine2_R", shineValue);
            model.material.SetFloat("_Shine2_G", shineValue);
            model.material.SetFloat("_Shine2_B", shineValue);
        }
        #endregion

        #region Getters & Setters
        public bool IsAlreadyLinked()
        {
            return alreadyLinked;
        }

        public bool CanBeLinked()
        {
            return !alreadyLinked;
        }
        #endregion
    }

}