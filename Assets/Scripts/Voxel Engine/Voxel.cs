using UnityEngine;

namespace Voxel_Engine
{
    public struct Voxel
    {
        public Vector3 position;
        public VoxelType type;
        public bool isActive;
        public Voxel(Vector3 position, VoxelType type, bool isActive = true)
        {
            this.position = position;
            this.type = type;
            this.isActive = isActive;
        }
        
        public enum VoxelType
        {
            Clay,
            Air
        }
    }
}
