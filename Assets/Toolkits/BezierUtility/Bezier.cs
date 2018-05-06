using UnityEngine;

namespace BezierUtility
{
    public static class Bezier
    {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * p0 +
                2f * oneMinusT * t * p1 +
                t * t * p2;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return
                2f * (1f - t) * (p1 - p0) +
                2f * t * (p2 - p1);
        }

        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float OneMinusT = 1f - t;
            return
                OneMinusT * OneMinusT * OneMinusT * p0 +
                3f * OneMinusT * OneMinusT * t * p1 +
                3f * OneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }



        public static Vector3 GetPoint(Vector3[] pts, float t)
        {
            float omt = 1f - t;
            float omt2 = omt * omt;
            float t2 = t * t;
            return pts[0] * (omt2 * omt) +
                    pts[1] * (3f * omt2 * t) +
                    pts[2] * (3f * omt * t2) +
                    pts[3] * (t2 * t);
        }

        public static Vector3 GetTangent(Vector3[] pts, float t)
        {
            float omt = 1f - t;
            float omt2 = omt * omt;
            float t2 = t * t;
            Vector3 tangent =
                        pts[0] * (-omt2) +
                        pts[1] * (3 * omt2 - 2 * omt) +
                        pts[2] * (-3 * t2 + 2 * t) +
                        pts[3] * (t2);
            return tangent.normalized;
        }

        public static Vector3 GetNormal3D(Vector3[] pts, float t, Vector3 up)
        {
            Vector3 tng = GetTangent(pts, t);
            Vector3 binormal = Vector3.Cross(up, tng).normalized;
            return Vector3.Cross(tng, binormal);
        }

        public static Quaternion GetOrientation3D(Vector3[] pts, float t, Vector3 up)
        {
            Vector3 tng = GetTangent(pts, t);
            Vector3 nrm = GetNormal3D(pts, t, up);
            return Quaternion.LookRotation(tng, nrm);
        }
    }
}