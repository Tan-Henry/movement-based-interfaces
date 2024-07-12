public static class GridMetrics
{
    public static float ChunkSize = 15.5f;
    public const int NumThreads = 8; // Number of threads for ComputeShader dispatch
    
    public const int PointsPerChunk = 32; // Number of points per chunk along one axis

    //public const int ThreadGroups = PointsPerChunk / NumThreads;
    public static int ThreadGroups() {
        return PointsPerChunk / NumThreads;
    }
}