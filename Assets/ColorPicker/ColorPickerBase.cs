using UnityEngine;

public abstract class ColorPickerBase : MonoBehaviour
{
    public GameObject largerCube;
    public float edgeThickness = 0.02f;

    protected Vector3 largerCubeSize;
    protected Vector3 innerCubeSize;
    protected bool lastUseHSV;
    protected GameObject[] innerCubes;

    private Color selectedColor;
    public Color SelectedColor
    {
        get { return selectedColor; }
        protected set { selectedColor = value; }
    }

    protected virtual void Start()
    {
        InitializeCubes();
    }

    protected void InitializeCubes()
    {
        largerCubeSize = largerCube.GetComponent<Renderer>().bounds.size;
        CreateColoredEdges();
        UpdateSelectedColor();
        lastUseHSV = GetUseHSV();
        UpdateCubeVisibility();
        UpdateInnerCubePositions();
    }

    protected void CreateColoredEdges()
    {
        foreach (Transform child in largerCube.transform)
        {
            if (child.name == "Edge")
            {
                Destroy(child.gameObject);
            }
        }

        Vector3 halfSize = largerCubeSize * 0.27f;

        // Bottom edges
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, halfSize.z));
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, halfSize.z), new Vector3(halfSize.x, -halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, halfSize.z), new Vector3(halfSize.x, -halfSize.y, -halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));

        // Top edges
        CreateEdge(new Vector3(-halfSize.x, halfSize.y, -halfSize.z), new Vector3(-halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(-halfSize.x, halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, -halfSize.z));
        CreateEdge(new Vector3(halfSize.x, halfSize.y, -halfSize.z), new Vector3(-halfSize.x, halfSize.y, -halfSize.z));

        // Vertical edges
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, halfSize.y, -halfSize.z));
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, halfSize.z), new Vector3(-halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, -halfSize.z), new Vector3(halfSize.x, halfSize.y, -halfSize.z));
    }

    protected void CreateEdge(Vector3 start, Vector3 end)
    {
        GameObject edge = new GameObject("Edge");
        edge.transform.SetParent(largerCube.transform, false);
        LineRenderer lr = edge.AddComponent<LineRenderer>();
        lr.startWidth = edgeThickness;
        lr.endWidth = edgeThickness;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Color startColor = CalculateColor(start + (largerCubeSize * 0.5f));
        Color endColor = CalculateColor(end + (largerCubeSize * 0.5f));

        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Sprites/Default"));
    }

    protected abstract Color CalculateColor(Vector3 position);
    protected abstract bool GetUseHSV();
    protected abstract void UpdateInnerCubePositions();
    protected abstract void UpdateSelectedColor();
    protected abstract void UpdateCubeVisibility();

    protected virtual void Update()
    {
        if (lastUseHSV != GetUseHSV())
        {
            CreateColoredEdges();
            lastUseHSV = GetUseHSV();
            UpdateCubeVisibility();
        }

        HandleInput();
        ConstrainInnerCubes();
        UpdateSelectedColor();
        UpdateInnerCubePositions();
    }

    protected void HandleInput()
    {
        float moveSpeed = 0.1f;
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Depth"));

        foreach (GameObject cube in innerCubes)
        {
            cube.transform.localPosition += movement * moveSpeed;
        }
    }

    protected void ConstrainInnerCubes()
    {
        foreach (GameObject cube in innerCubes)
        {
            ConstrainInnerCube(cube, true, true, true);
        }
    }

    protected void ConstrainInnerCube(GameObject innerCube, bool constrainX, bool constrainY, bool constrainZ)
    {
        Vector3 pos = innerCube.transform.localPosition;
        if (constrainX)
            pos.x = Mathf.Clamp(pos.x, -largerCubeSize.x / 2 + innerCubeSize.x / 2, largerCubeSize.x / 2 - innerCubeSize.x / 2);
        if (constrainY)
            pos.y = Mathf.Clamp(pos.y, -largerCubeSize.y / 2 + innerCubeSize.y / 2, largerCubeSize.y / 2 - innerCubeSize.y / 2);
        if (constrainZ)
            pos.z = Mathf.Clamp(pos.z, -largerCubeSize.z / 2 + innerCubeSize.z / 2, largerCubeSize.z / 2 - innerCubeSize.z / 2);
        innerCube.transform.localPosition = pos;
    }
}
