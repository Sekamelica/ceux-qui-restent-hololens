using UnityEngine;
using UnityEngine.UI;

namespace CeuxQuiRestent.Links
{
    [RequireComponent(typeof(Image))]
    public class EnergyBar : MonoBehaviour
    {
        #region Attributes
        [SerializeField]
        private RectTransform bar;
        [SerializeField]
        private Energy energy;
        [SerializeField]
        private Gradient gradient;
        [SerializeField]
        private float ease = 0.1f;
        private Image filler;
        private float startScaleX;
        private float energyLastFrame = 0;

        [SerializeField]
        private float increaseTime = 3;
        private float currentIncreaseTime = 0;
        private bool increase = false;
        private float previousRatio;

        [Space]
        [SerializeField]
        private float effect1_horizontalSpeedBase;
        [SerializeField]
        private float effect1_verticalSpeedBase;
        [SerializeField]
        private float effect1_horizontalSpeedMax;
        [SerializeField]
        private float effect1_verticalSpeedMax;
        private bool increaseEffect = false;
        [SerializeField]
        private float increaseEffectTime = 2;
        private float currentIncreaseEffectTime = 0;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            startScaleX = bar.localScale.x;
            filler = GetComponent<Image>();
        }

        void Update()
        {
            
            //filler.material.GetFloat("_HorizontalSpeed");
            if (increaseEffect)
            {
                currentIncreaseEffectTime += Time.deltaTime;
                filler.material.SetFloat("_HorizontalSpeed", Mathf.Lerp(filler.material.GetFloat("_HorizontalSpeed"), effect1_horizontalSpeedMax, 0.1f));
                filler.material.SetFloat("_VerticalSpeed", Mathf.Lerp(filler.material.GetFloat("_VerticalSpeed"), effect1_verticalSpeedMax, 0.1f));
                if (currentIncreaseEffectTime > increaseEffectTime)
                {
                    currentIncreaseTime = 0;
                    increaseEffect = false;
                }
            }
            else
            {
                filler.material.SetFloat("_HorizontalSpeed", Mathf.Lerp(filler.material.GetFloat("_HorizontalSpeed"), effect1_horizontalSpeedBase, 0.01f));
                filler.material.SetFloat("_VerticalSpeed", Mathf.Lerp(filler.material.GetFloat("_VerticalSpeed"), effect1_verticalSpeedBase, 0.01f));
            }
            
            if (energy.GetValue() < energyLastFrame)
            {
                increaseEffect = true;
                currentIncreaseEffectTime = 0;
            }


            float ratio = energy.GetEnergyLevel() / energy.GetStartingEnergyLevel();
            filler.color = gradient.Evaluate(Mathf.Clamp01(filler.fillAmount));
            if (!increase)
            {
                if (ratio != previousRatio)
                {
                    currentIncreaseTime = 0;
                    increase = true;
                }
            }


            if (increase)
            {
                currentIncreaseTime += Time.deltaTime;
                bar.localScale = new Vector3(Mathf.Lerp(startScaleX * previousRatio, startScaleX * ratio, currentIncreaseTime / increaseTime), bar.localScale.y, bar.localScale.z);
                if (currentIncreaseTime >= increaseTime)
                {
                    increase = false;
                }
            }
            else
            {
                bar.localScale = new Vector3(startScaleX * ratio, bar.localScale.y, bar.localScale.z);
            }


            filler.fillAmount = Mathf.Lerp(filler.fillAmount, energy.GetValue() / energy.GetEnergyLevel(), ease);
            if (!increase)
                previousRatio = ratio;

            energyLastFrame = energy.GetValue();
        }
        #endregion
    }

}