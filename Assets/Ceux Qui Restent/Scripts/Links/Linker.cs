using System.Collections.Generic;
using UnityEngine;
using CeuxQuiRestent.UI;
using CeuxQuiRestent.Interactables;
using CeuxQuiRestent.Tutorial;
using CeuxQuiRestent.Audio;

namespace CeuxQuiRestent.Links
{
    [RequireComponent(typeof(Help))]
    public class Linker : MonoBehaviour
    {
        #region Attributes
        // - - - Public attributes - - - //
        [Header("General")]
        public Energy energy;
        public GameObject linkPrefab;
        [Tooltip("Position at where the next link point will be instantiate.")]
        public Transform target;

        [Header("Particle Effects")]
        public GameObject effectStartLink;
        public GameObject effectEndLink;
        public GameObject effectBrokeLink;
        public GameObject effectBrokeLinkSmall;

        [Header("Sounds effects")]
        public WwiseAudioSource[] audioSources;
        [Space]
        public AK.Wwise.Event soundClick;
        public AK.Wwise.Event soundClickWrongLinkable;
        public AK.Wwise.Event soundClickAlreadyLinkedLinkable;
        [Space]
        public AK.Wwise.Event soundLinkContinuous;
        public AK.Wwise.Event soundLinkCorrectEnd;
        public AK.Wwise.Event soundMemoryCompleted;
        public AK.Wwise.Event soundEnergyIncrease;
        [Space]
        public AK.Wwise.Event soundLinkBroke;
        public AK.Wwise.Event soundLinkCrossed;
        public AK.Wwise.Event soundLackEnergy;

        [Header("Link optimization")]
        public float distanceBetweenTwoLinkPoints = 1;
        public int linkCurveLength = 30;
        
        [Header("Link polish")]
        public Vector3 gravityOffset;
        public float gravityFactor = 0.1f;
        public int min_linkAmount = 2;
        public int max_linkAmount = 4;

        // - - - Private attributes - - - //
        private Help helper;
        private List<List<Link>> allLinks = new List<List<Link>>(); // Remember all links created for each room

        // Current Links
        private float totalDistance = 0;
        private float distanceWalk = 0;
        private List<Vector3> linkPoints = new List<Vector3>();
        private List<LinkCurve> linkCurves = new List<LinkCurve>();
        private List<LinkMesh> linkMeshes = new List<LinkMesh>();
        private List<Link> previousLinks = new List<Link>(); // Used for link parts created before a room changing
        private GameObject origin;
        private GameObject destination;

        // Linker
        private bool isLinking = false;
        private Vector3 positionLastFrame;
        private Quaternion rotationLastFrame;
        private float distanceInteraction;
        private RoomManager roomManager;
        private bool hasTakePortal = false;
        private Vector3 previousPortalPoint;

        // Audio
        private int audioSource = 0;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
            energy.Initialize();
            helper = GetComponent<Help>();
            positionLastFrame = transform.position;
            distanceInteraction = GameObject.FindGameObjectWithTag("Cursor").GetComponent<TechicianCursor>().distanceInteraction;
            for (int r = 0; r < roomManager.rooms.Length; r++)
                allLinks.Add(new List<Link>());
        }

        public void PlaySound(AK.Wwise.Event _event)
        {
            audioSources[audioSource].PlayEvent(_event);
            audioSource++;
            if (audioSource >= audioSources.Length)
                audioSource = 0;
        }

        void Update()
        {
            transform.rotation = Quaternion.identity;

            if (isLinking && !hasTakePortal)
            {
                if (transform.position != positionLastFrame)
                {
                    // When the technician is creating a link, add a point to the list everytime he or she move a defined distance.
                    if (transform.position != positionLastFrame || transform.rotation != rotationLastFrame)
                    {
                        Vector3 targetPoint = target.position;

                        Ray rayPortal = new Ray(transform.position, targetPoint - transform.position);
                        RaycastHit rayhitPortal = new RaycastHit();
                        if (Physics.Raycast(rayPortal, out rayhitPortal, 0.6f, LayerMask.GetMask(new string[] { "PortalRenderer" })))
                            targetPoint = rayhitPortal.point;

                        distanceWalk = Vector3.Distance(linkPoints[linkPoints.Count - 1], targetPoint);

                        // We have moved enough, we try to increase the link length according to the new position.
                        if (distanceWalk >= distanceBetweenTwoLinkPoints)
                            IncreaseLinkLength(distanceWalk, targetPoint);
                    }
                }
            }

            // Register your position for detect if you have move in the next frame
            rotationLastFrame = transform.rotation;
            positionLastFrame = transform.position;
        }
        #endregion

        #region Linker Methods

        public void EnterPortal(GameObject portalRenderer)
        {
            hasTakePortal = true;
            if (isLinking)
            {
                //Vector3 endPoint = ProjectOnPortal(target.position, portalRenderer.transform.position, -portalRenderer.transform.forward);
                //Vector3 endPoint = LineLineIntersect(transform.position, transform.position + portalRenderer.transform.forward, portalRenderer.transform.position, portalRenderer.transform.position + portalRenderer.transform.right);
                Vector3 endPoint = target.position;

                Ray rayPortal = new Ray(transform.position, endPoint - transform.position);
                RaycastHit rayhitPortal = new RaycastHit();
                if (Physics.Raycast(rayPortal, out rayhitPortal, 0.6f, LayerMask.GetMask(new string[] { "PortalRenderer" })))
                    endPoint = rayhitPortal.point;

                previousPortalPoint = endPoint;
                
                if (IncreaseLinkLength(Vector3.Distance(linkPoints[linkPoints.Count - 1], endPoint), endPoint))
                {
                    // GameObjects of links created
                    List<GameObject> linksGO = new List<GameObject>();
                    foreach (LinkCurve linkCurve in linkCurves)
                        linksGO.Add(linkCurve.gameObject);

                    // Collider defined by the lines of the link created
                    List<Vector2> linkCollider = new List<Vector2>();
                    for (int p = 0; p < linkPoints.Count; p += linkCurveLength)
                    {
                        linkCollider.Add(new Vector2(linkPoints[p].x, linkPoints[p].z));
                        if (p + linkCurveLength >= linkPoints.Count)
                            linkCollider.Add(new Vector2(linkPoints[linkPoints.Count - 1].x, linkPoints[linkPoints.Count - 1].z));
                    }

                    // Remember them
                    previousLinks.Add(new Link(roomManager.GetCurrentRoomID(), linksGO, linkCollider));
                }
            }
        }

        public void ExitPortal(GameObject portalRenderer)
        {
            if (isLinking)
            {
                //Vector3 startPoint = ProjectOnPortal(target.position, portalRenderer.transform.position, -portalRenderer.transform.forward);
                //Vector3 startPoint = LineLineIntersect(transform.position, transform.position - portalRenderer.transform.forward, portalRenderer.transform.position + (Vector3.up * 500), portalRenderer.transform.position + (Vector3.up * 500) + portalRenderer.transform.right);
                Vector3 startPoint2 = target.position;

                Ray rayPortal = new Ray(transform.position, startPoint2 - transform.position);
                RaycastHit rayhitPortal = new RaycastHit();
                if (Physics.Raycast(rayPortal, out rayhitPortal, 0.6f, LayerMask.GetMask(new string[] { "PortalRenderer" })))
                    startPoint2 = rayhitPortal.point;

                int nbLinks = linkCurves.Count;
                linkPoints.Clear();
                linkMeshes.Clear();
                linkCurves.Clear();
                
                for (int i = 0; i < nbLinks; i++)
                {
                    GameObject linkGameObject = GameObject.Instantiate(linkPrefab, roomManager.currentRoom.GetLinksRepository());
                    linkCurves.Add(linkGameObject.GetComponent<LinkCurve>());
                    linkMeshes.Add(linkGameObject.GetComponent<LinkMesh>());
                }

                linkPoints.Add(previousPortalPoint);
                IncreaseLinkLength(0, startPoint2);
            }
            hasTakePortal = false;
        }

        /// <summary>
        /// Consume energy and increase the link length, broke if it collides another link or if there is not enough energy.
        /// Return false if the link has been destroyed in the process, return true otherwise.
        /// </summary>
        public bool IncreaseLinkLength(float distance, Vector3 pointToAdd)
        {
            distanceWalk -= distance;
            totalDistance += distance;
            if (energy.AddToValue(-distance)) // We have enough energy
            {
                linkPoints.Add(pointToAdd);
                UpdateLink();
                return !CheckLinkIntersection();
            }
            else // You don't have enough energy, the link broke.
            {
                PlaySound(soundLackEnergy);
                DestroyCurrentLink();
                helper.EnergyEmpty();
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
                List<Link> roomLinks = allLinks[roomManager.GetCurrentRoomID()];
                for (int l = 0; l < roomLinks.Count; l++)
                {
                    if (roomLinks[l].Intersect(currentLinkLines))
                    {
                        DestroyCurrentLink();
                        helper.LinkIntersectAndBroke();
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
            PlaySound(soundLinkBroke);

            // Link destruction
            for (int lc = linkCurves.Count - 1; lc >= 0; lc--)
            {
                LinkCurve linkCurve = linkCurves[lc];
                // Effects
                if (effectBrokeLinkSmall != null)
                {
                    int c = 0;
                    for (int pointID = 0; pointID < linkCurve.points.Length; pointID += 2)
                    {
                        for (int i = 0; i < 6; i++)
                            GameObject.Instantiate(effectBrokeLinkSmall, linkCurve.GetPoint(c, ((float)i) / 5.0f), Quaternion.identity);
                        c++;
                    }
                    if (effectBrokeLink != null)
                        GameObject.Instantiate(effectBrokeLink, linkPoints[linkPoints.Count - 1], Quaternion.identity);
                }
                // Destroy link
                GameObject.Destroy(linkCurve.gameObject);
            }
            for (int pl = 0; pl < previousLinks.Count; pl++)
            {
                for (int lc = previousLinks[pl].linkGameObjects.Count - 1; lc >= 0; lc--)
                {
                    LinkCurve linkCurve = previousLinks[pl].linkGameObjects[lc].GetComponent<LinkCurve>();
                    // Effects
                    if (effectBrokeLinkSmall != null)
                    {
                        int c = 0;
                        for (int pointID = 0; pointID < linkCurve.points.Length; pointID += 2)
                        {
                            for (int i = 0; i < 6; i++)
                                GameObject.Instantiate(effectBrokeLinkSmall, linkCurve.GetPoint(c, ((float)i) / 5.0f), Quaternion.identity);
                            c++;
                        }
                    }
                    // Destroy link
                    GameObject.Destroy(linkCurve.gameObject);
                }
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
            for (int lc = 0; lc < linkCurves.Count; lc++)
            {
                // Initialization
                LinkCurve linkCurve = linkCurves[lc];
                LinkMesh linkMesh = linkMeshes[lc];
                List<Vector3> linkPointsWithGravity = new List<Vector3>();

                // Recreate the point list with a decalage for gravity, based on eloignment from link extremities
                for (int v = 0; v < linkPoints.Count; v++)
                {
                    Vector3 vec = linkPoints[v];
                    if ((v != 0 && v != linkPoints.Count - 1) && (v != 0 && v != linkPoints.Count - 1))
                    {
                        float percent = (float)v / (float)(linkPoints.Count - 1);
                        percent = Mathf.Cos((2 * Mathf.PI * percent) - Mathf.PI);
                        percent += lc * gravityFactor * percent;
                        vec = vec + gravityOffset + gravityOffset * percent;
                    }
                    linkPointsWithGravity.Add(vec);
                }

                // Calculate how many curves the link contains.
                int curves = Mathf.FloorToInt(linkPoints.Count / linkCurveLength);
                if (linkPointsWithGravity.Count % linkCurveLength != 0)
                    curves++;

                // Fill the curves points
                List<Vector3> curvePoints = new List<Vector3>();
                List<Vector3> curveModifiers = new List<Vector3>();
                for (int c = 0; c < curves; c++)
                {
                    // Find curve extremities
                    int aID = c * linkCurveLength;
                    int bID = ((c == curves - 1) ? linkPointsWithGravity.Count - 1 : (c + 1) * linkCurveLength - 1);
                    Vector3 a = linkPointsWithGravity[aID];
                    Vector3 b = linkPointsWithGravity[bID];

                    // Find farthest point from the line defined by the extremities of the curve. This point will be used to define the curvature.
                    int longestID = -1;
                    float longestDist = 0;
                    for (int i = aID; i < bID; i++)
                    {
                        float distance = Vector3.Cross((b - a), linkPointsWithGravity[i] - a).magnitude;
                        if (distance > longestDist)
                        {
                            longestDist = distance;
                            longestID = i;
                        }
                    }
                    if (longestID == -1)
                        longestID = bID;

                    // Add thoses points to the curves points lists
                    curvePoints.Add(a);
                    curvePoints.Add(b);
                    curveModifiers.Add(linkPointsWithGravity[longestID]);
                    curveModifiers.Add(linkPointsWithGravity[longestID]);

                }
                linkCurve.SetCurves(curvePoints.ToArray(), curveModifiers.ToArray());
                linkMesh.Regenerate();
            }
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
                GameObject linkGameObject = GameObject.Instantiate(linkPrefab, roomManager.currentRoom.GetLinksRepository());
                linkCurves.Add(linkGameObject.GetComponent<LinkCurve>());
                linkMeshes.Add(linkGameObject.GetComponent<LinkMesh>());
            }

            // Add first points
            helper.SetIsLinking(true);
            isLinking = true;
            linkPoints.Add(linkablePos);
            IncreaseLinkLength(Vector3.Distance(linkablePos, target.position), target.position);
        }

        /// <summary>
        /// End the link and return if the link has been correctly linked.
        /// </summary>
        /// <param name="linkablePos"></param>
        /// <returns></returns>
        public bool StopLinking(Vector3 linkablePos)
        {
            helper.SetIsLinking(false);
            isLinking = false;
            Vector3 lastLinkPoint = linkPoints[linkPoints.Count - 1];

            if (IncreaseLinkLength(Vector3.Distance(lastLinkPoint, linkablePos), linkablePos))
            {
                // GameObjects of links created
                List<GameObject> linksGO = new List<GameObject>();
                foreach (LinkCurve linkCurve in linkCurves)
                    linksGO.Add(linkCurve.gameObject);

                // Collider defined by the lines of the link created
                List<Vector2> linkCollider = new List<Vector2>();
                for (int p = 0; p < linkPoints.Count; p += linkCurveLength)
                {
                    linkCollider.Add(new Vector2(linkPoints[p].x, linkPoints[p].z));
                    if (p + linkCurveLength >= linkPoints.Count)
                        linkCollider.Add(new Vector2(linkPoints[linkPoints.Count - 1].x, linkPoints[linkPoints.Count - 1].z));
                }

                // Remember them
                allLinks[roomManager.GetCurrentRoomID()].Add(new Link(roomManager.GetCurrentRoomID(), linksGO, linkCollider));
                for (int l = 0; l < previousLinks.Count; l++)
                    allLinks[previousLinks[l].roomID].Add(previousLinks[l]);

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
            linkCurves.Clear();
            linkMeshes.Clear();
            previousLinks.Clear();
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
            bool canInteract = false;
            Ray ray = new Ray(transform.position, linkablePos - transform.position);
            RaycastHit rayHit = new RaycastHit();
            if (Physics.Raycast(ray, out rayHit, 30, LayerMask.GetMask(new string[] { LayerMask.LayerToName(clicked.layer) })))
            {
                if (rayHit.distance <= distanceInteraction)
                    canInteract = true;
            }

            // Too Far
            if (!canInteract)
            {
                helper.ClickLinkable_TooFar();
                return; // Too far away to interact with
            }

            if (isLinking)
            {
                if (destination == clicked) // End the link on the good Linkable.
                {
                    if (StopLinking(linkablePos))
                    {
                        PlaySound(soundClick);
                        PlaySound(soundLinkCorrectEnd);
                        PlaySound(soundMemoryCompleted);
                        PlaySound(soundEnergyIncrease);

                        helper.ClickLinkable_Valid();

                        // Effect
                        if (effectStartLink != null)
                            GameObject.Instantiate(effectStartLink, linkablePos, Quaternion.Euler(-90, 0, 0), null);

                        // Energy (Increase max energy and refill energy)
                        energy.IncreaseEnergyLevel();
                        energy.Fill();

                        // Link the Linkables
                        clicked.GetComponent<Linkable>().Linked();
                        clickedPair.GetComponent<Linkable>().Linked();
                    }
                }
                else // Try to end the link on a wrong Linkable.
                {
                    PlaySound(soundClickWrongLinkable);
                    helper.ClickLinkable_WrongPair();
                }
            }
            else // Start a link on a Linkable.
            {
                if (clicked.GetComponent<Linkable>().IsAlreadyLinked()) // Can't add a link, this linkable is already linked.
                {
                    PlaySound(soundClickAlreadyLinkedLinkable);
                    helper.ClickLinkable_AlreadyLinked();
                }
                else if (clicked.GetComponent<Linkable>().pair != null) // Add the link on this linkable.
                {
                    PlaySound(soundClick);
                    PlaySound(soundLinkContinuous);
                    helper.ClickLinkable_Valid();

                    // Effect
                    if (effectEndLink != null)
                        GameObject.Instantiate(effectEndLink, linkablePos, Quaternion.Euler(-90, 0, 0), null);

                    // Register the Linkable origin and the Linkable that should be the destination and start the link.
                    origin = clicked;
                    destination = clickedPair;
                    StartLinking(linkablePos);
                }
                else // The linkable doesn't have a Pair Linkable
                {
                    PlaySound(soundClickWrongLinkable);
                }
            }
        }

        /// <summary>
        /// Return if the linker is currently holding a link
        /// </summary>
        /// <returns></returns>
        public bool IsLinking()
        {
            return isLinking;
        }

        public bool HasTakePortal()
        {
            return hasTakePortal;
        }
        #endregion
    }
}
