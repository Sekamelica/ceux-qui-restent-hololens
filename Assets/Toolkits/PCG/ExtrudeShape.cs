using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    public enum MeshFaces
    {
        Outside,
        Inside,
        Doublesided
    }

    public enum Shape
    {
        Square_Intern,
        Square_Extern,
        Circle_Intern,
        Circle_Extern,
        Bridge_Extern,
        Bridge_Contour,
        Bridge_Road
    }

    public class ExtrudeShape
    {
        public List<Vector2> points;
        public List<Vector3> normals;
        public int[] lines;


        public ExtrudeShape(List<Vector2> _points, List<Vector3> _normals, int[] _lines)
        {
            points = _points;
            normals = _normals;
            lines = _lines;
        }

        public static ExtrudeShape GetShape(Shape shape, Vector2 scale, int pointsAmount)
        {
            switch(shape)
            {
                // Square
                case Shape.Square_Extern:
                    return CreateSquareShape_Extern(scale);
                case Shape.Square_Intern:
                    return CreateSquareShape_Intern(scale);
                
                // Circle
                case Shape.Circle_Extern:
                    return CreateCircleShape_Extern(pointsAmount, scale);
                case Shape.Circle_Intern:
                    return CreateCircleShape_Intern(pointsAmount, scale);

                // Bridge
                case Shape.Bridge_Extern:
                    return CreateBridgeShape_Extern(scale);
                case Shape.Bridge_Contour:
                    return CreateBridgeShape_Contour(scale);
                case Shape.Bridge_Road:
                    return CreateBridgeShape_Road(scale);

                default:
                    return CreateSquareShape_Extern(scale);
            }
        }

        

        public static ExtrudeShape CreateSquareShape_Extern(Vector2 scale)
        {
            List<Vector2> squarePoints = new List<Vector2>()
            {
                new Vector2(-1, -1),
                new Vector2(1, -1),

                new Vector2(1, -1),
                new Vector2(1, 1),

                new Vector2(1, 1),
                new Vector2(-1, 1),

                new Vector2(-1, 1),
                new Vector2(-1, -1)
            };
            for(int sp = 0; sp < squarePoints.Count; sp++)
                squarePoints[sp] = new Vector2(squarePoints[sp].x * scale.x, squarePoints[sp].y * scale.y);
            List<Vector3> squareNormals = new List<Vector3>()
            {
                Vector3.forward,
                Vector3.forward,

                Vector3.left,
                Vector3.left,

                Vector3.back,
                Vector3.back,

                Vector3.right,
                Vector3.right
            };
            int[] squareLines = new int[]{
                0, 1,
                2, 3,
                4, 5,
                6, 7
            };
            
            return new ExtrudeShape(squarePoints, squareNormals, squareLines);
        }

        public static ExtrudeShape CreateSquareShape_Intern(Vector2 scale)
        {
            List<Vector2> squarePoints = new List<Vector2>()
            {
                new Vector2(1, -1),
                new Vector2(-1, -1),

                new Vector2(1, 1),
                new Vector2(1, -1),

                new Vector2(-1, 1),
                new Vector2(1, 1),

                new Vector2(-1, -1),
                new Vector2(-1, 1)

            };
            for (int sp = 0; sp < squarePoints.Count; sp++)
                squarePoints[sp] = new Vector2(squarePoints[sp].x * scale.x, squarePoints[sp].y * scale.y);
            List<Vector3> squareNormals = new List<Vector3>()
            {
                Vector3.back,
                Vector3.back,

                Vector3.right,
                Vector3.right,

                Vector3.forward,
                Vector3.forward,

                Vector3.left,
                Vector3.left
            };
            int[] squareLines = new int[]{
                0, 1,
                2, 3,
                4, 5,
                6, 7
            };

            return new ExtrudeShape(squarePoints, squareNormals, squareLines);
        }

        public static ExtrudeShape CreateCircleShape_Extern(int pointsAmount, Vector2 scale)
        {
            List<Vector2> circlePoints = new List<Vector2>();
            List<Vector3> circleNormals = new List<Vector3>();
            List<int> circleLines = new List<int>();

            for (int i = 0; i < pointsAmount; i++)
            {
                float angleDegree = (360 / pointsAmount) * i;
                float angleRad = angleDegree * Mathf.PI / 180;
                float point_x = scale.x * Mathf.Cos(angleRad);
                float point_y = scale.y * Mathf.Sin(angleRad);

                circlePoints.Add(new Vector2(point_x, point_y));
                circleLines.Add(i);
                if (i == pointsAmount - 1)
                    circleLines.Add(0);
                else
                    circleLines.Add(i + 1);
                circleNormals.Add(Vector3.zero);
            }

            return new ExtrudeShape(circlePoints, circleNormals, circleLines.ToArray());
        }

        public static ExtrudeShape CreateCircleShape_Intern(int pointsAmount, Vector2 scale)
        {
            List<Vector2> circlePoints = new List<Vector2>();
            List<Vector3> circleNormals = new List<Vector3>();
            List<int> circleLines = new List<int>();

            for (int i = 0; i < pointsAmount; i++)
            {
                float angleDegree = 360 - (360 / pointsAmount) * i;
                float angleRad = angleDegree * Mathf.PI / 180;
                float point_x = scale.x * Mathf.Cos(angleRad);
                float point_y = scale.y * Mathf.Sin(angleRad);

                circlePoints.Add(new Vector2(point_x, point_y));
                circleLines.Add(i);
                if (i == pointsAmount - 1)
                    circleLines.Add(0);
                else
                    circleLines.Add(i + 1);
                circleNormals.Add(Vector3.zero);
            }

            return new ExtrudeShape(circlePoints, circleNormals, circleLines.ToArray());
        }

        public static ExtrudeShape CreateBridgeShape_Extern(Vector2 scale)
        {
            List<Vector2> bridgePoints = new List<Vector2>()
            {
                new Vector2(0.5f, 0.9f),
                new Vector2(0.25f, 1),

                new Vector2(0.25f, 1),
                new Vector2(0, 1),

                new Vector2(0, 1),
                new Vector2(0.25f, 0),

                new Vector2(0.25f, 0),
                new Vector2(1, 0.25f),

                new Vector2(1, 0.25f),
                new Vector2(1.25f, 0.25f),

                new Vector2(1.25f, 0.25f),
                new Vector2(2.75f, 0.25f),

                new Vector2(2.75f, 0.25f),
                new Vector2(3, 0),

                new Vector2(3, 0),
                new Vector2(3.75f, 0),

                new Vector2(3.75f, 0),
                new Vector2(4, 1),

                new Vector2(4, 1),
                new Vector2(3.75f, 1),

                new Vector2(3.75f, 1),
                new Vector2(3.5f, 0.9f),

                new Vector2(3.5f, 0.9f),
                new Vector2(0.5f, 0.9f)
            };
            for(int bp = 0; bp < bridgePoints.Count; bp++)
                bridgePoints[bp] += new Vector2(-2, -0.5f);
            
            List<Vector3> bridgeNormals = new List<Vector3>();

            for (int i = 0; i < bridgePoints.Count; i++)
            {
                bridgePoints[i] = new Vector2(bridgePoints[i].x * scale.x, bridgePoints[i].y * scale.y);
                bridgeNormals.Add(Vector3.up);
            }
            int[] bridgeLines = new int[]{
                0, 1,
                2, 3,
                4, 5,
                6, 7,
                8, 9,
                10, 11,
                12, 13,
                14, 15,
                16, 17,
                18, 19,
                20, 21,
                22, 23
            };

            return new ExtrudeShape(bridgePoints, bridgeNormals, bridgeLines);
        }

        public static ExtrudeShape CreateBridgeShape_Contour(Vector2 scale)
        {
            List<Vector2> bridgePoints = new List<Vector2>()
            {
                new Vector2(0.5f, 0.9f),
                new Vector2(0.25f, 1),

                new Vector2(0.25f, 1),
                new Vector2(0, 1),

                new Vector2(0, 1),
                new Vector2(0.25f, 0),

                new Vector2(0.25f, 0),
                new Vector2(1, 0.25f),

                new Vector2(1, 0.25f),
                new Vector2(1.25f, 0.25f),

                new Vector2(1.25f, 0.25f),
                new Vector2(2.75f, 0.25f),

                new Vector2(2.75f, 0.25f),
                new Vector2(3, 0),

                new Vector2(3, 0),
                new Vector2(3.75f, 0),

                new Vector2(3.75f, 0),
                new Vector2(4, 1),

                new Vector2(4, 1),
                new Vector2(3.75f, 1),

                new Vector2(3.75f, 1),
                new Vector2(3.5f, 0.9f)
            };
            for (int bp = 0; bp < bridgePoints.Count; bp++)
            {
                bridgePoints[bp] += new Vector2(-2, -0.5f);
                bridgePoints[bp] = new Vector2(bridgePoints[bp].x * scale.x, bridgePoints[bp].y * scale.y);
            }
            List<Vector3> bridgeNormals = new List<Vector3>();

            for (int i = 0; i < bridgePoints.Count; i++)
            {
                bridgePoints[i] = new Vector2(bridgePoints[i].x * scale.x, bridgePoints[i].y * scale.y);
                bridgeNormals.Add(Vector3.up);
            }
            int[] bridgeLines = new int[]{
                0, 1,
                2, 3,
                4, 5,
                6, 7,
                8, 9,
                10, 11,
                12, 13,
                14, 15,
                16, 17,
                18, 19,
                20, 21
            };

            return new ExtrudeShape(bridgePoints, bridgeNormals, bridgeLines);
        }

        public static ExtrudeShape CreateBridgeShape_Road(Vector2 scale)
        {
            List<Vector2> bridgePoints = new List<Vector2>()
            {
                new Vector2(3.5f, 0.9f),
                new Vector2(0.5f, 0.9f)
            };
            for (int bp = 0; bp < bridgePoints.Count; bp++)
            {
                bridgePoints[bp] += new Vector2(-2, -0.5f);
                bridgePoints[bp] = new Vector2(bridgePoints[bp].x * scale.x, bridgePoints[bp].y * scale.y);
            }

            List<Vector3> bridgeNormals = new List<Vector3>()
            {
                Vector3.up,
                Vector3.up
            };

            int[] bridgeLines = new int[]{
                0, 1   
            };

            return new ExtrudeShape(bridgePoints, bridgeNormals, bridgeLines);
        }

        public Mesh Extrude(OrientedPoint[] path)
        {
            // Init
            int vertsInShape = points.Count;
            int segments = path.Length - 1;
            int edgeLoops = path.Length;
            int vertCount = vertsInShape * edgeLoops;
            int triCount = lines.Length * segments;
            int triIndexCount = triCount * 3;

            // Init2
            int[] triangleIndices = new int[triIndexCount];
            Vector3[] vertices = new Vector3[vertCount];
            Vector3[] normals = new Vector3[vertCount];
            //Vector2[] uvs = new Vector2[vertCount];


            // Mesh Generation
            for (int i = 0; i < path.Length; i++)
            {
                int offset = i * vertsInShape;
                for (int j = 0; j < vertsInShape; j++)
                {
                    int id = offset + j;
                    vertices[id] = path[i].LocalToWorld(points[j]);
                    normals[id] = path[i].LocalToWorldDirection(normals[j]);
                    //uvs[id] = new Vector2(vert2Ds[j].uCoord, i / ((float)edgeLoops));
                }
            }
            int ti = 0;
            for (int i = 0; i < segments; i++)
            {
                int offset = i * vertsInShape;
                for (int l = 0; l < lines.Length; l += 2)
                {
                    int a = offset + lines[l] + vertsInShape;
                    int b = offset + lines[l];
                    int c = offset + lines[l + 1];
                    int d = offset + lines[l + 1] + vertsInShape;
                    triangleIndices[ti] = a; ti++;
                    triangleIndices[ti] = b; ti++;
                    triangleIndices[ti] = c; ti++;
                    triangleIndices[ti] = c; ti++;
                    triangleIndices[ti] = d; ti++;
                    triangleIndices[ti] = a; ti++;
                }
            }

            // Assign
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangleIndices;
            mesh.normals = normals;
            mesh.RecalculateNormals();
            //mesh.uv = uvs;
            return mesh;
        }

    }
}