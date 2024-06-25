using System.Collections.Generic;
using UnityEngine;

public class DynamicLineDrawing : MonoBehaviour
{
    private List<Vector3> linePoints;

    private GameObject newLine;
    private LineRenderer drawLine;
    private Vector3 lastPoint;
    private float lastTime;
    public float minLineWidth;
    public float maxLineWidth;
    public float maxSpeed;
    private float lastSpeed;
    private int positionCount;
    private float totalLengthOld;

    private void Start()
    {
        linePoints = new List<Vector3>();
        totalLengthOld = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeLine();
            lastPoint = GetMousePosition();
            lastTime = Time.time;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = GetMousePosition();
            float currentTime = Time.time;
            float speed = CalculateSpeed(lastPoint, currentPoint, lastTime, currentTime);

            float t = Mathf.Clamp01(speed / maxSpeed);
            float width = Mathf.Lerp(minLineWidth, maxLineWidth, t);
            AddPoint(currentPoint, width);

            lastPoint = currentPoint;
            lastTime = currentTime;
            lastSpeed = speed;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnLineComplete();
            Mesh mesh = new Mesh { name = "Line" };
            drawLine.BakeMesh(mesh);
            linePoints.Clear();
        }
    }

    private void InitializeLine()
    {
        positionCount = 0;
        newLine = new GameObject("LineSegment");
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.positionCount = 0;
        drawLine.startColor = Color.blue;
        drawLine.endColor = Color.blue;
        drawLine.useWorldSpace = true;
    }

    private void OnLineComplete() { }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 3;
    }

    private float CalculateSpeed(Vector3 startPoint, Vector3 endPoint, float startTime, float endTime)
    {
        float distance = Vector3.Distance(startPoint, endPoint);
        float time = Mathf.Max(endTime - startTime, 0.0001f); // Ensure time is not zero to prevent division by zero
        return distance / time;
    }

    private void AddPoint(Vector3 position, float width)
    {
        // increase the position count by one
        positionCount++;

        // set the count back to the line
        drawLine.positionCount = positionCount;

        // add our new point
        drawLine.SetPosition(positionCount - 1, position);

        // now get the current width curve
        var curve = drawLine.widthCurve;

        // Is this the beginning of the line?
        if (positionCount == 1)
        {
            // First point => simply set the first keyframe 
            curve.MoveKey(0, new Keyframe(0f, width));
        }
        else
        {
            // otherwise get all positions
            var positions = new Vector3[positionCount];
            drawLine.GetPositions(positions);

            // sum up the distances between positions to obtain the length of the line
            var totalLengthNew = 0f;
            for (var i = 1; i < positionCount; i++)
            {
                totalLengthNew += Vector3.Distance(positions[i - 1], positions[i]);
            }

            // calculate the time factor we have to apply to all already existing keyframes
            var factor = totalLengthOld / totalLengthNew;

            // then store for the next added point
            totalLengthOld = totalLengthNew;

            // now move all existing keys which are currently based on the totalLengthOld to according positions based on the totalLengthNew
            // we can skip the first one as it will stay at 0 always
            var keys = curve.keys;
            for (var i = 1; i < keys.Length; i++)
            {
                var key = keys[i];
                Debug.Log("Debug i-1: " + keys[i-1].time);
                Debug.Log("Debug i: " + key.time);
                key.time *= factor;

                curve.MoveKey(i, key);
            }

            // add the new last keyframe
            curve.AddKey(1f, width);
        }

        // finally write the curve back to the line
        drawLine.widthCurve = curve;
    }
}
