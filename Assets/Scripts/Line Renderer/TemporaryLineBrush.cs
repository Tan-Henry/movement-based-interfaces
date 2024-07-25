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

    private List<Vector3> rightHandLinePoints;
    private List<Vector3> leftHandLinePoints;

    private List<float> rightHandPointTimes;
    private List<float> leftHandPointTimes;

    private LineRenderer rightHandDrawLine;
    private LineRenderer leftHandDrawLine;

    private GameObject rightHandLine;
    private GameObject leftHandLine;

    protected override void Start()
    {
        base.Start();
        rightHandLinePoints = new List<Vector3>();
        leftHandLinePoints = new List<Vector3>();
        rightHandPointTimes = new List<float>();
        leftHandPointTimes = new List<float>();
        //bStartCoroutine(RemoveExpiredPointsCoroutine());
    }

    protected override void Update()
    {
        // doesn't work deactivated
       // HandleRightHandDrawing();
       // HandleLeftHandDrawing();
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
            rightHandPointTimes.Add(Time.time);
            rightHandDrawLine.positionCount = rightHandLinePoints.Count;
            rightHandDrawLine.SetPositions(rightHandLinePoints.ToArray());
        }
        else if (isRightHandDrawing)
        {
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
            leftHandPointTimes.Add(Time.time);
            leftHandDrawLine.positionCount = leftHandLinePoints.Count;
            leftHandDrawLine.SetPositions(leftHandLinePoints.ToArray());
        }
        else if (isLeftHandDrawing)
        {
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

    private IEnumerator RemoveExpiredPointsCoroutine()
    {
        while (true)
        {
            RemoveExpiredPoints(rightHandLinePoints, rightHandPointTimes, rightHandDrawLine, rightHandLine);
            RemoveExpiredPoints(leftHandLinePoints, leftHandPointTimes, leftHandDrawLine, leftHandLine);
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void RemoveExpiredPoints(List<Vector3> linePoints, List<float> pointTimes, LineRenderer drawLine, GameObject lineObject)
    {
        float currentTime = Time.time;

        // Remove points that have exceeded their lifetime
        while (pointTimes.Count > 0 && currentTime - pointTimes[0] > pointLifetime)
        {
            linePoints.RemoveAt(0);
            pointTimes.RemoveAt(0);
        }

        // Update the LineRenderer with the remaining points
        drawLine.positionCount = linePoints.Count;
        drawLine.SetPositions(linePoints.ToArray());

        // Destroy the line segment if all points are gone
        if (linePoints.Count == 0 && lineObject != null)
        {
            Destroy(lineObject);
        }
    }
}
