using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;

    protected GameObject newLine;
    protected LineRenderer drawLine;
    public float lineWidth;

    protected virtual void Start()
    {
        linePoints = new List<Vector3>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeLine();
        }
        if (Input.GetMouseButton(0))
        {
            
            linePoints.Add(GetMousePosition());
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnLineComplete();
            linePoints.Clear();
        }
    }

    protected virtual void InitializeLine()
    {
        newLine = new GameObject();
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.startWidth = lineWidth;
        drawLine.endWidth = lineWidth;
        drawLine.startColor = Color.clear;
        drawLine.endColor = Color.clear;
    }


    protected virtual void OnLineComplete()
    {
        UndoRedoScript undoRedoScript = GetComponent<UndoRedoScript>();
        undoRedoScript.AddLastLineGameObject(newLine);
    }

    protected Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 3;
    }
}