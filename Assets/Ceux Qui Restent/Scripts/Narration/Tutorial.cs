using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CeuxQuiRestent
{
    public class Tutorial : MonoBehaviour
    {
        // - - - Attributes - - - //
        public bool tutorialEnabled = true;

        [Space]
        [Header("Help - Don't move")]
        public float dontMoveTime = 10;
        public float dontMoveDistanceThreshold = 1f;
        public ActionExecuter dontMoveAction;

        private float currentDontMoveTime = 0;
        private float dontMoveTotalDistance = 0;
        private Vector3 dontMovePositionLastFrame;

        [Space]
        [Header("Help - Click")]
        public ActionExecuter clickVoid;
        public ActionExecuter clickLinkable_ButTooFar;
        public ActionExecuter clickLinkable_ButWrongPair;
        public ActionExecuter clickLinkable_ButAlreadyLinked;
        public ActionExecuter clickLinkable_Valid;

        private float currentDontClickTime = 0;

        [Space]
        [Header("Help - Don't click")]
        public float dontClickTime = 10;
        public ActionExecuter dontClickAction;

        [Space]
        [Header("Help - Links")]
        public ActionExecuter linkBrokeAction;

        [Space]
        [Header("Help - Energy")]
        public int energyEmptyAmount = 2;
        public ActionExecuter energyEmpty;

        private int currentEnergyEmptyAmount = 0;
        
        #region MonoBehaviour Methods
        void Update()
        {
            if (!tutorialEnabled)
                return;

            // Don't Click
            currentDontClickTime += Time.deltaTime;
            if (currentDontClickTime >= dontClickTime)
                DontClick();

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

            // Save position
            dontMovePositionLastFrame = transform.position;
        }
        #endregion

        #region Tutorial Helpers Methods
        // Move Helpers
        public void DontMove()
        {
            if (!tutorialEnabled)
                return;

            if (dontMoveAction != null)
                dontMoveAction.SetStarted(true);
            currentDontMoveTime = 0;
        }

        // Click Helpers
        public void ClickLinkable_Valid()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickLinkable_Valid != null)
                clickLinkable_ButTooFar.SetStarted(true);
        }

        public void ClickLinkable_TooFar()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickLinkable_ButTooFar != null)
                clickLinkable_ButTooFar.SetStarted(true);
        }

        public void ClickLinkable_WrongPair()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickLinkable_ButWrongPair != null)
                clickLinkable_ButTooFar.SetStarted(true);
        }

        public void ClickLinkable_AlreadyLinked()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickLinkable_ButAlreadyLinked != null)
                clickLinkable_ButAlreadyLinked.SetStarted(true);
        }

        public void ClickVoid()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickVoid != null)
                clickVoid.SetStarted(true);
        }

        public void DontClick()
        {
            if (!tutorialEnabled)
                return;

            if (dontClickAction != null)
                dontClickAction.SetStarted(true);
            currentDontClickTime = 0;
        }

        // Link Helpers
        public void LinkIntersectAndBroke()
        {
            if (!tutorialEnabled)
                return;

            if (linkBrokeAction != null)
                linkBrokeAction.SetStarted(true);
        }

        // Energy Helpers
        public void EnergyEmpty()
        {
            if (!tutorialEnabled)
                return;
            
            currentEnergyEmptyAmount++;
            if (currentEnergyEmptyAmount >= energyEmptyAmount)
                if (energyEmpty != null)
                    energyEmpty.SetStarted(true);
        }
        #endregion
    }
}
