using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    public LineDrawer lineDrawer;
    private List<Vector3> recordedPoints;
    private List<float> recordedTimes;
    private List<int> strokeIndices;
    private List<LineRenderer> lineRenderers; // List to store LineRenderers
    private float recordingStartTime;
    private float recordingEndTime;
    private bool isRecording = false;
    private bool isPlaying = false;
    private float playbackStartTime;

    void Update()
    {
        HandleInput();
        if (isRecording)
        {
            RecordBrushStrokes();
        }
        else if (isPlaying)
        {
            PlayRecordedStrokes();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StopRecordingAndStartPlayback();
        }
    }

    private void StartRecording()
    {
        isRecording = true;
        isPlaying = false;
        recordingStartTime = Time.time;
        recordedPoints = new List<Vector3>();
        recordedTimes = new List<float>();
        strokeIndices = new List<int>();
        lineRenderers = new List<LineRenderer>();
        lineDrawer.ClearLine();
    }

    private void RecordBrushStrokes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            strokeIndices.Add(recordedPoints.Count); // Add the start index of the new stroke
            lineRenderers.Add(lineDrawer.GetDrawLine()); // Add the newly created LineRenderer to the list
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = lineDrawer.GetMousePosition();
            recordedPoints.Add(mousePosition);
            recordedTimes.Add(Time.time - recordingStartTime);
        }
    }

    private void StopRecordingAndStartPlayback()
    {
        isRecording = false;
        isPlaying = true;
        recordingEndTime = Time.time;
        playbackStartTime = Time.time;
        ClearAllLines(); // Clear existing lines before playback
        InitializePlaybackLines();
    }

    private void PlayRecordedStrokes()
    {
        float elapsedTime = (Time.time - playbackStartTime);

        if (elapsedTime >= (recordingEndTime - recordingStartTime))
        {
            // Playback is complete, reset to start over
            ClearAllLines();
            playbackStartTime = Time.time;
            InitializePlaybackLines();
            return; // Exit the function to prevent further execution
        }

        int currentStrokeIndex = 0;

        for (int i = 0; i < recordedPoints.Count; i++)
        {
            if (currentStrokeIndex < strokeIndices.Count && i == strokeIndices[currentStrokeIndex])
            {
                if (i != 0)
                {
                    lineDrawer.InitializeLine();
                    lineRenderers.Add(lineDrawer.GetDrawLine());
                }
                currentStrokeIndex++;
            }

            if (recordedTimes[i] <= elapsedTime)
            {
                if (lineRenderers.Count > 0 && currentStrokeIndex - 1 < lineRenderers.Count)
                {
                    lineRenderers[currentStrokeIndex - 1].positionCount = i + 1 - strokeIndices[currentStrokeIndex - 1];
                    lineRenderers[currentStrokeIndex - 1].SetPosition(i - strokeIndices[currentStrokeIndex - 1], recordedPoints[i]);

                    // Apply dynamic width adjustment if using DynamicBrushDrawer
                    if (lineDrawer is DynamicBrushDrawer dynamicBrushDrawer)
                    {
                        AnimationCurve widthCurve = new AnimationCurve();
                        float midPoint = (i + 1 - strokeIndices[currentStrokeIndex - 1]) / 2.0f;
                        for (int j = strokeIndices[currentStrokeIndex - 1]; j <= i; j++)
                        {
                            float t = Mathf.Abs(j - midPoint) / midPoint; // Normalized value [0, 1]
                            float width = Mathf.Lerp(dynamicBrushDrawer.maxLineWidth, dynamicBrushDrawer.initialLineWidth, t);
                            widthCurve.AddKey((float)(j - strokeIndices[currentStrokeIndex - 1]) / (i + 1 - strokeIndices[currentStrokeIndex - 1]), width);
                        }
                        lineRenderers[currentStrokeIndex - 1].widthCurve = widthCurve;
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    private void ClearAllLines()
    {
        foreach (LineRenderer line in lineRenderers)
        {
            if (line != null)
            {
                Destroy(line.gameObject);
            }
        }
        lineRenderers.Clear();
        lineDrawer.ClearLine();
    }

    private void InitializePlaybackLines()
    {
        foreach (int index in strokeIndices)
        {
            lineDrawer.InitializeLine();
            lineRenderers.Add(lineDrawer.GetDrawLine());
        }
    }
}
