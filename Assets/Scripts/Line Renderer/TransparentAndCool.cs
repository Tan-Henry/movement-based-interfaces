using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransparentAndCool : MonoBehaviour
{
    private List<Vector3> linePoints;
    private List<float> pointTimestamps;
    private GameObject newLine;
    private LineRenderer drawLine;
    private Vector3 lastPoint;
    private float lastTime;
    public float minLineWidth;
    public float maxLineWidth;
    public float maxSpeed = 0f; // Constant max speed of 0
    public float minSpeed = 0f; // Constant min speed of 0
    public Gradient gradient; // Gradient field
    private float lastSpeed;
    private int positionCount;
    private float totalLengthOld;
    public float pointLifetime = 2.0f; // Lifetime for each point in seconds
    public float deathSpeed = 1.0f; // Speed at which points die
    public float blendingThreshold = 0.1f; // Blending threshold to avoid overblending

    private void Start()
    {
        linePoints = new List<Vector3>();
        pointTimestamps = new List<float>();
        totalLengthOld = 0;
        StartCoroutine(RemoveExpiredPointsCoroutine());
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
            pointTimestamps.Clear();
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
        drawLine.startColor = gradient.colorKeys[0].color;
        drawLine.endColor = gradient.colorKeys[gradient.colorKeys.Length - 1].color;
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

    private void AddPoint(Vector3 position, float width)
    {
        positionCount++;
        linePoints.Add(position);
        pointTimestamps.Add(Time.time);

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
        for (int i = 1; i < linePoints.Count; i++)
        {
            totalDistance += Vector3.Distance(linePoints[i - 1], linePoints[i]);
        }

        for (int i = 0; i < 8; i++)
        {
            float t = Mathf.Clamp01((float)i / 7 * totalDistance / Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(lastSpeed / maxSpeed)));
            t = Mathf.Clamp(t, blendingThreshold, 1 - blendingThreshold); // Apply blending threshold

            colorKeys[i] = new GradientColorKey(gradient.Evaluate(t), t);
            alphaKeys[i] = new GradientAlphaKey(gradient.Evaluate(t).a, t);
        }

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(colorKeys, alphaKeys);

        drawLine.colorGradient = newGradient;
    }

    private IEnumerator RemoveExpiredPointsCoroutine()
    {
        while (true)
        {
            RemoveExpiredPoints();
            yield return new WaitForSeconds(deathSpeed);
        }
    }

    private void RemoveExpiredPoints()
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
            StartCoroutine(ReduceLineLength(expiredCount));
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
            GameObject.Destroy(newLine);
        }
    }
}
