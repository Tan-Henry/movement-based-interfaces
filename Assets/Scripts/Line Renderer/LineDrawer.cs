using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;
    
    protected GameObject newLine;
    protected LineRenderer drawLine;
    protected bool isDrawing;
    public float lineWidth;
    private UndoRedoScript _undoRedoScript;
    [SerializeField] protected BaseInputManager inputManager;

    protected virtual void Start()
    {
        _undoRedoScript = GetComponent<UndoRedoScript>();
        linePoints = new List<Vector3>();
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

    protected virtual void InitializeLine()
    {
        newLine = new GameObject();
        newLine.tag = "Line";
        drawLine = newLine.AddComponent<LineRenderer>();
        drawLine.material = new Material(Shader.Find("Sprites/Default"));
        drawLine.startWidth = lineWidth;
        drawLine.endWidth = lineWidth;
        drawLine.startColor = Color.clear;
        drawLine.endColor = Color.clear;
    }


    protected virtual void OnLineComplete()
    {
        _undoRedoScript.AddLastLineGameObject(newLine);
        
        Mesh mesh = new Mesh();
        drawLine.BakeMesh(mesh);

        MeshCollider meshCollider = newLine.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.isTrigger = true;
    }
}