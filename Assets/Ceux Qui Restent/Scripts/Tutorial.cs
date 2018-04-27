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
        public ActionExecuter clickTooFarAction;
        public ActionExecuter clickOnVoidAction;

        [Space]
        [Header("Help - Don't click")]
        public float dontClickTime = 10;
        public ActionExecuter dontClickAction;

        private float currentDontClickTime = 0;

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
            if (dontMoveAction != null)
                dontMoveAction.SetStarted(true);
        }

        // Click Helpers
        public void ClickLinkable_Valid()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
        }

        public void ClickLinkable_TooFar()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickTooFarAction != null)
                clickTooFarAction.SetStarted(true);
        }

        public void ClickLinkable_WrongPair()
        {

        }

        public void ClickLinkable_AlreadyLinked()
        {

        }

        public void ClickVoid()
        {
            if (!tutorialEnabled)
                return;

            dontClickTime = 0;
            if (clickOnVoidAction != null)
                clickOnVoidAction.SetStarted(true);
        }

        public void DontClick()
        {
            if (!tutorialEnabled)
                return;

            if (dontClickAction != null)
                dontClickAction.SetStarted(true);
        }

        // Energy/Link Helpers
        public void LinkIntersectAndBroke()
        {

        }

        public void EnergyEmpty()
        {

        }
        #endregion
    }
}
