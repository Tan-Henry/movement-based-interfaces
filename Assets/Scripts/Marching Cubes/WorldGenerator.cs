using System.Collections.Generic;
using UnityEngine;

namespace Marching_Cubes
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject chunkPrefab;
        public int worldSizeX;
        public int worldSizeZ;
        
        public Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
    
        void Start()
        {
            GenerateWorld();
        }
        
        void GenerateWorld()
        {
            for (int x = 0; x < worldSizeX; x++)
            {
                for (int z = 0; z < worldSizeZ; z++)
                {
                    Vector3 chunkPosition = new Vector3(x * GridMetrics.ChunkSize, 0f, z * GridMetrics.ChunkSize);
                    GameObject newChunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
                    Chunk chunkScript = newChunk.GetComponent<Chunk>();
                    chunkScript.Initialize(chunkPosition);
                    chunks.Add(new Vector3Int(x, 0, z), chunkScript);
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
    }
}
