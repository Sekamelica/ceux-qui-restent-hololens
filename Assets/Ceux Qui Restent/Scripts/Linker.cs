using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierUtility;
using PCG;

namespace CeuxQuiRestent
{
    public class Linker : MonoBehaviour
    {
        // - - - Public attributes - - - //
        [Header("General")]
        public Energy energy;
        public GameObject linkPrefab;
        [Tooltip("Position at where the next link point will be instantiate.")]
        public Transform target;
        [Tooltip("Maximum distance between you and the Linkable to interact with.")]
        public float distanceInteraction;

        [Header("Particle Effects")]
        public GameObject effect_startLink;
        public GameObject effect_endLink;
        public GameObject effect_brokeLink;
        public GameObject effect_brokeLink_small;

        [Header("Link optimization")]
        public float distanceBetweenTwoLinkPoints = 1;
        public int linkCurveLength = 30;

        [Header("Link polish")]
        public Vector3 gravityOffset;
        public float animationIntensity = 1;
        public float animationSpeed = 1;
        public float gravityFactor = 0.1f;
        public int min_linkAmount = 2;
        public int max_linkAmount = 4;

        // - - - Private attributes - - - //
        private List<Link> allLinks = new List<Link>(); // Remember all links created.

        // Current Links
        private float totalDistance = 0;
        private float distanceWalk = 0;
        private List<Vector3> linkPoints = new List<Vector3>();
        private List<BezierMultiCurves> links = new List<BezierMultiCurves>();
        private List<BezierMeshMultiCurves> linkMeshes = new List<BezierMeshMultiCurves>();
        private GameObject origin;
        private GameObject destination;

        // Linker
        private bool isLinking = false;
        private Vector3 positionLastFrame;

        #region MonoBehaviour Methods
        void Start()
        {
            positionLastFrame = transform.position;
        }

        void Update()
        {
            // IsLinking and has moved
            if (isLinking && (transform.position != positionLastFrame))
            {
                // When the technician is creating a link, add a point to the list everytime he or she move a defined distance.
                if (transform.position != positionLastFrame)
                {
                    distanceWalk += Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(positionLastFrame.x, positionLastFrame.z));

                    // We have moved enough, we try to increase the link length according to the new position.
                    if (distanceWalk >= distanceBetweenTwoLinkPoints)
                    {
                        IncreaseLinkLength(distanceBetweenTwoLinkPoints);
                    }
                }
            }

            // Register your position for detect if you have move in the next frame
            positionLastFrame = transform.position;
        }
        #endregion

        #region Linker Methods
        /// <summary>
        /// Consume energy and increase the link length, broke if it collides another link or if there is not enough energy.
        /// Return false if the link has been destroyed in the process, return true otherwise.
        /// </summary>
        public bool IncreaseLinkLength(float distance)
        {
            distanceWalk -= distance;
            totalDistance += distance;
            if (energy.AddToValue(-distance)) // We have enough energy
            {
                linkPoints.Add(target.position);
                UpdateLink();
                return !CheckLinkIntersection();
            }
            else // You don't have enough energy, the link broke.
            {
                DestroyCurrentLink();
                return false;
            }
        }

        /// <summary>
        /// Check intersection between the current links and another created link.
        /// Destroy current link in case of intersect.
        /// Return if the current link has intersected anything.
        /// </summary>
        /// <returns></returns>
        public bool CheckLinkIntersection()
        {
            if (linkPoints.Count > 0)
            {
                // Create a list of lines representing the current link.
                List<Vector2> currentLinkLines = new List<Vector2>();
                for (int p = 0; p < linkPoints.Count; p += linkCurveLength)
                {
                    currentLinkLines.Add(new Vector2(linkPoints[p].x, linkPoints[p].z));
                    if (p + linkCurveLength >= linkPoints.Count)
                        currentLinkLines.Add(new Vector2(linkPoints[linkPoints.Count - 1].x, linkPoints[linkPoints.Count - 1].z));
                }

                // If the current link intersect another created link, destroy the current link.
                for (int l = 0; l < allLinks.Count; l++)
                {
                    if (allLinks[l].Intersect(currentLinkLines))
                    {
                        DestroyCurrentLink();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Destroy current link and refill energy.
        /// </summary>
        public void DestroyCurrentLink()
        {
            // Link destruction
            for (int bmc = links.Count - 1; bmc >= 0; bmc--)
            {
                BezierMultiCurves bmcurves = links[bmc];

                // Effects
                if (effect_brokeLink_small != null)
                {
                    int c = 0;
                    for (int eff = 0; eff < bmcurves.points.Length; eff += 4)
                    {
                        for (int i = 0; i < 6; i++)
                            GameObject.Instantiate(effect_brokeLink_small, bmcurves.GetPoint(c, ((float)i) / 5.0f), Quaternion.identity);
                        c++;
                    }
                }
                if (effect_brokeLink != null)
                    GameObject.Instantiate(effect_brokeLink, linkPoints[linkPoints.Count - 1], Quaternion.identity);

                // Destroy link
                GameObject.Destroy(bmcurves.gameObject);
            }

            // Refill energy
            energy.AddToValue(totalDistance);

            ClearVariables();
        }

        /// <summary>
        /// Update how the link should looks like
        /// </summary>
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

        /// <summary>
        /// Start the link and return if the link has been correctly created.
        /// </summary>
        /// <param name="linkablePos"></param>
        public void StartLinking(Vector3 linkablePos)
        {
            // Clear variables
            ClearVariables();

            // Initialize variables
            int nbLinks = Random.Range(min_linkAmount, max_linkAmount);
            for (int i = 0; i < nbLinks; i++)
            {
                GameObject linkGO = GameObject.Instantiate(linkPrefab);
                links.Add(linkGO.GetComponent<BezierMultiCurves>());
                linkMeshes.Add(linkGO.GetComponent<BezierMeshMultiCurves>());
            }
            isLinking = true;

            // Add first points
            linkPoints.Add(linkablePos);
            linkPoints.Add(target.position);
            IncreaseLinkLength(Vector3.Distance(linkablePos, target.position));
        }

        /// <summary>
        /// End the link and return if the link has been correctly linked.
        /// </summary>
        /// <param name="linkablePos"></param>
        /// <returns></returns>
        public bool StopLinking(Vector3 linkablePos)
        {
            isLinking = false;
            linkPoints.Add(linkablePos);

            if (IncreaseLinkLength(Vector3.Distance(linkablePos, target.position)))
            {
                // The link has been sucessfully completed
                for (int lm = 0; lm < linkMeshes.Count; lm++)
                {
                    linkMeshes[lm].animationSpeed = animationSpeed;
                    linkMeshes[lm].animationIntensity = animationIntensity;
                    linkMeshes[lm].gravityAnimation = true;
                }

                // GameObjects of links created
                List<GameObject> linksGO = new List<GameObject>();
                foreach (BezierMultiCurves bmmc in links)
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

                ClearVariables();
                return true;
            }
            else
            {
                // The link has broke
                ClearVariables();
                return false;
            }
        }

        /// <summary>
        /// Clear and reset the private attributes
        /// </summary>
        public void ClearVariables()
        {
            isLinking = false;
            linkPoints.Clear();
            links.Clear();
            linkMeshes.Clear();
            distanceWalk = 0;
            totalDistance = 0;
        }

        /// <summary>
        /// Called when you interact with a Linkable
        /// </summary>
        /// <param name="linkablePos"></param>
        /// <param name="clicked"></param>
        /// <param name="clickedPair"></param>
        public void LinkableClick(Vector3 linkablePos, GameObject clicked, GameObject clickedPair)
        {
            if (isLinking)
            {
                if (destination == clicked && origin == clickedPair) // End the link on the good Linkable.
                {
                    // Effect
                    if (effect_startLink != null)
                        GameObject.Instantiate(effect_startLink, linkablePos, Quaternion.Euler(-90, 0, 0), null);

                    if (StopLinking(linkablePos))
                    {
                        // Energy (Increase max energy and refill energy)
                        energy.AddToMaximum(origin.GetComponent<Linkable>().energyMaximumIncrease);
                        energy.ChangeValue(energy.GetMaximum());

                        // Link the Linkables
                        clicked.GetComponent<Linkable>().Linked();
                        clickedPair.GetComponent<Linkable>().Linked();
                    }
                }
                else // Try to end the link on a wrong Linkable.
                {

                }
            }
            else // Start a link on a Linkable.
            {
                if (clicked.GetComponent<Linkable>().IsAlreadyLinked()) // Can't add a link, this linkable is already linked.
                {
                    
                }
                else if (clicked.GetComponent<Linkable>().pair != null) // Add the link on this linkable.
                {
                    // Effect
                    if (effect_endLink != null)
                        GameObject.Instantiate(effect_endLink, linkablePos, Quaternion.Euler(-90, 0, 0), null);

                    // Register the Linkable origin and the Linkable that should be the destination and start the link.
                    origin = clicked;
                    destination = clickedPair;
                    StartLinking(linkablePos);
                }
                else // The linkable doesn't have a Pair Linkable
                {
                    
                }
            }
        }
        #endregion
    }
}
