using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using PCG;
//using UnityEditor;

namespace CeuxQuiRestent
{
    public class Scanner : MonoBehaviour
    {
        [Header("Materials")]
        public Material scanningMaterial;
        public Material obfuscationMaterial;
        public Transform scannerObject;

        [Header("Room Manager")]
        public Transform roomManager;
        public Transform roomCenter;
        public Transform roomBoundsMin;

        [Header("Spatial Mapping")]
        public Transform meshesWrapper;
        public int ecartCenterAmount = 10;

        [Header("Scan")]
        public float scan_waitTime = 6;
        private float scan_currentWaitTime = 0;
        private bool scan_done = false;
        private bool scan_active = false;

        private Transform technician;
        private Vector3 lastPositionCenter = Vector3.zero;
        private List<float> ecartCenter = new List<float>();
        private int lastScanningLoop = 0;

        private void Start()
        {
            technician = GameObject.FindGameObjectWithTag("Cursor").transform;
        }

        public void ActiveScan()
        {
            scan_active = true;
            scannerObject.gameObject.SetActive(true);
            for (int c = 0; c < meshesWrapper.childCount; c++)
            {
                GameObject child = meshesWrapper.GetChild(c).gameObject;
                child.GetComponent<MeshRenderer>().material = scanningMaterial;
            }
        }

        public void FinishScan()
        {
            scan_done = true;
            for (int c = 0; c < meshesWrapper.childCount; c++)
            {
                GameObject child = meshesWrapper.GetChild(c).gameObject;
                child.GetComponent<MeshRenderer>().material = obfuscationMaterial;
            }
            scannerObject.gameObject.SetActive(false);
            scan_active = false;
        }

        void FixedUpdate()
        {
            int scanningLoop = Mathf.FloorToInt((Time.timeSinceLevelLoad) / (5 / scanningMaterial.GetFloat("_Speed")));
            if (!scan_active)
                return;
            Ray ray = new Ray(technician.position, technician.forward);
            RaycastHit rayHit = new RaycastHit();
            if (Physics.Raycast(ray, out rayHit, 500, LayerMask.GetMask(new string[] { "Spatial Mapping Mesh" })))
            {
                //Shader.SetGlobalVector("_Center", rayHit.point);
                if (lastScanningLoop != scanningLoop)
                    scanningMaterial.SetVector("_Center", rayHit.point);
                lastScanningLoop = scanningLoop;
            }
            if (!scan_done)
            {
                //ScanV1();
                /*
                scan_currentWaitTime += Time.deltaTime;
                if (scan_currentWaitTime >= scan_waitTime)
                {
                    for (int c = 0; c < meshesWrapper.childCount; c++)
                    {
                        GameObject child = meshesWrapper.GetChild(c).gameObject;
                        child.GetComponent<MeshRenderer>().material = scanningMaterial;
                    }

                    scan_currentWaitTime = 0;
                    Vector3 centerLargestRoomBounds = new Vector3(0, 0, 0);
                    Vector3 mappedRoomMin = new Vector3();
                    float largestRoomArea = 0;
                    for (int c = 0; c < meshesWrapper.childCount; c++)
                    {

                        MeshFilter mf = (meshesWrapper.GetChild(c).gameObject.GetComponent<MeshFilter>());
                        float roomArea = mf.sharedMesh.bounds.size.x * mf.sharedMesh.bounds.size.z;
                        if (roomArea > largestRoomArea)
                        {
                            largestRoomArea = roomArea;
                            centerLargestRoomBounds = mf.sharedMesh.bounds.center;
                            mappedRoomMin = mf.sharedMesh.bounds.min;
                        }
                    }
                    scannerObject.position = centerLargestRoomBounds;
                    if (lastPositionCenter != Vector3.zero)
                    {
                        ecartCenter.Add(Vector3.Distance(scannerObject.position, lastPositionCenter));
                        while (ecartCenter.Count > ecartCenterAmount && ecartCenterAmount >= 1)
                            ecartCenter.RemoveAt(0);
                    }
                    float sumEcarts = 0;
                    for (int ec = 0; ec < ecartCenter.Count; ec++)
                        sumEcarts += ecartCenter[ec];
                    lastPositionCenter = scannerObject.position;
                }*/
            }
        }

        public void PlaceRoom()
        {
            /*
            Vector3 centerLargestRoomBounds = new Vector3(0, 0, 0);
            Vector3 mappedRoomMin = new Vector3();
            float largestRoomArea = 0;
            for (int c = 0; c < meshesWrapper.childCount; c++)
            {

                MeshFilter mf = (meshesWrapper.GetChild(c).gameObject.GetComponent<MeshFilter>());
                float roomArea = mf.sharedMesh.bounds.size.x * mf.sharedMesh.bounds.size.z;
                if (roomArea > largestRoomArea)
                {
                    largestRoomArea = roomArea;
                    centerLargestRoomBounds = mf.sharedMesh.bounds.center;
                    mappedRoomMin = mf.sharedMesh.bounds.min;
                }
            }

            int loop = 0;
            float angle = 0;
            do
            {
                loop++;
                Vector3 differentielRealRoom_VirtualRoom = roomCenter.position - centerLargestRoomBounds;

                Vector3 a = mappedRoomMin - new Vector3(centerLargestRoomBounds.x, 0, centerLargestRoomBounds.z);
                Vector3 b = (roomBoundsMin.position - differentielRealRoom_VirtualRoom) - (new Vector3(roomCenter.position.x, 0, roomCenter.position.z) - differentielRealRoom_VirtualRoom);
                
                //Debug.DrawLine(mappedRoomMin, new Vector3(centerLargestRoomBounds.x, 0, centerLargestRoomBounds.z), Color.yellow, 3);
                //Debug.DrawLine(tutorialRoomMin.position - differentielRealRoom_VirtualRoom, new Vector3(tutorialRoomCenter.position.x, 0, tutorialRoomCenter.position.z) - differentielRealRoom_VirtualRoom, Color.green, 3);

                angle = Vector3.Angle(a, b);
                roomManager.Rotate(Vector3.up, -angle);

                Vector3 differentielRoomPositionCenterRoom = roomCenter.position - roomManager.position;
                roomManager.position = centerLargestRoomBounds - differentielRoomPositionCenterRoom;
            } while ((angle >= 1 || angle <= -1) && loop < 20);
            */

            FinishScan();
        }

        // Attributes
        // WallScan
        /*
        [Header("Wall scan")]
        public Transform wallScan_target;
        public float wallScan_timeBetweenIterations = 0.5f;
        public int wallScan_totalRays = 50;
        public int wallScan_iterations = 10;
        public int nbTurns = 3;
        public ActionExecuter wallScan_endActions;
        public float thresholdWall = 5;
        
        // Private WallScan
        public bool wallScan_finished = false;
        private int currentNbTurns = 0;
        private float currTime = 0;
        private int raysPerIteration = 0;
        private int raysShot = 0;
        private float currentAngle = 0;
        private float angleIncrement = 0;
        private List<Vector3> impactPoints = new List<Vector3>();
        private List<GameObject> impactPointsGO = new List<GameObject>();
        private List<int> impactPointsID_XOrdered = new List<int>();
        private List<int> impactPointsID_ZOrdered = new List<int>();
        public bool orderX = true;
        */

        // Debug WallScan
        /*[Header("Debug")]
        public GameObject wallScan_impactPoint;*/

        //private Transform player;


        //public Transform meshesWrapper2;
        //private List<MeshFilter> meshes = new List<MeshFilter>();

        // MonoBehaviour Methods
        //void Start()
        //{
            /*
            player = GameObject.FindGameObjectWithTag("MainCamera").transform;
            raysPerIteration = wallScan_totalRays / wallScan_iterations;
            angleIncrement = 360.0f / (float)(wallScan_totalRays);*/
        //}

            /*
            if (!wallScan_finished)
            {
                if (currentNbTurns == nbTurns)
                    FinalizeWallScan();
                else
                {
                    currTime += Time.deltaTime;
                    if (currTime >= wallScan_timeBetweenIterations)
                    {
                        currTime = 0;
                        if (raysShot < wallScan_totalRays)
                            WallScanIteration();
                        else
                        {
                            currentNbTurns++;
                            raysShot = 0;
                            currentAngle = 0;
                        }
                    }
                }
            }*/

        public void ScanV1()
        {
            scan_currentWaitTime += Time.deltaTime;
            if (scan_currentWaitTime >= scan_waitTime)
            {
                Vector3 centerLargestRoomBounds = new Vector3(0, 0, 0);
                Vector3 mappedRoomMin = new Vector3();
                float largestRoomArea = 0;
                for (int c = 0; c < meshesWrapper.childCount; c++)
                {

                    MeshFilter mf = (meshesWrapper.GetChild(c).gameObject.GetComponent<MeshFilter>());
                    float roomArea = mf.sharedMesh.bounds.size.x * mf.sharedMesh.bounds.size.z;
                    if (roomArea > largestRoomArea)
                    {
                        largestRoomArea = roomArea;
                        centerLargestRoomBounds = mf.sharedMesh.bounds.center;
                        mappedRoomMin = mf.sharedMesh.bounds.min;
                    }
                }

                float angle = 0;
                do
                {
                    Vector3 differentielRealRoom_VirtualRoom = roomCenter.position - centerLargestRoomBounds;

                    Vector3 a = mappedRoomMin - new Vector3(centerLargestRoomBounds.x, 0, centerLargestRoomBounds.z);
                    Vector3 b = (roomBoundsMin.position - differentielRealRoom_VirtualRoom) - (new Vector3(roomCenter.position.x, 0, roomCenter.position.z) - differentielRealRoom_VirtualRoom);

                    /*
                    Debug.DrawLine(mappedRoomMin, new Vector3(centerLargestRoomBounds.x, 0, centerLargestRoomBounds.z), Color.yellow, 3);
                    Debug.DrawLine(tutorialRoomMin.position - differentielRealRoom_VirtualRoom, new Vector3(tutorialRoomCenter.position.x, 0, tutorialRoomCenter.position.z) - differentielRealRoom_VirtualRoom, Color.green, 3);*/

                    angle = Vector3.Angle(a, b);
                    roomManager.Rotate(Vector3.up, -angle);

                    Vector3 differentielRoomPositionCenterRoom = roomCenter.position - roomManager.position;
                    roomManager.position = centerLargestRoomBounds - differentielRoomPositionCenterRoom;
                } while (angle >= 1 || angle <= -1);


                scan_done = true;
            }
        }

        /*
        void OnDrawGizmosSelected()
        {
            MeshFilter largestMeshFilter = new MeshFilter();
            float largestRoomArea = 0;
            for (int c = 0; c < meshesWrapper.childCount; c++)
            {
                MeshFilter mf = (meshesWrapper.GetChild(c).gameObject.GetComponent<MeshFilter>());
                float roomArea = mf.sharedMesh.bounds.size.x * mf.sharedMesh.bounds.size.z;
                if (roomArea > largestRoomArea)
                {
                    largestRoomArea = roomArea;
                    largestMeshFilter = mf;
                }
            }
            if (largestMeshFilter != null)
            {
                MeshFilter mf = largestMeshFilter;
                Handles.matrix = mf.gameObject.transform.localToWorldMatrix;
                Handles.DrawWireCube(mf.sharedMesh.bounds.center, mf.sharedMesh.bounds.size);
                Handles.DrawWireCube(mf.sharedMesh.bounds.min, Vector3.one);
                Handles.DrawWireCube(mf.sharedMesh.bounds.max, Vector3.one);
            }
            MeshFilter largestMeshFilter2 = meshesWrapper2.GetChild(0).gameObject.GetComponent<MeshFilter>();
            float largestRoomArea2 = 0;
            if (meshesWrapper2 != null)
            {
                for (int c = 0; c < meshesWrapper2.childCount; c++)
                {
                    MeshFilter mf2 = (meshesWrapper2.GetChild(c).gameObject.GetComponent<MeshFilter>());
                    float roomArea = mf2.sharedMesh.bounds.size.x * mf2.sharedMesh.bounds.size.z;
                    if (roomArea > largestRoomArea)
                    {
                        largestRoomArea2 = roomArea;
                        largestMeshFilter2 = mf2;
                    }
                }
                if (largestMeshFilter != null)
                {
                    MeshFilter mf2 = largestMeshFilter2;
                    Handles.matrix = mf2.gameObject.transform.localToWorldMatrix;
                    Handles.DrawWireCube(mf2.sharedMesh.bounds.center, mf2.sharedMesh.bounds.size);
                    Handles.DrawWireCube(mf2.sharedMesh.bounds.min, Vector3.one);
                    Handles.DrawWireCube(mf2.sharedMesh.bounds.max, Vector3.one);

                }
            }
        }*/

        // WallScan Methods
        
        /*private void WallScanIteration()
        {
            for (int r = 0; r < raysPerIteration; r++)
            {
                Vector3 target = player.position;
                target = new Vector3(target.x + 2 * Mathf.Cos(currentAngle * Mathf.PI / 180), target.y, target.z + 2 * Mathf.Sin(currentAngle * Mathf.PI / 180));
                Ray ray = new Ray(player.position, target - player.position);
                RotatePointAroundAxis(target, currentAngle, player.up);
                RaycastHit raycastHit = new RaycastHit();
                if (Physics.Raycast(ray, out raycastHit, 60, LayerMask.GetMask(new string[] { "Spatial Mapping Mesh" })))
                {
                    Debug.DrawLine(player.position, raycastHit.point, Color.yellow, wallScan_timeBetweenIterations);
                    impactPoints.Add(raycastHit.point);
                    impactPointsGO.Add(Instantiate(wallScan_impactPoint, raycastHit.point, Quaternion.identity));
                }
                else
                    Debug.DrawLine(player.position, player.position + (50 * (target - player.position)), Color.blue, wallScan_timeBetweenIterations);
                currentAngle += angleIncrement;
            }
            raysShot += raysPerIteration;
        }*/

        /*Vector3 RotatePointAroundAxis(Vector3 point, float angle, Vector3 axis)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            return q * point; //Note: q must be first (point * q wouldn't compile)
        }*/

        private void FinalizeWallScan()
        {
            /*
            // Init
            bool wallScanCompleted = true;

            // Process
            for (int p = 0; p < impactPoints.Count; p++)
            {
                if (impactPointsID_XOrdered.Count > 0)
                {
                    impactPointsID_XOrdered.Add(p);
                    int pid = impactPointsID_XOrdered.Count - 1;
                    while (pid > 0 && impactPoints[impactPointsID_XOrdered[pid]].x < impactPoints[impactPointsID_XOrdered[pid - 1]].x)
                    {
                        int a = impactPointsID_XOrdered[pid];
                        int b = impactPointsID_XOrdered[pid - 1];
                        int c = a;
                        impactPointsID_XOrdered[pid] = b;
                        impactPointsID_XOrdered[pid - 1] = c;
                        pid--;
                    }
                }
                else
                    impactPointsID_XOrdered.Add(p);

                if (impactPointsID_ZOrdered.Count > 0)
                {
                    impactPointsID_ZOrdered.Add(p);
                    int pid = impactPointsID_ZOrdered.Count - 1;
                    while (pid > 0 && impactPoints[impactPointsID_ZOrdered[pid]].z < impactPoints[impactPointsID_ZOrdered[pid - 1]].z)
                    {
                        int a = impactPointsID_ZOrdered[pid];
                        int b = impactPointsID_ZOrdered[pid - 1];
                        int c = a;
                        impactPointsID_ZOrdered[pid] = b;
                        impactPointsID_ZOrdered[pid - 1] = c;
                        pid--;
                    }
                }
                else
                    impactPointsID_ZOrdered.Add(p);
            }

            Vector3 minX = impactPoints[impactPointsID_XOrdered[0]];
            Vector3 maxX = impactPoints[impactPointsID_XOrdered[impactPointsID_XOrdered.Count - 1]];
            Vector3 minZ = impactPoints[impactPointsID_ZOrdered[0]];
            Vector3 maxZ = impactPoints[impactPointsID_ZOrdered[impactPointsID_ZOrdered.Count - 1]];*/

            /*
            Mesh cube = MeshUtility.CombineMeshes(MeshUtility.GenerateCube(new Vector3(Mathf.Lerp(minX.x, maxX.x, 0.5f), player.position.y, 0), new Vector3(Vector3.Distance(minX, maxX) / 2, 5, 1), MeshFaces.Outside));
            MeshUtility.GenerateMeshGameObject(transform, "Cube", false, null, cube);
            *//*
            List<int> lineXmin = new List<int>();
            List<int> lineXmax = new List<int>();
            List<int> lineZmin = new List<int>();
            List<int> lineZmax = new List<int>();

            for (int p = 0; p < impactPointsID_XOrdered.Count; p++)
            {
                if (impactPoints[impactPointsID_XOrdered[p]].x < minX.x + thresholdWall)
                    lineXmin.Add(impactPointsID_XOrdered[p]);
            }
            for (int p = impactPointsID_XOrdered.Count - 1; p >= 0; p--)
            {
                if (impactPoints[impactPointsID_XOrdered[p]].x > maxX.x - thresholdWall)
                    lineXmax.Add(impactPointsID_XOrdered[p]);
            }

            for (int p = 0; p < impactPointsID_ZOrdered.Count; p++)
            {
                if (impactPoints[impactPointsID_ZOrdered[p]].z < minZ.z + thresholdWall)
                    lineZmin.Add(impactPointsID_ZOrdered[p]);
            }
            for (int p = impactPointsID_ZOrdered.Count - 1; p >= 0; p--)
            {
                if (impactPoints[impactPointsID_ZOrdered[p]].z > maxZ.z - thresholdWall)
                    lineZmax.Add(impactPointsID_ZOrdered[p]);
            }

            lineXmin = CutStartEnd(0.1f, lineXmin);
            lineXmax = CutStartEnd(0.1f, lineXmax);
            lineZmin = CutStartEnd(0.1f, lineZmin);
            lineZmax = CutStartEnd(0.1f, lineZmax);

            for (int go = 0; go < lineXmin.Count; go++)
                impactPointsGO[lineXmin[go]].transform.position = new Vector3(impactPointsGO[lineXmin[go]].transform.position.x, 10, impactPointsGO[lineXmin[go]].transform.position.z);

            for (int go = 0; go < lineXmax.Count; go++)
                impactPointsGO[lineXmax[go]].transform.position = new Vector3(impactPointsGO[lineXmax[go]].transform.position.x, 11, impactPointsGO[lineXmax[go]].transform.position.z);

            for (int go = 0; go < lineZmin.Count; go++)
                impactPointsGO[lineZmin[go]].transform.position = new Vector3(impactPointsGO[lineZmin[go]].transform.position.x, 21, impactPointsGO[lineZmin[go]].transform.position.z);

            for (int go = 0; go < lineZmax.Count; go++)
                impactPointsGO[lineZmax[go]].transform.position = new Vector3(impactPointsGO[lineZmax[go]].transform.position.x, 22, impactPointsGO[lineZmax[go]].transform.position.z);

            */


            /*
            float posDist = 20;
            float posMin = player.position.y - 2;
            float posMax = player.position.z + 2;
            float ratio = (float)((float)posDist / (float)impactPointsID_XOrdered.Count);
            if (orderX)
            {
                for (int p = 0; p < impactPointsID_XOrdered.Count; p++)
                {
                    if (p < impactPointsGO.Count)
                    {
                        Vector3 pos = impactPointsGO[impactPointsID_XOrdered[p]].transform.position;
                        pos = new Vector3(pos.x, posMin + ratio * p, pos.z);
                        impactPointsGO[impactPointsID_XOrdered[p]].transform.position = pos;
                    }
                }
            }
            else
            {
                ratio = (float)((float)posDist / (float)impactPointsID_ZOrdered.Count);
                for (int p = 0; p < impactPointsID_ZOrdered.Count; p++)
                {
                    if (p < impactPointsGO.Count)
                    {
                        Vector3 pos = impactPointsGO[impactPointsID_ZOrdered[p]].transform.position;
                        pos = new Vector3(pos.x, posMin + ratio * p, pos.z);
                        impactPointsGO[impactPointsID_ZOrdered[p]].transform.position = pos;
                    }
                }
            }*/




            // End
            /*if (wallScanCompleted)
            {
                if (wallScan_endActions != null)
                    wallScan_endActions.SetStarted(true);
                wallScan_finished = true;
            }
            else
            {
                raysShot = 0;
                currentAngle = 0;
                wallScan_finished = false;
            }*/
        }

        /*
        public List<int> CutStartEnd(float percent, List<int> listToCut)
        {
            int amount = (int)(percent * listToCut.Count);
            for (int a = 0; a < amount; a++)
                listToCut.RemoveAt(0);
            for (int a = 0; a < amount; a++)
                listToCut.RemoveAt(listToCut.Count - 1);
            return listToCut;
        }*/
    }

}