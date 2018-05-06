using UnityEngine;

namespace BezierUtility
{
    public class BezierMultiCurves : MonoBehaviour
    {
        public Vector3[] points;

        public Vector3 GetPoint(int curve, float t)
        {
            return transform.TransformPoint(Bezier.GetPoint(points[0 + curve * 4], points[1 + curve * 4], points[2 + curve * 4], points[3 + curve * 4], t));
        }

        public Vector3 GetVelocity(int curve, float t)
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(points[0 + curve * 4], points[1 + curve * 4], points[2 + curve * 4], points[3 + curve * 4], t)) - transform.position;
        }

        public Vector3 GetDirection(int curve, float t)
        {
            return GetVelocity(curve, t).normalized;
        }

        public void Reset()
        {
            points = new Vector3[] {
                new Vector3(1f, 0f, 0f),
                new Vector3(2f, 0f, 0f),
                new Vector3(3f, 0f, 0f),
                new Vector3(4f, 0f, 0f)
            };
        }

        public void SetOriginEnd(Vector3 origin, Vector3 end)
        {
            points = new Vector3[] {
                origin,
                Vector3.Lerp(origin, end, 0.25f),
                Vector3.Lerp(origin, end, 0.75f),
                end
            };
        }

        public void SetCurves(Vector3[] curvesPoints)
        {
            points = curvesPoints;
        }
    }
}