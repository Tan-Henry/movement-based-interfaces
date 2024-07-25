using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;
    protected float timer;
    public float timerDelay;

    protected GameObject newLine;
    protected LineRenderer drawLine;
    protected bool isDrawing;
    public float lineWidth;
    private UndoRedoScript _undoRedoScript;
    [SerializeField] protected BaseInputManager inputManager;
    [SerializeField] protected Material lineMaterial;

    protected virtual void Start()
    {
        _undoRedoScript = GetComponent<UndoRedoScript>();
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    protected virtual void Update()
    {
        if (inputManager.RightHandIsDrawing2D)
        {
            if (!isDrawing)
            {
                InitializeLine();
                isDrawing = true;
            }
            linePoints.Add(inputManager.RightHandPosition);
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());
            timer = timerDelay;
        }
        else
        {
            if (isDrawing)
            {
                OnLineComplete();
                linePoints.Clear();
                isDrawing = false;
            }
        }
    }

    public virtual void InitializeLine()
    {
        newLine = new GameObject();
        newLine.tag = "Line";
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = lineMaterial;

        //Brush Settings Input Manager
        //Opacity & Color
        float opacity = inputManager.Current2DBrushSettings.opacity;
        Color color = inputManager.Current2DBrushSettings.color;
        drawLine.startColor = new Color(color.r, color.g, color.b, opacity);
        drawLine.endColor = new Color(color.r, color.g, color.b, opacity);
        drawLine.material.SetColor("Color", new Color(1f, 1f, 1f, opacity));
        //Size
        float brushSize = inputManager.Current2DBrushSettings.brushSize;
        drawLine.startWidth = brushSize;
        drawLine.endWidth = brushSize;

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

    protected virtual void OnLineComplete()
    {
        _undoRedoScript.AddLastLineGameObject(newLine);
        
        Mesh mesh = new Mesh();
        drawLine.BakeMesh(mesh);

        MeshCollider meshCollider = newLine.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
    
    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 3;
    }

    public LineRenderer GetDrawLine()
    {
        return drawLine;
    }
}