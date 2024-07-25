using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryLineBrush : LineDrawer
{
    public float pointLifetime = 2.0f; // Lifetime for each point in seconds
    public float checkInterval = 0.1f; // Interval to check for expired points
    public Gradient gradientRight;
    public Gradient gradientLeft;

    private bool isRightHandDrawing;
    private bool isLeftHandDrawing;

    private float rightHandLineCompleteTime;
    private float leftHandLineCompleteTime;

    private List<Vector3> rightHandLinePoints;
    private List<Vector3> leftHandLinePoints;

    private LineRenderer rightHandDrawLine;
    private LineRenderer leftHandDrawLine;

    private GameObject rightHandLine;
    private GameObject leftHandLine;

    protected override void Start()
    {
        base.Start();
        rightHandLinePoints = new List<Vector3>();
        leftHandLinePoints = new List<Vector3>();
    }

    protected override void Update()
    {
        HandleRightHandDrawing();
        HandleLeftHandDrawing();
    }

    private void HandleRightHandDrawing()
    {
        if (inputManager.RightHandIsEffecting)
        {
            if (!isRightHandDrawing)
            {
                InitializeRightHandLine();
                isRightHandDrawing = true;
            }
            rightHandLinePoints.Add(inputManager.RightHandPosition);
            rightHandDrawLine.positionCount = rightHandLinePoints.Count;
            rightHandDrawLine.SetPositions(rightHandLinePoints.ToArray());
        }
        else if (isRightHandDrawing)
        {
            OnRightHandLineComplete();
            isRightHandDrawing = false;
        }
    }

    private void HandleLeftHandDrawing()
    {
        if (inputManager.LeftHandIsEffecting)
        {
            if (!isLeftHandDrawing)
            {
                InitializeLeftHandLine();
                isLeftHandDrawing = true;
            }
            leftHandLinePoints.Add(inputManager.LeftHandPosition);
            leftHandDrawLine.positionCount = leftHandLinePoints.Count;
            leftHandDrawLine.SetPositions(leftHandLinePoints.ToArray());
        }
        else if (isLeftHandDrawing)
        {
            OnLeftHandLineComplete();
            isLeftHandDrawing = false;
        }
    }

    private void InitializeRightHandLine()
    {
        rightHandLine = new GameObject("RightHandLineSegment");
        rightHandDrawLine = rightHandLine.AddComponent<LineRenderer>();
        rightHandDrawLine.material = new Material(Shader.Find("Sprites/Default"));
        rightHandDrawLine.positionCount = 0;
        rightHandDrawLine.useWorldSpace = true;
        rightHandDrawLine.colorGradient = gradientRight;
        rightHandDrawLine.startWidth = lineWidth;
        rightHandDrawLine.endWidth = lineWidth;
    }

    private void InitializeLeftHandLine()
    {
        leftHandLine = new GameObject("LeftHandLineSegment");
        leftHandDrawLine = leftHandLine.AddComponent<LineRenderer>();
        leftHandDrawLine.material = new Material(Shader.Find("Sprites/Default"));
        leftHandDrawLine.positionCount = 0;
        leftHandDrawLine.useWorldSpace = true;
        leftHandDrawLine.colorGradient = gradientLeft;
        leftHandDrawLine.startWidth = lineWidth;
        leftHandDrawLine.endWidth = lineWidth;
    }

    private void OnRightHandLineComplete()
    {
        rightHandLineCompleteTime = Time.time; // Record the time when drawing is complete
        StartCoroutine(RemoveExpiredPointsCoroutine(rightHandLinePoints, rightHandDrawLine, rightHandLine, rightHandLineCompleteTime));
    }

    private void OnLeftHandLineComplete()
    {
        leftHandLineCompleteTime = Time.time; // Record the time when drawing is complete
        StartCoroutine(RemoveExpiredPointsCoroutine(leftHandLinePoints, leftHandDrawLine, leftHandLine, leftHandLineCompleteTime));
    }

    private IEnumerator RemoveExpiredPointsCoroutine(List<Vector3> linePoints, LineRenderer drawLine, GameObject lineObject, float lineCompleteTime)
    {
        while (linePoints.Count > 0)
        {
            RemoveExpiredPoints(linePoints, drawLine, lineCompleteTime);
            yield return new WaitForSeconds(checkInterval);
        }

        Destroy(lineObject);
    }

    private void RemoveExpiredPoints(List<Vector3> linePoints, LineRenderer drawLine, float lineCompleteTime)
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
