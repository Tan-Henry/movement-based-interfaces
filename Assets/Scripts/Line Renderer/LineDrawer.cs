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
    private Material originalMaterial;

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
        originalMaterial = drawLine.material;
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

    public GameObject GetNewLine()
    {
        return newLine;
    }

    public void ApplyMaterial(Material material)
    {
        if (drawLine != null && material != null)
        {
            drawLine.material = material;
        }
    }

    public void RevertMaterial()
    {
        if (drawLine != null && originalMaterial != null)
        {
            drawLine.material = originalMaterial;
        }
    }
}