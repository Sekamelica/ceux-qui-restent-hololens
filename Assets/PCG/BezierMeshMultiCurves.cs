using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierUtility;

namespace PCG
{
    public class BezierMeshMultiCurves : MonoBehaviour
    {
        public float animationIntensity = 1f;
        public float animationSpeed = 1f;
        public bool gravityAnimation = false;
        public Material material;
        public int echantillonage = 4;
        public Shape shape = Shape.Square_Extern;
        public int pointsAmount = 12;
        public Vector2 scale = new Vector2(1, 1);
        public BezierMultiCurves curve;
        public GameObject bezierMeshGO = null;
        private Vector3 randomAnim = new Vector3();

        // Use this for initialization
        void Start()
        {
            randomAnim = new Vector3(Random.Range(0.0f, 12.0f), Random.Range(0.0f, 12.0f), Random.Range(0.0f, 12.0f));
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (gravityAnimation)
            {
                MeshFilter mf = bezierMeshGO.GetComponent<MeshFilter>();
                Mesh m = mf.sharedMesh;
                List<Vector3> vertices = new List<Vector3>();
                m.GetVertices(vertices);
                for (int v = 0; v < vertices.Count; v++)
                {
                    float percent = (float)v / (float)(vertices.Count - 1);
                    float animX = percent * animationIntensity * Mathf.Sin(animationSpeed * (Time.timeSinceLevelLoad + randomAnim.x));
                    float animY = percent * animationIntensity * Mathf.Sin(animationSpeed * (Time.timeSinceLevelLoad + randomAnim.y));
                    float animZ = percent * animationIntensity * Mathf.Sin(animationSpeed * (Time.timeSinceLevelLoad + randomAnim.z));
                    vertices[v] = new Vector3(vertices[v].x + animX, vertices[v].y + animY, vertices[v].z - animZ);
                }
                m.SetVertices(vertices);
                m.RecalculateNormals();
                mf.sharedMesh = m;
            }
        }

        public void Regenerate()
        {
            if (curve == null)
                curve = GetComponent<BezierMultiCurves>();
            if (curve == null)
                return;
            if (bezierMeshGO != null)
                DestroyImmediate(bezierMeshGO);

            if (transform.position != Vector3.zero)
            {
                for (int i = 0; i < curve.points.Length; i++)
                    curve.points[i] += transform.position;
                transform.position = Vector3.zero;
            }

            List<OrientedPoint> pathList = new List<OrientedPoint>();
            int curveID = 0;
            for (int p = 0; p < curve.points.Length; p += 4)
            {
                for (int i = 0; i < echantillonage; i++)
                {
                    OrientedPoint op = new OrientedPoint();
                    float i_float = i;
                    float echantillonage_float = echantillonage - 1;
                    float t = (i_float / echantillonage_float);

                    op.position = curve.GetPoint(curveID, t);
                    op.rotation = Bezier.GetOrientation3D(new Vector3[] { curve.points[p], curve.points[p + 1], curve.points[p + 2], curve.points[p + 3] }, t, Vector3.up);

                    pathList.Add(op);
                }
                
                curveID++;
            }
            bezierMeshGO = MeshUtility.GenerateMeshGameObject(transform, "LinkMesh", false, material, ExtrudeShape.GetShape(shape, scale, pointsAmount).Extrude(pathList.ToArray()));
            pathList.Clear();
            return;
        }

    }
}