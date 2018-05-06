using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierUtility;

namespace PCG
{
    public class BezierMesh : MonoBehaviour
    {
        public Material material;
        public int echantillonage = 4;
        public Shape shape = Shape.Square_Extern;
        public int pointsAmount = 12;
        public Vector2 scale = new Vector2(1, 1);
        public BezierCurve curve;
        public GameObject bezierMeshGO = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Regenerate()
        {
            if (curve == null)
                curve = GetComponent<BezierCurve>();
            if (curve == null)
                return;
            if (bezierMeshGO != null)
                DestroyImmediate(bezierMeshGO);

            if (transform.position != Vector3.zero)
            {
                for(int i = 0; i < curve.points.Length; i++)
                {
                    curve.points[i] += transform.position;
                }
                transform.position = Vector3.zero;
            }


            List<OrientedPoint> pathList = new List<OrientedPoint>();
            for(int i = 0; i < echantillonage; i++)
            {
                OrientedPoint op = new OrientedPoint();
                float i_float = i;
                float echantillonage_float = echantillonage-1;
                float t = (i_float / echantillonage_float);

                op.position = curve.GetPoint(t);
                op.rotation = Bezier.GetOrientation3D(curve.points, t, Vector3.up);

                pathList.Add(op);
            }
            bezierMeshGO = new GameObject("BezierMesh");
            bezierMeshGO.transform.parent = transform;
            MeshFilter bezierMeshFilter = bezierMeshGO.AddComponent<MeshFilter>();
            MeshRenderer bezierMeshRenderer = bezierMeshGO.AddComponent<MeshRenderer>();
            MeshCollider bezierMeshCollider = bezierMeshGO.AddComponent<MeshCollider>();
            bezierMeshFilter.mesh = ExtrudeShape.GetShape(shape, scale, pointsAmount).Extrude(pathList.ToArray());
            bezierMeshRenderer.material = material;
            bezierMeshCollider.sharedMesh = bezierMeshFilter.sharedMesh;
            return;
        }

    }
}