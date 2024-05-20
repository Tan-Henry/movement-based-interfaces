using System.Collections.Generic;
using SimplexNoise;
using UnityEngine;

namespace Voxel_Engine
{
    public class Chunk : MonoBehaviour
    {
        private Voxel[,,] voxels;
        private int chunkSize = 16;

        private Color gizmoColor;

        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();
        private List<Vector2> uvs = new List<Vector2>();
        
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private MeshCollider meshCollider;

        void Start()
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshCollider = gameObject.AddComponent<MeshCollider>();

            GenerateMesh();
        }
        
        private void GenerateMesh()
        {
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();

            IterateVoxels(); // Make sure this processes all voxels

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals(); // Important for lighting

            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;

            meshRenderer.material = World.Instance.VoxelMaterial;
        }

        public void initialize(int size)
        {
            gizmoColor = Random.ColorHSV(0, 1, 1, 1, 1, 1, 0.4f, 0.4f);

            chunkSize = size;
            voxels = new Voxel[chunkSize, chunkSize, chunkSize];
            InitializeVoxels();
        }

        private void InitializeVoxels()
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        Vector3 worldPos = transform.position + new Vector3(x, y, z);
                        Voxel.VoxelType type = DetermineVoxelType(worldPos.x, worldPos.y, worldPos.z);
                        voxels[x, y, z] = new Voxel(worldPos, type, type != Voxel.VoxelType.Air);
                    }
                }
            }
        }
        
        private Voxel.VoxelType DetermineVoxelType(float x, float y, float z)
        {
            float noiseValue = Noise.CalcPixel3D((int)x, (int)y, (int)z, 0.02f);
    
            float threshold = 125f; // The threshold for determining solid/air
    
            if (noiseValue > threshold)
                return Voxel.VoxelType.Clay; // Solid voxel
            else
                return Voxel.VoxelType.Air; // Air voxel
        }

        public void IterateVoxels()
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        ProcessVoxel(x, y, z);
                    }
                }
            }
        }

        private void ProcessVoxel(int x, int y, int z)
        {
            if (voxels == null || x < 0 || x >= voxels.GetLength(0) ||
                y < 0 || y >= voxels.GetLength(1) || z < 0 || z >= voxels.GetLength(2))
            {
                return;
            }

            Voxel voxel = voxels[x, y, z];

            if (voxel.isActive)
            {
                bool[] facesVisible = new bool[6];
                facesVisible[0] = IsFaceVisible(x, y + 1, z); // Top
                facesVisible[1] = IsFaceVisible(x, y - 1, z); // Bottom
                facesVisible[2] = IsFaceVisible(x - 1, y, z); // Left
                facesVisible[3] = IsFaceVisible(x + 1, y, z); // Right
                facesVisible[4] = IsFaceVisible(x, y, z + 1); // Front
                facesVisible[5] = IsFaceVisible(x, y, z - 1); // Back

                for (int i = 0; i < facesVisible.Length; i++)
                {
                    if (facesVisible[i])
                        AddFaceData(x, y, z, i);
                }
            }
        }


        private bool IsFaceVisible(int x, int y, int z)
        {
            // Convert local chunk coordinates to global coordinates
            Vector3 globalPos = transform.position + new Vector3(x, y, z);

            // Check if the neighboring voxel is inactive or out of bounds in the current chunk
            // and also if it's inactive or out of bounds in the world (neighboring chunks)
            return IsVoxelHiddenInChunk(x, y, z) && IsVoxelHiddenInWorld(globalPos);
        }
        
        private bool IsVoxelHiddenInChunk(int x, int y, int z)
        {
            if (x < 0 || x >= chunkSize || y < 0 || y >= chunkSize || z < 0 || z >= chunkSize)
                return true; // Face is at the boundary of the chunk
            return !voxels[x, y, z].isActive;
        }
        
        private bool IsVoxelHiddenInWorld(Vector3 globalPos)
        {
            // Check if there is a chunk at the global position
            Chunk neighborChunk = World.Instance.GetChunkAt(globalPos);
            if (neighborChunk == null)
            {
                return true;
            }

            // Convert the global position to the local position within the neighboring chunk
            Vector3 localPos = neighborChunk.transform.InverseTransformPoint(globalPos);

            return !neighborChunk.IsVoxelActiveAt(localPos);
        }
        
        public bool IsVoxelActiveAt(Vector3 localPosition)
        {
            // Round the local position to get the nearest voxel index
            int x = Mathf.RoundToInt(localPosition.x);
            int y = Mathf.RoundToInt(localPosition.y);
            int z = Mathf.RoundToInt(localPosition.z);

            // Check if the indices are within the bounds of the voxel array
            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                return voxels[x, y, z].isActive;
            }

            return false;
        }

        private void AddFaceData(int x, int y, int z, int faceIndex)
        {
            // Based on faceIndex, determine vertices and triangles
            // Add vertices and triangles for the visible face

            if (faceIndex == 0) // Top Face
            {
                vertices.Add(new Vector3(x, y + 1, z));
                vertices.Add(new Vector3(x, y + 1, z + 1));
                vertices.Add(new Vector3(x + 1, y + 1, z + 1));
                vertices.Add(new Vector3(x + 1, y + 1, z));
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(0, 1));
            }

            if (faceIndex == 1) // Bottom Face
            {
                vertices.Add(new Vector3(x, y, z));
                vertices.Add(new Vector3(x + 1, y, z));
                vertices.Add(new Vector3(x + 1, y, z + 1));
                vertices.Add(new Vector3(x, y, z + 1));
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(1, 0));
            }

            if (faceIndex == 2) // Left Face
            {
                vertices.Add(new Vector3(x, y, z));
                vertices.Add(new Vector3(x, y, z + 1));
                vertices.Add(new Vector3(x, y + 1, z + 1));
                vertices.Add(new Vector3(x, y + 1, z));
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(0, 1));
            }

            if (faceIndex == 3) // Right Face
            {
                vertices.Add(new Vector3(x + 1, y, z + 1));
                vertices.Add(new Vector3(x + 1, y, z));
                vertices.Add(new Vector3(x + 1, y + 1, z));
                vertices.Add(new Vector3(x + 1, y + 1, z + 1));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(1, 0));
            }

            if (faceIndex == 4) // Front Face
            {
                vertices.Add(new Vector3(x, y, z + 1));
                vertices.Add(new Vector3(x + 1, y, z + 1));
                vertices.Add(new Vector3(x + 1, y + 1, z + 1));
                vertices.Add(new Vector3(x, y + 1, z + 1));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(1, 1));
            }

            if (faceIndex == 5) // Back Face
            {
                vertices.Add(new Vector3(x + 1, y, z));
                vertices.Add(new Vector3(x, y, z));
                vertices.Add(new Vector3(x, y + 1, z));
                vertices.Add(new Vector3(x + 1, y + 1, z));
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(0, 0));
            }

            AddTriangleIndices();
        }
        
        private void AddTriangleIndices()
        {
            int vertCount = vertices.Count;

            // First triangle
            triangles.Add(vertCount - 4);
            triangles.Add(vertCount - 3);
            triangles.Add(vertCount - 2);

            // Second triangle
            triangles.Add(vertCount - 4);
            triangles.Add(vertCount - 2);
            triangles.Add(vertCount - 1);
        }
        
        public void SetVoxelActiveAt(Vector3 position, bool isActive)
        {
            // Round the position to get the nearest voxel index
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);
            int z = Mathf.RoundToInt(position.z);

            // Check if the indices are within the bounds of the voxel array
            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                // Set the active state of the voxel at these indices
                voxels[x, y, z].isActive = isActive;
                Debug.Log("Set voxel at " + position + " to " + (isActive ? "active" : "inactive"));
            }
    
            GenerateMesh();
        }
    }
}