using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marching_Cubes
{
    public class Chunk : MonoBehaviour
    {
        public NoiseGenerator NoiseGenerator;
        public ComputeShader MarchingShader;
        public MeshFilter MeshFilter;
        public MeshCollider MeshCollider;

        Mesh _mesh;
        Vector3 _chunkWorldPosition;

        struct Triangle
        {
            public Vector3 a;
            public Vector3 b;
            public Vector3 c;

            public static int SizeOf => sizeof(float) * 3 * 3;
        }

        ComputeBuffer _trianglesBuffer;
        ComputeBuffer _trianglesCountBuffer;
        ComputeBuffer _weightsBuffer;

        float[] _weights;

        /*private void Awake()
        {
            CreateBuffers();
            NoiseGenerator = GameObject.Find("NoiseGenerator").GetComponent<NoiseGenerator>();
        }

        private void OnDestroy()
        {
            ReleaseBuffers();
        }*/

        void CreateBuffers()
        {
            _trianglesBuffer = new ComputeBuffer(
                5 * (GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk),
                Triangle.SizeOf, ComputeBufferType.Append);
            _trianglesCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
            _weightsBuffer = new ComputeBuffer(
                GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk,
                sizeof(float));
        }

        void ReleaseBuffers()
        {
            _trianglesBuffer.Release();
            _trianglesCountBuffer.Release();
            _weightsBuffer.Release();
        }

        public void Initialize(Vector3 chunkWorldPosition)
        {
            Create(chunkWorldPosition);
        }

        public void Create(Vector3 chunkWorldPosition)
        {
            CreateBuffers();
            NoiseGenerator = GameObject.Find("NoiseGenerator").GetComponent<NoiseGenerator>();
            _chunkWorldPosition = chunkWorldPosition;
            _weights = NoiseGenerator.GetNoise(_chunkWorldPosition);

            _mesh = new Mesh();
            UpdateMesh();
            ReleaseBuffers();
        }

        Mesh ConstructMesh()
        {
            int kernel = MarchingShader.FindKernel("March");

            MarchingShader.SetBuffer(kernel, "_Triangles", _trianglesBuffer);
            MarchingShader.SetBuffer(kernel, "_Weights", _weightsBuffer);
            
            MarchingShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
            MarchingShader.SetFloat("_IsoLevel", .5f);

            MarchingShader.SetVector("_ChunkWorldPosition", _chunkWorldPosition);

            _weightsBuffer.SetData(_weights);
            _trianglesBuffer.SetCounterValue(0);

            MarchingShader.Dispatch(kernel, 
                GridMetrics.ThreadGroups(),
                GridMetrics.ThreadGroups(),
                GridMetrics.ThreadGroups());

            Triangle[] triangles = new Triangle[ReadTriangleCount()];
            _trianglesBuffer.GetData(triangles);

            return CreateMeshFromTriangles(triangles);
        }

        int ReadTriangleCount()
        {
            int[] triCount = { 0 };
            ComputeBuffer.CopyCount(_trianglesBuffer, _trianglesCountBuffer, 0);
            _trianglesCountBuffer.GetData(triCount);
            return triCount[0];
        }

        Mesh CreateMeshFromTriangles(Triangle[] triangles)
        {
            Vector3[] verts = new Vector3[triangles.Length * 3];
            int[] tris = new int[triangles.Length * 3];

            for (int i = 0; i < triangles.Length; i++)
            {
                int startIndex = i * 3;
                verts[startIndex] = triangles[i].a + _chunkWorldPosition;
                verts[startIndex + 1] = triangles[i].b + _chunkWorldPosition;
                verts[startIndex + 2] = triangles[i].c + _chunkWorldPosition;
                tris[startIndex] = startIndex;
                tris[startIndex + 1] = startIndex + 1;
                tris[startIndex + 2] = startIndex + 2;
            }

            _mesh.Clear();
            _mesh.vertices = verts;
            _mesh.triangles = tris;
            _mesh.RecalculateNormals();
            return _mesh;
        }

        void UpdateMesh()
        {
            try
            {
                Mesh mesh = ConstructMesh();
                MeshFilter.sharedMesh = mesh;
                MeshCollider.sharedMesh = mesh;
            }
            catch (Exception e)
            {
                Debug.Log("I know this is bad practice, but I'm just going to ignore this exception: " + e.Message);
            }
        }
        
        private int spacingInterval = 3;
        private int lineSpacing = 50;

        public void EditWeights(Vector3 hitPosition, float brushSize, bool add, int brushType) {
            CreateBuffers();
            int kernel = MarchingShader.FindKernel("UpdateWeights");

            _weightsBuffer.SetData(_weights);
            MarchingShader.SetBuffer(kernel, "_Weights", _weightsBuffer);

            MarchingShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
            MarchingShader.SetVector("_HitPosition", hitPosition);
            MarchingShader.SetVector("_ChunkWorldPosition", _chunkWorldPosition); 
            MarchingShader.SetFloat("_BrushSize", brushSize);
            MarchingShader.SetFloat("_TerraformStrength", add ? 5f : -5f);
            
            MarchingShader.SetFloat("_range", 7.0f);
            MarchingShader.SetInt("_seed", Random.Range(0, int.MaxValue));
            MarchingShader.SetInt("_spacingInterval", spacingInterval);
            MarchingShader.SetInt("_lineSpacing", lineSpacing);
            MarchingShader.SetInt("_brushType", brushType);

            MarchingShader.Dispatch(kernel,
                GridMetrics.ThreadGroups(),
                GridMetrics.ThreadGroups(),
                GridMetrics.ThreadGroups());

            _weightsBuffer.GetData(_weights);

            UpdateMesh();
            ReleaseBuffers();
        }
        
        private void OnValidate() {
            if (Application.isPlaying) {
                Create(_chunkWorldPosition);
            }
        }
    }
    
    
}
