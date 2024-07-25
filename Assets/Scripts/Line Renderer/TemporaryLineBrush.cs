using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryLineBrush : LineDrawer
{
    public float pointLifetime = 2.0f; // Lifetime for each point in seconds
    public float checkInterval = 0.1f; // Interval to check for expired points
    private float lineCompleteTime;
    public Gradient gradient;

    protected override void Update()
    {
        if (inputManager.RightHandIsEffecting)
        {
            if (!isDrawing)
            {
                InitializeLine();
                isDrawing = true;
            }
            linePoints.Add(inputManager.RightHandPosition);
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());
        }
        else if (isDrawing)
        {
            OnLineComplete();
            isDrawing = false;
        }
        
        if (inputManager.LeftHandIsEffecting)
        {
            if (!isDrawing)
            {
                InitializeLine();
                isDrawing = true;
            }
            linePoints.Add(inputManager.LeftHandPosition);
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());
        }
        else if (isDrawing)
        {
            OnLineComplete();
            isDrawing = false;
        }
    }

    public override void InitializeLine()
    {
        newLine = new GameObject("LineSegment");
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.positionCount = 0;
        drawLine.useWorldSpace = true;
        drawLine.colorGradient = gradient;
        drawLine.startWidth = lineWidth;
        drawLine.endWidth = lineWidth;
    }

    protected override void OnLineComplete()
    {
        lineCompleteTime = Time.time; // Record the time when drawing is complete
        StartCoroutine(RemoveExpiredPointsCoroutine());
    }

    private IEnumerator RemoveExpiredPointsCoroutine()
    {
        while (linePoints.Count > 0)
        {
            RemoveExpiredPoints();
            yield return new WaitForSeconds(checkInterval);
        }

        Destroy(newLine);
    }

    private void RemoveExpiredPoints()
    {
        float currentTime = Time.time;

        // Calculate the elapsed time since the line was completed
        float elapsedTimeSinceCompletion = currentTime - lineCompleteTime;

        // Remove points that have exceeded their lifetime since the line was completed
        while (linePoints.Count > 0 && elapsedTimeSinceCompletion > pointLifetime)
        {
            linePoints.RemoveAt(0);
            elapsedTimeSinceCompletion -= checkInterval;
        }

        // Update the LineRenderer with the remaining points
        drawLine.positionCount = linePoints.Count;
        drawLine.SetPositions(linePoints.ToArray());
    }
}
