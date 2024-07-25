using System.Collections.Generic;
using UnityEngine;

namespace Marching_Cubes
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject chunkPrefab;
        public int worldSizeX;
        public int worldSizeZ;
        public int worldSizeY;
        //public int brushType;
        
        public Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
    
        void Start()
        {
            GenerateWorld();
        }
        
        void GenerateWorld()
        {
            for (int x = 0; x < worldSizeX; x++)
            {
                for (int y = 0; y < worldSizeY; y++)  // Include y-axis iteration
                {
                    for (int z = 0; z < worldSizeZ; z++)
                    {
                        Vector3 chunkPosition = new Vector3(x * GridMetrics.ChunkSize, y * GridMetrics.ChunkSize, z * GridMetrics.ChunkSize);
                        GameObject newChunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
                        Chunk chunkScript = newChunk.GetComponent<Chunk>();
                        chunkScript.Initialize(chunkPosition);
                        chunks.Add(new Vector3Int(x, y, z), chunkScript);
                    }
                }
            }
        }
        
        public Chunk GetChunk(Vector3Int chunkPosition)
        {
            return chunks[chunkPosition];
        }
        
        public void hideChunks(Vector3 cameraPosition)
        {
            foreach (KeyValuePair<Vector3Int, Chunk> chunk in chunks)
            {
                Vector3 chunkPosition = chunk.Value.transform.position;
                float distance = Vector3.Distance(cameraPosition, chunkPosition);
                if (distance > 400)
                {
                    chunk.Value.gameObject.SetActive(false);
                }
                else
                {
                    chunk.Value.gameObject.SetActive(true);
                }
            }
        }
        
        public void TerraformAtPoint(Vector3 hitPoint, float brushSize, bool add, int brushType) {
            // Calculate the affected chunks
            Vector3 minCorner = hitPoint - Vector3.one * brushSize;
            Vector3 maxCorner = hitPoint + Vector3.one * brushSize;

            // Calculate min and max chunk coordinates for each axis
            Vector3Int minChunk = Vector3Int.FloorToInt(minCorner / GridMetrics.PointsPerChunk);
            Vector3Int maxChunk = Vector3Int.FloorToInt(maxCorner / GridMetrics.PointsPerChunk);

            for (int x = minChunk.x; x <= maxChunk.x; x++) {
                for (int y = minChunk.y; y <= maxChunk.y; y++) {  // Include y-axis iteration
                    for (int z = minChunk.z; z <= maxChunk.z; z++) {
                        Vector3Int chunkPos = new Vector3Int(x, y, z);

                        if (chunks.TryGetValue(chunkPos, out Chunk chunk)) {
                            chunk.EditWeights(hitPoint, brushSize, add, brushType);
                        }
                    }
                }
            }
        }
    }
}
