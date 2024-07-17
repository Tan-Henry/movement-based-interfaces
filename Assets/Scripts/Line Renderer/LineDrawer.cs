using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;
    
    protected GameObject newLine;
    protected LineRenderer drawLine;
    private bool _initialized;
    private bool _drawing;
    public float lineWidth;
    [SerializeField] protected BaseInputManager inputManager;

    protected virtual void Start()
    {
        linePoints = new List<Vector3>();
    }

    protected virtual void Update()
    {
        if (inputManager.RightHandIsDrawing2D && !_initialized)
        {
            InitializeLine();
        }
        if (inputManager.RightHandIsDrawing2D)
        {
            linePoints.Add(inputManager.RightHandPosition);
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());
            _drawing = true;
        }

        if (!inputManager.RightHandIsDrawing2D && _drawing)
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
        _initialized = true;
    }


    protected virtual void OnLineComplete()
    {
        UndoRedoScript undoRedoScript = GetComponent<UndoRedoScript>();
        undoRedoScript.AddLastLineGameObject(newLine);
        _initialized = false;
        _drawing = false;
    }
}