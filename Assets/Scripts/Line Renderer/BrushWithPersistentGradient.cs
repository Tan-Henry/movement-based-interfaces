using System.Collections.Generic;
using UnityEngine;

public class BrushWithReversingGradient : MonoBehaviour
{
    private List<Vector3> linePoints;
    private List<float> lineDistances;

    private GameObject newLine;
    private LineRenderer drawLine;
    private Vector3 lastPoint;
    private float lastTime;
    public float minLineWidth;
    public float maxLineWidth;
    public float maxSpeed;
    public Gradient gradient; // Gradient field
    public float gradientSpeedMin; // Minimum speed control for gradient transition
    public float gradientSpeedMax; // Maximum speed control for gradient transition
    private float lastSpeed;
    private int positionCount;
    private float totalLengthOld;
    private float gradientPosition; // Position in the gradient
    private bool gradientDirectionForward = true; // Direction of the gradient

    private void Start()
    {
        linePoints = new List<Vector3>();
        lineDistances = new List<float>();
        totalLengthOld = 0;
        gradientPosition = 0; // Start at the beginning of the gradient
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
            lineDistances.Clear();
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

    private void OnLineComplete()
    {
        // Store the gradient position for the next line
        float lineLength = 0;
        foreach (float distance in lineDistances)
        {
            lineLength += distance;
        }

        float gradientSpeed = gradientDirectionForward ? gradientSpeedMax : -gradientSpeedMax;
        gradientPosition += lineLength / gradientSpeed;
        
        if (gradientPosition >= 1.0f)
        {
            gradientPosition = 1.0f;
            gradientDirectionForward = false;
        }
        else if (gradientPosition <= 0.0f)
        {
            gradientPosition = 0.0f;
            gradientDirectionForward = true;
        }
    }

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
        linePoints.Add(position);

        float distance = positionCount > 1 ? Vector3.Distance(linePoints[positionCount - 2], position) : 0;
        lineDistances.Add(distance);

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

        // Apply the gradient dynamically
        ApplyGradient();
    }

    private void ApplyGradient()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[8];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[8];

        float totalDistance = 0;
        foreach (float distance in lineDistances)
        {
            totalDistance += distance;
        }

        for (int i = 0; i < 8; i++)
        {
            float t = gradientDirectionForward
                ? (gradientPosition + (float)i / 7 * totalDistance / gradientSpeedMax) % 1.0f
                : (gradientPosition - (float)i / 7 * totalDistance / gradientSpeedMax) % 1.0f;

            // Ensure t stays within [0, 1] range
            if (t < 0) t += 1.0f;
            if (t > 1) t -= 1.0f;

            colorKeys[i] = new GradientColorKey(gradient.Evaluate(t), t);
            alphaKeys[i] = new GradientAlphaKey(gradient.Evaluate(t).a, t);
        }

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(colorKeys, alphaKeys);

        drawLine.colorGradient = newGradient;
    }
}
