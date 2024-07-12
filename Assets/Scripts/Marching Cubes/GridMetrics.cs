public static class GridMetrics
{
    public static float ChunkSize = 15.5f;
    public const int NumThreads = 8; // Number of threads for ComputeShader dispatch
    public const int Scale = 32;

    public const int PointsPerChunkTerra = 32; // Number of points per chunk along one axis
    //public const int PointsPerChunk = 32; // Number of points per chunk along one axis
    public static int PointsPerChunk(int lod) {
        return LODs[lod];
    }
    
    //public const int ThreadGroups = PointsPerChunk / NumThreads;
    public static int ThreadGroups(int lod) {
        return LODs[lod] / NumThreads;
    }
    
    public static int[] LODs = {
        8,
        16,
        24,
        32,
        40
    };
    
}