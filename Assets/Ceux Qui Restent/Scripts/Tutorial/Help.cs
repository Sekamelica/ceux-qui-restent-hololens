using UnityEngine;
using Utility;

namespace CeuxQuiRestent.Tutorial
{
    public class Help : MonoBehaviour
    {
        #region Attributes
        [Space]
        [Header("Help - Don't move")]
        public float dontMoveTime = 20;
        public float dontMoveDistanceThreshold = 1f;
        public ActionExecuter dontMoveAction;

        private float currentDontMoveTime = 0;
        private float dontMoveTotalDistance = 0;
        private Vector3 dontMovePositionLastFrame;

        [Space]
        [Header("Help - Don't Click")]
        public ActionExecuter dontClickAction;
        public float dontClickTime = 15;

        private float dontClickTime_current = 0;
        private bool isLinking = false;

        [Space]
        [Header("Help - Click")]
        public ActionExecuter clickLinkable_ButTooFar;
        public ActionExecuter clickLinkable_ButWrongPair;
        public ActionExecuter clickLinkable_ButAlreadyLinked;
        public ActionExecuter clickLinkable_Valid;

        [Space]
        [Header("Help - Links")]
        public ActionExecuter linkBrokeAction;
        public float linkBrokeMinimumWait = 20;
        public float linkBrokeMinimumHappens = 2;

        private int currentLinkBrokes = 0;
        private float linkBrokeMinimumWait_current = 20;

        [Space]
        [Header("Help - Energy")]
        public ActionExecuter energyEmpty;
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
                if (!isLinking)
                {
                    dontClickTime_current += Time.deltaTime;
                    if (dontClickTime_current >= dontClickTime)
                    {
                        if (dontClickAction != null)
                            dontClickAction.StartActions();
                        dontClickTime_current = 0;
                    }
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
            if (dontMoveAction != null)
                dontMoveAction.SetStarted(true);
            currentDontMoveTime = 0;
        }

        // Click Helpers
        public void ClickLinkable_Valid()
        {
            if (clickLinkable_Valid != null)
                clickLinkable_ButTooFar.SetStarted(true);
        }

        public void ClickLinkable_TooFar()
        {
            if (clickLinkable_ButTooFar != null)
                clickLinkable_ButTooFar.SetStarted(true);
        }

        public void ClickLinkable_WrongPair()
        {
            if (clickLinkable_ButWrongPair != null)
                clickLinkable_ButTooFar.SetStarted(true);
        }

        public void ClickLinkable_AlreadyLinked()
        {
            if (clickLinkable_ButAlreadyLinked != null)
                clickLinkable_ButAlreadyLinked.SetStarted(true);
        }

        // Link Helpers
        public void LinkIntersectAndBroke()
        {
            currentLinkBrokes++;
            if (currentLinkBrokes == 1 || ((currentLinkBrokes - 1) % linkBrokeMinimumHappens == 0 && linkBrokeMinimumWait_current == 0))
                if (linkBrokeAction != null)
                    linkBrokeAction.SetStarted(true);
        }

        // Energy Helpers
        public void EnergyEmpty()
        {
            currentEnergyEmptyAmount++;
            if (currentEnergyEmptyAmount == 1 || ((currentEnergyEmptyAmount - 1) % energyEmptyMinimumHappens == 0 && energyEmptyMinimumWait_current == 0))
                if (energyEmpty != null)
                    energyEmpty.SetStarted(true);
        }
        #endregion

        #region Getters & Setters
        public void SetIsLinking(bool _isLinking)
        {
            isLinking = _isLinking;
            dontClickTime = 0;
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
