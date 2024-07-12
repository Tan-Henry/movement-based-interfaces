using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    ComputeBuffer _weightsBuffer;
    public ComputeShader NoiseShader;

    [SerializeField] float noiseScale = 0.08f;
    [SerializeField] float amplitude = 5;
    [SerializeField] float frequency = 0.004f;
    [SerializeField] int octaves = 6;
    [SerializeField, Range(0f, 1f)] float groundPercent = 0.2f;


    /*private void Awake() {
        CreateBuffers();
    }

    private void OnDestroy() {
        ReleaseBuffers();
    }*/

    public float[] GetNoise(Vector3 chunkPosition) {
        CreateBuffers();
        float[] noiseValues =
            new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];

        NoiseShader.SetBuffer(0, "_Weights", _weightsBuffer);

        NoiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        NoiseShader.SetFloat("_NoiseScale", noiseScale);
        NoiseShader.SetFloat("_Amplitude", amplitude);
        NoiseShader.SetFloat("_Frequency", frequency);
        NoiseShader.SetInt("_Octaves", octaves);
        NoiseShader.SetFloat("_GroundPercent", groundPercent);

        NoiseShader.SetVector("_ChunkPosition", chunkPosition * 2);

        NoiseShader.Dispatch(
            0, GridMetrics.ThreadGroups(), GridMetrics.ThreadGroups(), GridMetrics.ThreadGroups()
        );

        _weightsBuffer.GetData(noiseValues);
        ReleaseBuffers();
        return noiseValues;
        
    }

    void CreateBuffers() {
        _weightsBuffer = new ComputeBuffer(
            GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float)
        );
    }

    void ReleaseBuffers() {
        _weightsBuffer.Release();
    }
}
