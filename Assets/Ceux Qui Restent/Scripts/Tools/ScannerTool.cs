using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Tools
{
    [ExecuteInEditMode]
    public class ScannerTool : MonoBehaviour
    {
        #region Attributes
        private bool isScanning = false;
        private bool scanDone = false;

        [Header("Scan point")]
        public bool followPlayer = false;
        public Vector3 posOffset = Vector3.zero;

        private float angleIncrement = 0;
        private float currentAngle = 0;

        [Header("Scan effect")]
        public float timeBetweenIterations = 0.25f;
        public float scanTime = 5;
        public float turnTime = 2;
        public int raysPerTurn = 10;

        private float currentTimeBetweenIterations = 0;
        private float scanCurrentTime = 0;
        private float turnCurrentTime = 0;
        private int raysPerIteration = 0;
        #endregion

        #region MonoBehaviour Methods
        void Update()
        {
            if (isScanning)
            {
                if (followPlayer)
                    posOffset = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;

                currentTimeBetweenIterations += Time.deltaTime;
                if (currentTimeBetweenIterations >= timeBetweenIterations)
                {
                    currentTimeBetweenIterations -= timeBetweenIterations;
                    ScanIteration();
                }
            }
        }
        #endregion

        #region Methods
        public void StartScan()
        {
            ResetScan();
            scanCurrentTime = 0;
            int nbIterationsPerTurn = (int)(turnTime / timeBetweenIterations);
            if (nbIterationsPerTurn <= 0)
                nbIterationsPerTurn = 1;
            raysPerIteration = (int)(raysPerTurn / nbIterationsPerTurn);
            angleIncrement = 360.0f / (raysPerTurn);
            isScanning = true;
        }

        public void EndScan()
        {
            scanDone = true;
            isScanning = false;
        }

        public void ScanIteration()
        {
            scanCurrentTime += timeBetweenIterations;

            turnCurrentTime += timeBetweenIterations;
            if (turnCurrentTime >= turnTime)
                turnCurrentTime -= turnTime;

            Vector3 pointThatShootRays = transform.position + posOffset;
            for (int r = 0; r < raysPerIteration; r++)
            {
                Vector3 target = pointThatShootRays;
                target = new Vector3(target.x + 2 * Mathf.Cos(currentAngle * Mathf.PI / 180), target.y, target.z + 2 * Mathf.Sin(currentAngle * Mathf.PI / 180));

                Ray ray = new Ray(pointThatShootRays, target - pointThatShootRays);

                RotatePointAroundAxis(target, currentAngle, pointThatShootRays + Vector3.up);
                RaycastHit raycastHit = new RaycastHit();
                if (Physics.Raycast(ray, out raycastHit, 60, LayerMask.GetMask(new string[] { "Spatial Mapping Mesh" })))
                {
                    Debug.DrawLine(pointThatShootRays, raycastHit.point, Color.yellow, timeBetweenIterations);
                    //impactPoints.Add(raycastHit.point);
                    //impactPointsGO.Add(Instantiate(wallScan_impactPoint, raycastHit.point, Quaternion.identity));
                }
                else
                    Debug.DrawLine(pointThatShootRays, pointThatShootRays + (50 * (target - pointThatShootRays)), Color.blue, timeBetweenIterations);

                currentAngle += angleIncrement;
            }

            if (scanCurrentTime >= scanTime)
                EndScan();
        }

        Vector3 RotatePointAroundAxis(Vector3 point, float angle, Vector3 axis)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            return q * point; //Note: q must be first (point * q wouldn't compile)
        }

        public void ResetScan()
        {
            scanDone = false;
            scanCurrentTime = 0;
            currentAngle = 0;
        }
        #endregion

        #region Getters & Setters
        public float GetCurrentTime()
        {
            return scanCurrentTime;
        }

        public bool IsScanDone()
        {
            return scanDone;
        }

        public bool IsScanning()
        {
            return isScanning;
        }
        #endregion
    }

}
