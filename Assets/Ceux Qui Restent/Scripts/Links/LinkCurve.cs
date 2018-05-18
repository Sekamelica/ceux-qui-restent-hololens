using UnityEngine;
using BezierUtility;

namespace CeuxQuiRestent.Links
{
    public class LinkCurve : MonoBehaviour
    {
        #region Attributes
        public Vector3[] points;
        public Vector3[] modifiers;
        #endregion

        #region Methods
        public Vector3 GetPoint(int curve, float t)
        {
            return transform.TransformPoint(Bezier.GetPoint(points[(2 * curve) + 0], modifiers[(2 * curve) + 0], modifiers[(2 * curve) + 1], points[(2 * curve) + 1], t));
        }

        public Vector3 GetVelocity(int curve, float t)
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(points[(2 * curve) + 0], modifiers[(2 * curve) + 0], modifiers[(2 * curve) + 1], points[(2 * curve) + 1], t)) - transform.position;
        }

        public Vector3 GetDirection(int curve, float t)
        {
            return GetVelocity(curve, t).normalized;
        }

        public void Reset()
        {
            points = new Vector3[] {
                new Vector3(1f, 0f, 0f),
                new Vector3(4f, 0f, 0f)
            };
            modifiers = new Vector3[]
            {
                new Vector3(2f, 0f, 0f),
                new Vector3(3f, 0f, 0f)
            };
        }

        public void SetOriginEnd(Vector3 origin, Vector3 end)
        {
            points = new Vector3[] {
                origin,
                end
            };
            modifiers = new Vector3[]
            {
                Vector3.Lerp(origin, end, 0.25f),
                Vector3.Lerp(origin, end, 0.75f)
            };
        }

        public void SetCurves(Vector3[] curvesPoints, Vector3[] curveModifiers)
        {
            points = curvesPoints;
            modifiers = curveModifiers;
        }
        #endregion
    }
}