using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    public LineDrawer lineDrawer;
    private List<Vector3> recordedPoints;
    private List<float> recordedTimes;
    private float recordingStartTime;
    private float recordingEndTime;
    private bool isRecording = false;
    private bool isPlaying = false;

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
        lineDrawer.ClearLine();
    }

    private void RecordBrushStrokes()
    {
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
        lineDrawer.ClearLine();
    }

    private void PlayRecordedStrokes()
    {
        float elapsedTime = (Time.time - recordingEndTime) % (recordingEndTime - recordingStartTime);
        lineDrawer.ClearLine();
        for (int i = 0; i < recordedPoints.Count; i++)
        {
            if (recordedTimes[i] <= elapsedTime)
            {
                lineDrawer.AddPoint(recordedPoints[i]);
            }
            else
            {
                break;
            }
        }
    }
}
