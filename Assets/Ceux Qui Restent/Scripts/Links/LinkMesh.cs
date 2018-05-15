using BezierUtility;
using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Gameplay
{
    [ExecuteInEditMode]
    public class LinkMesh : MonoBehaviour
    {
        #region Attributes
        [Header("Visuals")]
        public Material material;
        public Shape shape = Shape.Circle_Extern;
        public Vector2 scale = new Vector2(1, 1);

        [Space]
        [Header("Precision")]
        public int echantillonage = 4;
        public int pointsAmount = 3;
        
        [Space]
        [Header("Curve and mesh")]
        public LinkCurve curve;
        public GameObject meshGameObject = null;
        public bool regenerate = false;
        #endregion

        #region Methods
        private void Update()
        {
            if (regenerate)
            {
                regenerate = false;
                Regenerate();
            }
        }

        public void Regenerate()
        {
            if (curve == null)
                curve = GetComponent<LinkCurve>();
            if (curve == null)
                return;
            if (meshGameObject != null)
                DestroyImmediate(meshGameObject);

            if (transform.position != Vector3.zero)
            {
                for (int i = 0; i < curve.points.Length; i++)
                    curve.points[i] += transform.position;
                transform.position = Vector3.zero;
            }

            List<OrientedPoint> pathList = new List<OrientedPoint>();
            int curveID = 0;
            for (int p = 0; p < curve.points.Length - 1; p += 2)
            {
                for (int i = 0; i < echantillonage; i++)
                {
                    OrientedPoint op = new OrientedPoint();
                    float i_float = i;
                    float echantillonage_float = echantillonage - 1;
                    float t = (i_float / echantillonage_float);

                    op.position = curve.GetPoint(curveID, t);
                    op.rotation = Bezier.GetOrientation3D(new Vector3[] { curve.points[p], curve.modifiers[p], curve.modifiers[p + 1], curve.points[p + 1] }, t, Vector3.up);

                    pathList.Add(op);
                }

                curveID++;
            }
            meshGameObject = MeshUtility.GenerateMeshGameObject(transform, "LinkMesh", false, material, ExtrudeShape.GetShape(shape, scale, pointsAmount).Extrude(pathList.ToArray()));
            pathList.Clear();
            return;
        }
        #endregion
    }
}
