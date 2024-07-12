using System.Collections.Generic;
using UnityEngine;

public abstract class LineDrawer : MonoBehaviour
{
    protected List<Vector3> linePoints;
    protected float timer;
    public float timerDelay;

    protected GameObject newLine;
    protected LineRenderer drawLine;
    public float lineWidth;

    public BaseInputManager inputManager;
    private bool isDrawing;

    protected virtual void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    protected virtual void Update()
    {
        if (inputManager.RightHandIsDrawing)
        {
            if (!isDrawing)
            {
                isDrawing = true;
                InitializeLine();
            }
            
            linePoints.Add(inputManager.RightHandDrawPosition);
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
        
        /*if (Input.GetMouseButtonDown(0))
        {
            InitializeLine();
        }*/
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

    protected virtual void OnLineComplete() { }
}