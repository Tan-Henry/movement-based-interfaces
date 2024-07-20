using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolBrush : MonoBehaviour
{
    private List<CoolBrushSegment> lineSegments;
    [SerializeField] private float minLineWidth;
    [SerializeField] private float maxLineWidth;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed; // Minimum speed control for gradient transition
    [SerializeField] private Gradient gradient; // Gradient field
    [SerializeField] private float pointLifetime = 2.0f; // Lifetime for each point in seconds
    [SerializeField] private bool enablePointLifetime = true; // Toggle for point lifetime

    private Vector3 lastPoint;
    private float lastTime;
    private float lastSpeed;

    private void Start()
    {
        lineSegments = new List<CoolBrushSegment>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPoint = GetMousePosition();
            lastTime = Time.time;
            CreateNewLineSegment();
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = GetMousePosition();
            float currentTime = Time.time;
            float speed = CalculateSpeed(lastPoint, currentPoint, lastTime, currentTime);

            float t = Mathf.Clamp01(speed / maxSpeed);
            float width = Mathf.Lerp(minLineWidth, maxLineWidth, t);

            AddPointToCurrentLineSegment(currentPoint, width);

            lastPoint = currentPoint;
            lastTime = currentTime;
            lastSpeed = speed;
        }

        // Check for points that have exceeded their lifetime and remove them progressively
        if (enablePointLifetime)
        {
            foreach (var segment in lineSegments)
            {
                segment.RemoveExpiredPoints();
            }
        }
    }

    private void CreateNewLineSegment()
    {
        var newLineSegment = new CoolBrushSegment(this, minLineWidth, maxLineWidth, maxSpeed, minSpeed, gradient, pointLifetime,  enablePointLifetime);
        lineSegments.Add(newLineSegment);
    }

    private void AddPointToCurrentLineSegment(Vector3 point, float width)
    {
        if (lineSegments.Count > 0)
        {
            lineSegments[lineSegments.Count - 1].AddPoint(point, width);
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
}

public class CoolBrushSegment
{
    private List<Vector3> linePoints;
    private List<float> pointTimestamps;
    private GameObject lineObject;
    private LineRenderer drawLine;
    private float minLineWidth;
    private float maxLineWidth;
    private float maxSpeed;
    private float minSpeed;
    private Gradient gradient;
    private float pointLifetime;
    private float deathSpeed;
    private int positionCount;
    private bool enablePointLifetime;

    private MonoBehaviour monoBehaviour;

    public CoolBrushSegment(MonoBehaviour monoBehaviour, float minLineWidth, float maxLineWidth, float maxSpeed, float minSpeed, Gradient gradient, float pointLifetime,  bool enablePointLifetime)
    {
        this.monoBehaviour = monoBehaviour;
        this.minLineWidth = minLineWidth;
        this.maxLineWidth = maxLineWidth;
        this.maxSpeed = maxSpeed;
        this.minSpeed = minSpeed;
        this.gradient = gradient;
        this.pointLifetime = pointLifetime;
        this.deathSpeed = deathSpeed;
        this.enablePointLifetime = enablePointLifetime;

        linePoints = new List<Vector3>();
        pointTimestamps = new List<float>();

        lineObject = new GameObject("CoolBrushSegment");
        drawLine = lineObject.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.positionCount = 0;
        drawLine.useWorldSpace = true;
        drawLine.startColor = new Color(1, 1, 1, 0.5f); // Adjust transparency
        drawLine.endColor = new Color(1, 1, 1, 0.5f);   // Adjust transparency

        if (enablePointLifetime)
        {
            monoBehaviour.StartCoroutine(RemoveExpiredPointsCoroutine());
        }
    }

    public void AddPoint(Vector3 position, float width)
    {
        positionCount++;
        linePoints.Add(position);
        pointTimestamps.Add(Time.time);

        drawLine.positionCount = positionCount;
        drawLine.SetPosition(positionCount - 1, position);

        var curve = new AnimationCurve();
        float totalLengthNew = 0f;
        for (var i = 1; i < positionCount; i++)
        {
            totalLengthNew += Vector3.Distance(linePoints[i - 1], linePoints[i]);
        }
        var factor = 1f;
        if (totalLengthNew > 0)
        {
            factor = 1f / totalLengthNew;
        }

        for (var i = 0; i < positionCount; i++)
        {
            curve.AddKey(i * factor, Mathf.Lerp(minLineWidth, maxLineWidth, i * factor));
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
        for (int i = 1; i < linePoints.Count; i++)
        {
            totalDistance += Vector3.Distance(linePoints[i - 1], linePoints[i]);
        }

        if (totalDistance > 0)
        {
            for (int i = 0; i < 8; i++)
            {
                float t = Mathf.Clamp01((float)i / 7);
                float distanceFactor = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(totalDistance / maxSpeed));
                if (distanceFactor > 0)
                {
                    t *= totalDistance / distanceFactor;
                }
                else
                {
                    t = 0; // Ensure t is valid
                }

                colorKeys[i] = new GradientColorKey(gradient.Evaluate(t), t);
                alphaKeys[i] = new GradientAlphaKey(gradient.Evaluate(t).a, t);
            }

            Gradient newGradient = new Gradient();
            newGradient.SetKeys(colorKeys, alphaKeys);

            drawLine.colorGradient = newGradient;
        }
    }

    public void RemoveExpiredPoints()
    {
        float currentTime = Time.time;
        int expiredCount = 0;

        // Count how many points have expired
        for (int i = 0; i < pointTimestamps.Count; i++)
        {
            if (currentTime - pointTimestamps[i] > pointLifetime)
            {
                expiredCount++;
            }
            else
            {
                break;
            }
        }

        // Remove expired points from the start
        if (expiredCount > 0)
        {
            monoBehaviour.StartCoroutine(ReduceLineLength(expiredCount));
        }
    }

    private IEnumerator RemoveExpiredPointsCoroutine()
    {
        while (true)
        {
            RemoveExpiredPoints();
            yield return new WaitForSeconds(deathSpeed);
        }
    }

    private IEnumerator ReduceLineLength(int expiredCount)
    {
        for (int i = 0; i < expiredCount; i++)
        {
            if (linePoints.Count > 0)
            {
                linePoints.RemoveAt(0);
                pointTimestamps.RemoveAt(0);
                positionCount--;

                drawLine.positionCount = positionCount;
                for (int j = 0; j < positionCount; j++)
                {
                    drawLine.SetPosition(j, linePoints[j]);
                }

                // Apply the gradient dynamically
                ApplyGradient();

                yield return new WaitForSeconds(deathSpeed / expiredCount);
            }
        }

        // Destroy the line segment if all points are gone
        if (positionCount == 0)
        {
            GameObject.Destroy(lineObject);
        }
    }
}
