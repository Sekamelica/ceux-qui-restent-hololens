using UnityEngine;
using CeuxQuiRestent.Audio;

namespace CeuxQuiRestent.Tutorial
{
    public class Help : MonoBehaviour
    {
        #region Attributes
        [Space]
        [Header("Help - Don't move")]
        public float dontMoveTime = 20;
        public float dontMoveDistanceThreshold = 1f;
        public WwiseAudioSource dontMove;
        public WwiseAudioSource dontMoveLinking;

        private float currentDontMoveTime = 0;
        private float dontMoveTotalDistance = 0;
        private Vector3 dontMovePositionLastFrame;

        [Space]
        [Header("Help - Don't Click")]
        public WwiseAudioSource dontClick;
        public float dontClickTime = 15;

        private float dontClickTime_current = 0;
        private bool isLinking = false;

        [Space]
        [Header("Help - Click")]
        public WwiseAudioSource clickLinkable_ButTooFar;
        public WwiseAudioSource clickLinkable_ButWrongPair;
        public WwiseAudioSource clickLinkable_ButAlreadyLinked;
        public WwiseAudioSource clickLinkable_Valid;

        [Space]
        [Header("Help - Links")]
        public WwiseAudioSource linkBroke;
        public float linkBrokeMinimumWait = 20;
        public float linkBrokeMinimumHappens = 2;

        private int currentLinkBrokes = 0;
        private float linkBrokeMinimumWait_current = 20;

        [Space]
        [Header("Help - Energy")]
        public WwiseAudioSource energyEmpty;
        public float energyEmptyMinimumWait = 20;
        public float energyEmptyMinimumHappens = 2;

        private int currentEnergyEmptyAmount = 0;
        private float energyEmptyMinimumWait_current = 0;

        private bool cinematicMode = true;
        #endregion

        #region MonoBehaviour Methods
        void Update()
        {
            if (!cinematicMode)
            {
                // Don't Move
                dontMoveTotalDistance += Vector3.Distance(dontMovePositionLastFrame, transform.position);
                if (dontMoveTotalDistance <= dontMoveDistanceThreshold)
                    currentDontMoveTime += Time.deltaTime;
                else
                {
                    currentDontMoveTime = 0;
                    dontMoveTotalDistance = 0;
                }
                if (currentDontMoveTime >= dontMoveTime)
                {
                    DontMove();
                    currentDontMoveTime = 0;
                    dontMoveTotalDistance = 0;
                }
                dontMovePositionLastFrame = transform.position;

                // Don't Click
                dontClickTime_current += Time.deltaTime;
                if (dontClickTime_current >= dontClickTime)
                {
                    if (isLinking)
                    {
                        if (dontMoveLinking != null)
                            dontMoveLinking.Play();
                        currentDontMoveTime = 0;
                    }
                    else
                    {
                        if (dontClick != null)
                            dontClick.Play();
                    }
                    
                    dontClickTime_current = 0;
                }
            }
            
            // Link Broke timer
            if (linkBrokeMinimumWait_current > 0)
            {
                linkBrokeMinimumWait_current -= Time.deltaTime;
                if (linkBrokeMinimumWait_current < 0)
                    linkBrokeMinimumWait_current = 0;
            }

            // Energy Empty timer
            if (energyEmptyMinimumWait_current > 0)
            {
                energyEmptyMinimumWait_current -= Time.deltaTime;
                if (energyEmptyMinimumWait_current < 0)
                    energyEmptyMinimumWait_current = 0;
            }

        }
        #endregion

        #region Helpers Methods
        // Move Helpers
        public void DontMove()
        {
            if (isLinking)
            {
                if (dontMoveLinking != null)
                    dontMoveLinking.Play();
            }
            else
            {
                if (dontMove != null)
                    dontMove.Play();
            }
            currentDontMoveTime = 0;
        }

        // Click Helpers
        public void ClickLinkable_Valid()
        {
            if (clickLinkable_Valid != null)
                clickLinkable_Valid.Play();
        }

        public void ClickLinkable_TooFar()
        {
            if (clickLinkable_ButTooFar != null)
                clickLinkable_ButTooFar.Play();
        }

        public void ClickLinkable_WrongPair()
        {
            if (clickLinkable_ButWrongPair != null)
                clickLinkable_ButWrongPair.Play();
        }

        public void ClickLinkable_AlreadyLinked()
        {
            if (clickLinkable_ButAlreadyLinked != null)
                clickLinkable_ButAlreadyLinked.Play();
        }

        // Link Helpers
        public void LinkIntersectAndBroke()
        {
            currentLinkBrokes++;
            if (currentLinkBrokes == 1 || ((currentLinkBrokes - 1) % linkBrokeMinimumHappens == 0 && linkBrokeMinimumWait_current == 0))
                if (linkBroke != null)
                    linkBroke.Play();
        }

        // Energy Helpers
        public void EnergyEmpty()
        {
            currentEnergyEmptyAmount++;
            if (currentEnergyEmptyAmount == 1 || ((currentEnergyEmptyAmount - 1) % energyEmptyMinimumHappens == 0 && energyEmptyMinimumWait_current == 0))
                if (energyEmpty != null)
                    energyEmpty.Play();
        }
        #endregion

        #region Getters & Setters
        public void SetIsLinking(bool _isLinking)
        {
            isLinking = _isLinking;
            dontClickTime_current = 0;
        }

        public void StartCinematicMode()
        {
            cinematicMode = true;
            currentDontMoveTime = 0;
            dontClickTime_current = 0;
        }

        public void EndCinematicMode()
        {
            cinematicMode = false;
            currentDontMoveTime = 0;
            dontClickTime_current = 0;
        }
        #endregion
    }
}
