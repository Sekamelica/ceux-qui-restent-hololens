using UnityEngine;
using UnityEngine.UI;
using CeuxQuiRestent.Gameplay;

namespace CeuxQuiRestent.UI
{
    [RequireComponent(typeof(Image))]
    public class EnergyBar : MonoBehaviour
    {
        #region Attributes
        [SerializeField]
        private RectTransform bar;
        [SerializeField]
        private Energy energy;
        private Image filler;
        private float startScaleX;

        [SerializeField]
        private float increaseTime = 3;
        private float currentIncreaseTime = 0;
        private bool increase = false;
        private float previousRatio;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            startScaleX = bar.localScale.x;
            filler = GetComponent<Image>();
        }

        void Update()
        {
            float ratio = energy.GetMaximum() / energy.GetEnergy();

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


            filler.fillAmount = energy.GetValue() / energy.GetMaximum();
            if (!increase)
                previousRatio = ratio;
        }
        #endregion
    }

}