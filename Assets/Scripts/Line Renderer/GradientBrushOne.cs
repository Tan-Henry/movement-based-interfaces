
using System.Collections.Generic;
using UnityEngine;

public class GradientBrushOne : MonoBehaviour
{
    private List<Vector3> linePoints;
    private List<float> lineDistances;
    private List<Color> lineColors; // Track colors at each point

    private GameObject newLine;
    private LineRenderer drawLine;
    private Vector3 lastPoint;
    private float lastTime;
    public float minLineWidth;
    public float maxLineWidth;
    public float maxSpeed;
    public float minSpeed; // Minimum speed control for gradient transition
    public Gradient gradient; // Gradient field
    private float lastSpeed;
    private int positionCount;
    private float totalLengthOld;
    private float totalUsageTime; // Track total brush usage time
    public float gradientSpeed = 1.0f; // Speed of the gradient transition

    private void Start()
    {
        linePoints = new List<Vector3>();
        lineDistances = new List<float>();
        lineColors = new List<Color>(); // Initialize color list
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

            AddPoint(currentPoint, width, (currentTime - lastTime) * gradientSpeed);

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
            lineColors.Clear();
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
        // Apply the gradient when the line is complete
        ApplyGradient();
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

    private void AddPoint(Vector3 position, float width, float deltaTime)
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
        totalUsageTime += deltaTime;
        Color color = gradient.Evaluate(totalUsageTime / maxSpeed);
        lineColors.Add(color);

        ApplyDynamicGradient();
    }

    private void ApplyGradient()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[Mathf.Min(lineColors.Count, 8)];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[Mathf.Min(lineColors.Count, 8)];

        for (int i = 0; i < colorKeys.Length; i++)
        {
            float t = (float)i / (colorKeys.Length - 1);
            int colorIndex = Mathf.RoundToInt(t * (lineColors.Count - 1));
            colorKeys[i] = new GradientColorKey(lineColors[colorIndex], t);
            alphaKeys[i] = new GradientAlphaKey(lineColors[colorIndex].a, t);
        }

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(colorKeys, alphaKeys);

        drawLine.colorGradient = newGradient;
    }

    private void ApplyDynamicGradient()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[Mathf.Min(lineColors.Count, 8)];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[Mathf.Min(lineColors.Count, 8)];

        for (int i = 0; i < colorKeys.Length; i++)
        {
            float t = (float)i / (colorKeys.Length - 1);
            int colorIndex = Mathf.RoundToInt(t * (lineColors.Count - 1));
            colorKeys[i] = new GradientColorKey(lineColors[colorIndex], t);
            alphaKeys[i] = new GradientAlphaKey(lineColors[colorIndex].a, t);
        }

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(colorKeys, alphaKeys);

        drawLine.colorGradient = newGradient;
    }
}