using System.Collections.Generic;
using UnityEngine;

public class ColorChangingBrush : MonoBehaviour
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

    public Color colorStart = Color.red;
    public Color colorEnd = Color.green;
    public float colorChangeDuration = 1.0f;

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
        positionCount++;

        drawLine.positionCount = positionCount;
        drawLine.SetPosition(positionCount - 1, position);

        var curve = drawLine.widthCurve;

        if (positionCount == 1)
        {
            curve.MoveKey(0, new Keyframe(0f, width));
        }
        else
        {
            var positions = new Vector3[positionCount];
            drawLine.GetPositions(positions);

            var totalLengthNew = 0f;
            for (var i = 1; i < positionCount; i++)
            {
                totalLengthNew += Vector3.Distance(positions[i - 1], positions[i]);
            }

            var factor = totalLengthOld / totalLengthNew;
            totalLengthOld = totalLengthNew;

            var keys = curve.keys;
            for (var i = 1; i < keys.Length; i++)
            {
                var key = keys[i];
                key.time *= factor;
                curve.MoveKey(i, key);
            }

            curve.AddKey(1f, width);
        }

        drawLine.widthCurve = curve;

        // Apply the color dynamically
        ApplyColor();
    }

    private void ApplyColor()
    {
        float lerp = Mathf.PingPong(Time.time, colorChangeDuration) / colorChangeDuration;
        Color currentColor = Color.Lerp(colorStart, colorEnd, lerp);
        drawLine.startColor = currentColor;
        drawLine.endColor = currentColor;
    }
}
