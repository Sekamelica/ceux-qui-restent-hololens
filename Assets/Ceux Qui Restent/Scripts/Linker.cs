﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierUtility;
using PCG;

namespace CeuxQuiRestent
{
    public class Linker : MonoBehaviour
    {
        // Public attributes
        public Datamosh datamosh;
        public Vector3 gravityOffset;
        public GameObject linkPrefab;
        public float animationIntensity = 1;
        public float animationSpeed = 1;
        public float distanceBetweenTwoLinkPoints = 1;
        public Transform target;
        public float gravityFactor = 0.1f;
        public int linkCurveLength = 30;

        // All Links Colliders
        private List<Link> allLinks = new List<Link>();

        // Private attributes
        private List<Vector3> linkPoints = new List<Vector3>();
        private float totalDistance = 0;
        private float distanceWalk = 0;
        private Vector3 positionLastFrame;
        private List<BezierMultiCurves> links = new List<BezierMultiCurves>();
        private List<BezierMeshMultiCurves> linkMeshes = new List<BezierMeshMultiCurves>();
        private bool isLinking = false;


        // Use this for initialization
        void Start()
        {
            positionLastFrame = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isLinking)
            {
                // When the technician is creating a link, add a point to the list everytime he or she move a defined distance.
                if (transform.position != positionLastFrame)
                {
                    distanceWalk += Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(positionLastFrame.x, positionLastFrame.z));
                    if (distanceWalk >= distanceBetweenTwoLinkPoints)
                    {
                        distanceWalk -= distanceBetweenTwoLinkPoints;
                        totalDistance += distanceBetweenTwoLinkPoints;
                        
                        linkPoints.Add(target.position);

                        // Update how the link look likes
                        UpdateLink();
                    }
                }
            }
            positionLastFrame = transform.position;

            // Draw debug line for intersection line line
            List<Vector2> currentLinkLines = new List<Vector2>();
            for (int p = 0; p < linkPoints.Count; p += linkCurveLength)
            {
                currentLinkLines.Add(new Vector2(linkPoints[p].x, linkPoints[p].z));
                if (p + linkCurveLength >= linkPoints.Count)
                    currentLinkLines.Add(new Vector2(linkPoints[linkPoints.Count - 1].x, linkPoints[linkPoints.Count - 1].z));
            }
            for(int l = 0; l < allLinks.Count; l++)
            {
                if(allLinks[l].Intersect(currentLinkLines))
                {
                    allLinks[l].Clear();
                    allLinks.RemoveAt(l);
                }
            }
        }

        public void UpdateLink()
        {
            List<List<Vector3>> linkPointsSin = new List<List<Vector3>>();
            for (int l = 0; l < links.Count; l++)
            {
                // Recreate the point list with a decalage for gravity, based on eloignment from link extremities
                linkPointsSin.Add(new List<Vector3>());
                for (int v = 0; v < linkPoints.Count; v++)
                {
                    Vector3 vec = linkPoints[v];
                    if (v != 0 && v != linkPoints.Count - 1)
                    {
                        float percent = (float)v / (float)(linkPoints.Count - 1);
                        percent = Mathf.Cos((2 * Mathf.PI * percent) - Mathf.PI);
                        percent += l * gravityFactor * percent;
                        vec = vec + gravityOffset + gravityOffset * percent;
                    }
                    linkPointsSin[l].Add(vec);
                }

                // Calculate how many curves the link contains.
                int curves = Mathf.FloorToInt(linkPointsSin[l].Count / linkCurveLength);
                if (linkPointsSin[l].Count % linkCurveLength != 0)
                    curves++;

                // Fill the curves points
                List<Vector3> curveLinkPoints = new List<Vector3>();
                for (int c = 0; c < curves; c++)
                {
                    // Find curve extremities
                    int aID = c * linkCurveLength;
                    int bID = ((c == curves - 1) ? linkPointsSin[l].Count - 1 : (c + 1) * linkCurveLength - 1);
                    Vector3 a = linkPointsSin[l][aID];
                    Vector3 b = linkPointsSin[l][bID];

                    // Find farthest point from the line defined by the extremities of the curve. This point will be used to define the curvature.
                    int longestID = -1;
                    float longestDist = 0;
                    for (int i = aID; i < bID; i++)
                    {
                        float distance = Vector3.Cross((b - a), linkPointsSin[l][i] - a).magnitude;
                        if (distance > longestDist)
                        {
                            longestDist = distance;
                            longestID = i;
                        }
                    }
                    if (longestID == -1)
                        longestID = bID;

                    // Add thoses points to the curves points lists
                    curveLinkPoints.Add(a);
                    curveLinkPoints.Add(linkPointsSin[l][longestID]);
                    curveLinkPoints.Add(linkPointsSin[l][longestID]);
                    curveLinkPoints.Add(b);
                }
                links[l].SetCurves(curveLinkPoints.ToArray());
            }

            // Regenerate the link meshes according to the curves points previously sets.
            foreach (BezierMeshMultiCurves linkMesh in linkMeshes)
                linkMesh.Regenerate();

        }

        public void StartLinking(Vector3 linkablePos)
        {
            int nbLinks = Random.Range(2, 4);

            links.Clear();
            linkMeshes.Clear();
            linkPoints.Clear();

            for (int i = 0; i < nbLinks; i++)
            {
                GameObject linkGO = GameObject.Instantiate(linkPrefab);
                links.Add(linkGO.GetComponent<BezierMultiCurves>());
                linkMeshes.Add(linkGO.GetComponent<BezierMeshMultiCurves>());
            }

            linkPoints.Add(linkablePos);
            isLinking = true;
        }

        public void StopLinking(Vector3 linkablePos)
        {
            isLinking = false;
            linkPoints.Add(linkablePos);
            UpdateLink();
            for (int lm = 0; lm < linkMeshes.Count; lm++)
            {
                linkMeshes[lm].animationSpeed = animationSpeed;
                linkMeshes[lm].animationIntensity = animationIntensity;
                linkMeshes[lm].gravityAnimation = true;
            }

            // GameObject of links created
            List<GameObject> linksGO = new List<GameObject>();
            foreach(BezierMultiCurves bmmc in links)
                linksGO.Add(bmmc.gameObject);
            // Colliders lines of links created
            List<Vector2> linkColliders = new List<Vector2>();
            for (int p = 0; p < linkPoints.Count; p += linkCurveLength)
            {
                linkColliders.Add(new Vector2(linkPoints[p].x, linkPoints[p].z));
                if (p + linkCurveLength >= linkPoints.Count)
                    linkColliders.Add(new Vector2(linkPoints[linkPoints.Count - 1].x, linkPoints[linkPoints.Count - 1].z));
            }
            // Remember them
            allLinks.Add(new Link(totalDistance, linksGO, linkColliders));
            
            // Clear & Reset
            linkPoints.Clear();
            links.Clear();
            linkMeshes.Clear();
            distanceWalk = 0;
            totalDistance = 0;
        }

        public void LinkableClick(Vector3 linkablePos)
        {
            if (isLinking)
                StopLinking(linkablePos);
            else
                StartLinking(linkablePos);
        }
    }

}
