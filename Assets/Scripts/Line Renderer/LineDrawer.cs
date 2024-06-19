using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    public List<Vector3> linePoints;
    public float timerDelay;
    protected GameObject newLine;
    protected LineRenderer drawLine;
    protected float timer; // Changed from private to protected
    public float lineWidth;

    protected virtual void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeLine();
        }
        if (Input.GetMouseButton(0))
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Vector3 mousePosition = GetMousePosition();
                linePoints.Add(mousePosition);
                drawLine.positionCount = linePoints.Count;
                drawLine.SetPositions(linePoints.ToArray());
                timer = timerDelay;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnLineComplete();
            linePoints.Clear();
        }
    }

    public virtual void InitializeLine()
    {
        newLine = new GameObject();
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.startWidth = lineWidth;
        drawLine.endWidth = lineWidth;
        drawLine.startColor = Color.clear;
        drawLine.endColor = Color.clear;
    }

    public void ClearLine()
    {
        linePoints.Clear();
        if (drawLine != null)
        {
            drawLine.positionCount = 0;
        }
    }

    public void AddPoint(Vector3 point)
    {
        linePoints.Add(point);
        drawLine.positionCount = linePoints.Count;
        drawLine.SetPositions(linePoints.ToArray());
    }

    protected virtual void OnLineComplete() { }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 3;
    }

    public LineRenderer GetDrawLine()
    {
        return drawLine;
    }

    public GameObject GetNewLine()
    {
        return newLine;
    }
}
