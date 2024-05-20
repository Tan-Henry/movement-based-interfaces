using System.Collections.Generic;
using UnityEngine;

namespace Voxel_Engine
{
    public class World : MonoBehaviour
    {
        public static World Instance { get; private set; }

        public Material VoxelMaterial;
        
        public int worldSize = 5;
        public int chunkSize = 16;

        private Dictionary<Vector3, Chunk> chunks;
        
        public int noiseSeed = 1234;
        public float noiseScale = 0.015f;
        
        void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            chunks = new Dictionary<Vector3, Chunk>();
            GenerateWorld();
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void GenerateWorld()
        {
            
            for(int x = 0; x < worldSize; x++)
            {
                for(int y = 0; y < worldSize; y++)
                {
                    for(int z = 0; z < worldSize; z++)
                    {
                        Vector3 chunkPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);
                        Chunk newChunk = new GameObject("Chunk " + x + ", " + y + ", " + z).AddComponent<Chunk>();
                        newChunk.transform.position = chunkPosition;
                        newChunk.initialize(chunkSize);
                        chunks.Add(chunkPosition, newChunk);
                    }
                }
            }
            
        }
        
        public Chunk GetChunkAt(Vector3 globalPosition)
        {
            // Calculate the chunk's starting position based on the global position
            Vector3Int chunkCoordinates = new Vector3Int(
                Mathf.FloorToInt(globalPosition.x / chunkSize) * chunkSize,
                Mathf.FloorToInt(globalPosition.y / chunkSize) * chunkSize,
                Mathf.FloorToInt(globalPosition.z / chunkSize) * chunkSize
            );

            // Retrieve and return the chunk at the calculated position
            if (chunks.TryGetValue(chunkCoordinates, out Chunk chunk))
            {
                return chunk;
            }

            return null;
        }
    }
}
