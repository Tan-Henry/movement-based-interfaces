using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;

    protected GameObject newLine;
    protected LineRenderer drawLine;
    private Vector3 lastPoint;
    private float lastTime;
    public float minLineWidth;
    public float maxLineWidth;
    public float maxSpeed;
    private float lastSpeed;

    protected virtual void Start()
    {
        linePoints = new List<Vector3>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse down");
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
            // if (Mathf.Abs(speed - lastSpeed) > 4)
            // {
                AddSegment(lastPoint, currentPoint, width);
                Debug.Log(width);
            // }
            

            lastPoint = currentPoint;
            lastTime = currentTime;
            lastSpeed = speed;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnLineComplete();
            linePoints.Clear();
        }
    }

    protected virtual void InitializeLine()
    {
        newLine = new GameObject("LineSegment");
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.positionCount = 0;
        drawLine.startColor = Color.white;
        drawLine.endColor = Color.white;
        drawLine.useWorldSpace = true;
    }

    protected virtual void OnLineComplete() { }

    protected Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 3;
    }

    private float CalculateSpeed(Vector3 startPoint, Vector3 endPoint, float startTime, float endTime)
    {
        float distance = Vector3.Distance(startPoint, endPoint);
        float time = endTime - startTime;
        return distance / time;
    }

    private void AddSegment(Vector3 start, Vector3 end, float width)
    {
        GameObject segment = new GameObject("LineSegment");
        LineRenderer segmentLine = segment.AddComponent<LineRenderer>();
        segmentLine.material = new Material(Shader.Find("Sprites/Default"));
        segmentLine.startWidth = 0.1f;
        segmentLine.endWidth = 0.1f;
        segmentLine.positionCount = 2;
        segmentLine.SetPosition(0, start);
        segmentLine.SetPosition(1, end);
        segmentLine.startColor = Color.black;
        segmentLine.endColor = Color.black;
        //segmentLine.useWorldSpace = true;
    }
}
