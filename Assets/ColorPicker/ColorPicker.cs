using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public GameObject largerCube;
    public GameObject innerCubeRGB;
    public float edgeThickness = 0.02f;

    [SerializeField] private BaseInputManager inputManager;

    [Range(0, 255)] public int red = 0;
    [Range(0, 255)] public int green = 0;
    [Range(0, 255)] public int blue = 0;

    private Vector3 largerCubeSize;
    private Vector3 innerCubeSize;

    [SerializeField] private Color selectedColor;

    private Color SelectedColor
    {
        get => selectedColor;
        set => selectedColor = value;
    }

    private Renderer innerCubeRGBRenderer;

    private void Start()
    {
        InitializeCubes();
    }

    private void InitializeCubes()
    {
        largerCubeSize = largerCube.transform.localScale;
        innerCubeSize = innerCubeRGB.transform.localScale;

        // Create colored edges for the larger cube
        CreateColoredEdges();

        // Initialize the position of the inner cubes
        UpdateSelectedColor();

        // Initialize inner cubes position based on inspector values
        UpdateInnerCubePositions();

        innerCubeRGBRenderer = innerCubeRGB.GetComponent<Renderer>();
    }

    private void CreateColoredEdges()
    {
        // Remove existing edges
        foreach (Transform child in largerCube.transform)
        {
            if (child.name == "Edge")
            {
                Destroy(child.gameObject);
            }
        }

        Vector3 halfSize = largerCubeSize * 0.5f;

        // Bottom edges
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            new Vector3(-halfSize.x, -halfSize.y, halfSize.z));
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, halfSize.z), new Vector3(halfSize.x, -halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, halfSize.z), new Vector3(halfSize.x, -halfSize.y, -halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
            new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));

        // Top edges
        CreateEdge(new Vector3(-halfSize.x, halfSize.y, -halfSize.z), new Vector3(-halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(-halfSize.x, halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, -halfSize.z));
        CreateEdge(new Vector3(halfSize.x, halfSize.y, -halfSize.z), new Vector3(-halfSize.x, halfSize.y, -halfSize.z));

        // Vertical edges
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            new Vector3(-halfSize.x, halfSize.y, -halfSize.z));
        CreateEdge(new Vector3(-halfSize.x, -halfSize.y, halfSize.z), new Vector3(-halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
        CreateEdge(new Vector3(halfSize.x, -halfSize.y, -halfSize.z), new Vector3(halfSize.x, halfSize.y, -halfSize.z));
    }

    private void CreateEdge(Vector3 start, Vector3 end)
    {
        GameObject edge = new GameObject("Edge");
        edge.transform.SetParent(largerCube.transform, false);
        LineRenderer lr = edge.AddComponent<LineRenderer>();
        lr.startWidth = edgeThickness;
        lr.endWidth = edgeThickness;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        // Calculate color based on position and mode
        Color startColor = CalculateColorRGB(start + (largerCubeSize * 0.5f));
        Color endColor = CalculateColorRGB(end + (largerCubeSize * 0.5f));

        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Sprites/Default"));
    }

    private Color CalculateColorRGB(Vector3 position)
    {
        return new Color(position.x / largerCubeSize.x, position.y / largerCubeSize.y, position.z / largerCubeSize.z);
    }

    private void Update()
    {
        UpdateCubePosition();

        // Ensure the inner cubes stay within the bounds of the larger cube
        ConstrainInnerCubes();

        // Update the selected color based on the inner cubes' positions
        UpdateSelectedColor();
    }

    private void UpdateCubePosition()
    {
        if (inputManager.RightHandIsColorPicking)
        {
            var worldPickingPosition = inputManager.RightHandPosition;
            var localPickingPosition = largerCube.transform.InverseTransformPoint(worldPickingPosition);

            innerCubeRGB.transform.localPosition = localPickingPosition;
        }
    }

    private void ConstrainInnerCubes()
    {
        ConstrainInnerCube(innerCubeRGB, true, true, true);
    }

    private void ConstrainInnerCube(GameObject innerCube, bool constrainX, bool constrainY, bool constrainZ)
    {
        Vector3 pos = innerCube.transform.localPosition;
        if (constrainX)
            pos.x = Mathf.Clamp(pos.x, -largerCubeSize.x / 2 + innerCubeSize.x / 2,
                largerCubeSize.x / 2 - innerCubeSize.x / 2);
        if (constrainY)
            pos.y = Mathf.Clamp(pos.y, -largerCubeSize.y / 2 + innerCubeSize.y / 2,
                largerCubeSize.y / 2 - innerCubeSize.y / 2);
        if (constrainZ)
            pos.z = Mathf.Clamp(pos.z, -largerCubeSize.z / 2 + innerCubeSize.z / 2,
                largerCubeSize.z / 2 - innerCubeSize.z / 2);
        innerCube.transform.localPosition = pos;
    }

    private void UpdateSelectedColor()
    {
        Vector3 localPos = innerCubeRGB.transform.localPosition + (largerCubeSize / 2);
        red = Mathf.RoundToInt((localPos.x / largerCubeSize.x) * 255);
        green = Mathf.RoundToInt((localPos.y / largerCubeSize.y) * 255);
        blue = Mathf.RoundToInt((localPos.z / largerCubeSize.z) * 255);

        SelectedColor = new Color(red / 255f, green / 255f, blue / 255f);

        if (!innerCubeRGBRenderer) return;

        innerCubeRGBRenderer.material.color = SelectedColor;
        inputManager.Current2DBrushSettings.color = SelectedColor;
    }

    private void UpdateInnerCubePositions()
    {
        innerCubeRGB.transform.localPosition = new Vector3((red / 255f) * largerCubeSize.x - largerCubeSize.x / 2,
            (green / 255f) * largerCubeSize.y - largerCubeSize.y / 2,
            (blue / 255f) * largerCubeSize.z - largerCubeSize.z / 2);
    }
}