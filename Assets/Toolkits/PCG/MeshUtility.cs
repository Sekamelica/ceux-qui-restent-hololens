using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    public class MeshUtility
    {
        public static Mesh CombineMeshes(List<Mesh> meshes)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            for (int m = 0; m < meshes.Count; m++)
            {
                int verticesCountAtStart = vertices.Count;
                Mesh m_mesh = meshes[m];
                Vector3[] m_vertices = m_mesh.vertices;
                int[] m_triangles = m_mesh.triangles;
                Vector2[] m_uvs = m_mesh.uv;
                foreach (Vector3 v in m_vertices)
                    vertices.Add(v);
                for(int tid = 0; tid < m_triangles.Length; tid++)
                    triangles.Add(m_triangles[tid] + verticesCountAtStart);
                foreach (Vector2 uv in m_uvs)
                    uvs.Add(uv);
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.uv = uvs.ToArray();
            return mesh;
        }

        public static Mesh CombineMeshes(List<GameObject> meshesGO)
        {
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            for (int qgo = 0; qgo < meshesGO.Count; qgo++)
            {
                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = meshesGO[qgo].GetComponent<MeshFilter>().sharedMesh;
                combineInstance.transform = meshesGO[qgo].transform.localToWorldMatrix;
                combineInstances.Add(combineInstance);
            }
            Mesh combinaison = new Mesh();
            combinaison.CombineMeshes(combineInstances.ToArray());
            return combinaison;
        }

        public static GameObject GenerateMeshGameObject(Transform parent, string nameGO, bool collide, Material material, Mesh mesh)
        {
            GameObject meshGO = new GameObject(nameGO);

            if(parent != null)
                meshGO.transform.parent = parent;

            MeshFilter meshGO_meshFilter = meshGO.AddComponent<MeshFilter>();
            MeshRenderer meshGO_meshRenderer = meshGO.AddComponent<MeshRenderer>();

            meshGO_meshFilter.mesh = mesh;
            meshGO_meshRenderer.sharedMaterial = material;

            if(collide)
            {
                MeshCollider meshGO_meshCollider = meshGO.AddComponent<MeshCollider>();
                meshGO_meshCollider.sharedMesh = meshGO_meshFilter.sharedMesh;
            }
            return meshGO;
        }

        public static List<Mesh> GenerateCube(bool[] faces, Vector3 offset, Vector3 scale, MeshFaces meshFaces)
        {
            List<Mesh> quads = new List<Mesh>();
            Vector3 demiScale = scale / 2;

            if (meshFaces == MeshFaces.Outside || meshFaces == MeshFaces.Doublesided)
            {
                // Ground
                if (faces[0])
                    quads.Add(GenerateQuad(offset + new Vector3(0, -demiScale.y, 0), new Vector3(scale.x, 0, scale.z), false));
                // Ceiling
                if (faces[1])
                    quads.Add(GenerateQuad(offset + new Vector3(0, demiScale.y, 0), new Vector3(scale.x, 0, scale.z), true));

                // Forward
                if (faces[2])
                    quads.Add(GenerateQuad(offset + new Vector3(0, 0, -demiScale.z), new Vector3(scale.x, scale.y, 0), true));
                // Backward
                if (faces[3])
                    quads.Add(GenerateQuad(offset + new Vector3(0, 0, demiScale.z), new Vector3(scale.x, scale.y, 0), false));

                // Left
                if (faces[4])
                    quads.Add(GenerateQuad(offset + new Vector3(-demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), false));
                // Right
                if (faces[5])
                    quads.Add(GenerateQuad(offset + new Vector3(demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), true));
            }
            if (meshFaces == MeshFaces.Inside || meshFaces == MeshFaces.Doublesided)
            {
                // Ground
                if (faces[0])
                    quads.Add(GenerateQuad(offset + new Vector3(0, -demiScale.y, 0), new Vector3(scale.x, 0, scale.z), true));
                // Ceiling
                if (faces[1])
                    quads.Add(GenerateQuad(offset + new Vector3(0, demiScale.y, 0), new Vector3(scale.x, 0, scale.z), false));

                // Forward
                if (faces[2])
                    quads.Add(GenerateQuad(offset + new Vector3(0, 0, -demiScale.z), new Vector3(scale.x, scale.y, 0), false));
                // Backward
                if (faces[3])
                    quads.Add(GenerateQuad(offset + new Vector3(0, 0, demiScale.z), new Vector3(scale.x, scale.y, 0), true));

                // Left
                if (faces[4])
                    quads.Add(GenerateQuad(offset + new Vector3(-demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), true));
                // Right
                if (faces[5])
                    quads.Add(GenerateQuad(offset + new Vector3(demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), false));
            }

            return quads;
        }

        public static List<Mesh> GenerateCube(Vector3 offset, Vector3 scale, MeshFaces meshFaces)
        {
            List<Mesh> quads = new List<Mesh>();
            Vector3 demiScale = scale / 2;
            
            if (meshFaces == MeshFaces.Outside || meshFaces == MeshFaces.Doublesided)
            {
                // Ground
                quads.Add(GenerateQuad(offset + new Vector3(0, -demiScale.y, 0), new Vector3(scale.x, 0, scale.z), false));
                // Ceiling
                quads.Add(GenerateQuad(offset + new Vector3(0, demiScale.y, 0), new Vector3(scale.x, 0, scale.z), true));

                // Forward
                quads.Add(GenerateQuad(offset + new Vector3(0, 0, -demiScale.z), new Vector3(scale.x, scale.y, 0), true));
                // Backward
                quads.Add(GenerateQuad(offset + new Vector3(0, 0, demiScale.z), new Vector3(scale.x, scale.y, 0), false));

                // Left
                quads.Add(GenerateQuad(offset + new Vector3(-demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), false));
                // Right
                quads.Add(GenerateQuad(offset + new Vector3(demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), true));
            }
            if (meshFaces == MeshFaces.Inside || meshFaces == MeshFaces.Doublesided)
            {
                // Ground
                quads.Add(GenerateQuad(offset + new Vector3(0, -demiScale.y, 0), new Vector3(scale.x, 0, scale.z), true));
                // Ceiling
                quads.Add(GenerateQuad(offset + new Vector3(0, demiScale.y, 0), new Vector3(scale.x, 0, scale.z), false));

                // Forward
                quads.Add(GenerateQuad(offset + new Vector3(0, 0, -demiScale.z), new Vector3(scale.x, scale.y, 0), false));
                // Backward
                quads.Add(GenerateQuad(offset + new Vector3(0, 0, demiScale.z), new Vector3(scale.x, scale.y, 0), true));

                // Left
                quads.Add(GenerateQuad(offset + new Vector3(-demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), true));
                // Right
                quads.Add(GenerateQuad(offset + new Vector3(demiScale.x, 0, 0), new Vector3(0, scale.y, scale.z), false));
            }

            return quads;
        }

        public static Mesh MoveMesh(Mesh mesh, Vector3 offset)
        {
            List<Vector3> vertices = new List<Vector3>();
            mesh.GetVertices(vertices);
            for (int v = 0; v < vertices.Count; v++)
                vertices[v] = (offset + vertices[v]);
            mesh.SetVertices(vertices);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh RotateMesh(Mesh mesh, Quaternion rotation)
        {
            List<Vector3> vertices = new List<Vector3>();
            mesh.GetVertices(vertices);
            for(int v = 0; v < vertices.Count; v++)
                vertices[v] = rotation * vertices[v];
            mesh.SetVertices(vertices);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh GenerateQuad(Vector3 offset, Vector3 scale, bool counterClockwise)
        {
            scale /= 2;

            // Create mesh
            Mesh quad = new Mesh();

            // - - - Vertices
            // Initialization
            Vector3[] vertices = new Vector3[4];
            // Vertices
            if(scale.x == 0)
            {
                vertices[0] = offset + new Vector3(0, -scale.y, -scale.z);
                vertices[1] = offset + new Vector3(0, -scale.y, scale.z);
                vertices[2] = offset + new Vector3(0, scale.y, -scale.z);
                vertices[3] = offset + new Vector3(0, scale.y, scale.z);
            }
            else if(scale.y == 0)
            {
                vertices[0] = offset + new Vector3(-scale.x, 0, -scale.z);
                vertices[1] = offset + new Vector3(scale.x, 0, -scale.z);
                vertices[2] = offset + new Vector3(-scale.x, 0, scale.z);
                vertices[3] = offset + new Vector3(scale.x, 0, scale.z);
            }
            else if(scale.z == 0)
            {
                vertices[0] = offset + new Vector3(-scale.x, -scale.y, 0);
                vertices[1] = offset + new Vector3(scale.x, -scale.y, 0);
                vertices[2] = offset + new Vector3(-scale.x, scale.y, 0);
                vertices[3] = offset + new Vector3(scale.x, scale.y, 0);
            }
            // Assignation
            quad.vertices = vertices;

            // - - - Triangles
            // Initialization
            int[] tri = new int[6];
            if (counterClockwise)
            {
                // Triangle 1
                tri[0] = 0;
                tri[1] = 2;
                tri[2] = 1;
                // Triangle 2
                tri[3] = 2;
                tri[4] = 3;
                tri[5] = 1;
            }
            else
            {
                // Triangle 1
                tri[0] = 0;
                tri[1] = 1;
                tri[2] = 2;
                // Triangle 2
                tri[3] = 2;
                tri[4] = 1;
                tri[5] = 3;
            }
            // Assignation
            quad.triangles = tri;

            // - - - Normals
            quad.RecalculateNormals();

            // - - - UVs
            // Initialization
            Vector2[] uv = new Vector2[4];
            // UVs
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);
            // Assignation
            quad.uv = uv;

            // Return mesh
            return quad;
        }

        public static Mesh GenerateTrapeze(Vector3 offset, Vector3 scale, float trunc, bool counterClockwise)
        {
            scale /= 2;

            // Create mesh
            Mesh quad = new Mesh();

            // - - - Vertices
            // Initialization
            Vector3[] vertices = new Vector3[4];
            // Vertices
            if (scale.x == 0)
            {
                vertices[0] = offset + new Vector3(0, -scale.y + trunc, -scale.z);
                vertices[1] = offset + new Vector3(0, -scale.y, scale.z);
                vertices[2] = offset + new Vector3(0, scale.y - trunc, -scale.z);
                vertices[3] = offset + new Vector3(0, scale.y, scale.z);
            }
            else if (scale.y == 0)
            {
                vertices[0] = offset + new Vector3(-scale.x + trunc, 0, -scale.z);
                vertices[1] = offset + new Vector3(scale.x - trunc, 0, -scale.z);
                vertices[2] = offset + new Vector3(-scale.x, 0, scale.z);
                vertices[3] = offset + new Vector3(scale.x, 0, scale.z);
            }
            else if (scale.z == 0)
            {
                vertices[0] = offset + new Vector3(-scale.x, -scale.y + trunc, 0);
                vertices[1] = offset + new Vector3(scale.x, -scale.y, 0);
                vertices[2] = offset + new Vector3(-scale.x, scale.y - trunc, 0);
                vertices[3] = offset + new Vector3(scale.x, scale.y, 0);
            }
            // Assignation
            quad.vertices = vertices;

            // - - - Triangles
            // Initialization
            int[] tri = new int[6];
            if (counterClockwise)
            {
                // Triangle 1
                tri[0] = 0;
                tri[1] = 2;
                tri[2] = 1;
                // Triangle 2
                tri[3] = 2;
                tri[4] = 3;
                tri[5] = 1;
            }
            else
            {
                // Triangle 1
                tri[0] = 0;
                tri[1] = 1;
                tri[2] = 2;
                // Triangle 2
                tri[3] = 2;
                tri[4] = 1;
                tri[5] = 3;
            }
            // Assignation
            quad.triangles = tri;

            // - - - Normals
            quad.RecalculateNormals();

            // - - - UVs
            // Initialization
            Vector2[] uv = new Vector2[4];
            // UVs
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);
            // Assignation
            quad.uv = uv;

            // Return mesh
            return quad;
        }



    }
}

