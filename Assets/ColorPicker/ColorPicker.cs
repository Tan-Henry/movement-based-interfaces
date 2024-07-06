using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public GameObject largerCube;
    public GameObject innerCubeRGB;
    public GameObject innerCubeHue;
    public GameObject innerCubeSaturation;
    public GameObject innerCubeValue;
    public float edgeThickness = 0.02f;
    public bool useHSV = false; // Toggle to switch between RGB and HSV

    [Range(0, 360)]
    public int hue = 0;
    [Range(0, 100)]
    public int saturation = 0;
    [Range(0, 100)]
    public int value = 0;
    [Range(0, 255)]
    public int red = 0;
    [Range(0, 255)]
    public int green = 0;
    [Range(0, 255)]
    public int blue = 0;

    private Vector3 largerCubeSize;
    private Vector3 innerCubeSize;

    public Color SelectedColor { get; private set; }

    private bool lastUseHSV;

    private void Start()
    {
        InitializeCubes();
    }

    private void InitializeCubes()
    {
        // Set the size of the larger and inner cubes
        largerCubeSize = largerCube.GetComponent<Renderer>().bounds.size;
        innerCubeSize = innerCubeRGB.GetComponent<Renderer>().bounds.size;

        // Create colored edges for the larger cube
        CreateColoredEdges();

        // Initialize the position of the inner cubes
        UpdateSelectedColor();

        // Store the initial state of useHSV
        lastUseHSV = useHSV;

        // Add labels to HSV cubes
        AddLabelsToCube(innerCubeHue, "H");
        AddLabelsToCube(innerCubeSaturation, "S");
        AddLabelsToCube(innerCubeValue, "V");

        // Initialize inner cubes position based on inspector values
        UpdateInnerCubePositions();
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
        Color startColor = useHSV ? CalculateColorHSV(start + (largerCubeSize * 0.5f)) : CalculateColorRGB(start + (largerCubeSize * 0.5f));
        Color endColor = useHSV ? CalculateColorHSV(end + (largerCubeSize * 0.5f)) : CalculateColorRGB(end + (largerCubeSize * 0.5f));

        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Sprites/Default"));
    }

    private Color CalculateColorRGB(Vector3 position)
    {
        return new Color(position.x / largerCubeSize.x, position.y / largerCubeSize.y, position.z / largerCubeSize.z);
    }

    private Color CalculateColorHSV(Vector3 position)
    {
        float h = position.x / largerCubeSize.x;
        float s = position.y / largerCubeSize.y;
        float v = position.z / largerCubeSize.z;
        return Color.HSVToRGB(h, s, v);
    }

    private void Update()
    {
        // Check if the useHSV toggle has changed
        if (lastUseHSV != useHSV)
        {
            // Update edges to reflect the new color space
            CreateColoredEdges();
            lastUseHSV = useHSV;

            // Update visibility of inner cubes
            UpdateCubeVisibility();
        }

        // Update the position of the inner cubes based on user input
        HandleInput();

        // Ensure the inner cubes stay within the bounds of the larger cube
        ConstrainInnerCubes();

        // Update the selected color based on the inner cubes' positions
        UpdateSelectedColor();

        // Update inner cube positions based on inspector values
        UpdateInnerCubePositions();
    }

    private void HandleInput()
    {
        float moveSpeed = 0.1f;
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Depth"));

        if (useHSV)
        {
            innerCubeHue.transform.localPosition += new Vector3(movement.x, 0, 0) * moveSpeed;
            innerCubeSaturation.transform.localPosition += new Vector3(0, movement.y, 0) * moveSpeed;
            innerCubeValue.transform.localPosition += new Vector3(0, 0, movement.z) * moveSpeed;
        }
        else
        {
            innerCubeRGB.transform.localPosition += movement * moveSpeed;
        }
    }

    private void ConstrainInnerCubes()
    {
        if (useHSV)
        {
            ConstrainInnerCube(innerCubeHue, true, false, false);
            ConstrainInnerCube(innerCubeSaturation, false, true, false);
            ConstrainInnerCube(innerCubeValue, false, false, true);
        }
        else
        {
            ConstrainInnerCube(innerCubeRGB, true, true, true);
        }
    }

    private void ConstrainInnerCube(GameObject innerCube, bool constrainX, bool constrainY, bool constrainZ)
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

    private void UpdateSelectedColor()
    {
        if (useHSV)
        {
            Vector3 huePos = innerCubeHue.transform.localPosition + (largerCubeSize / 2);
            Vector3 saturationPos = innerCubeSaturation.transform.localPosition + (largerCubeSize / 2);
            Vector3 valuePos = innerCubeValue.transform.localPosition + (largerCubeSize / 2);

            hue = Mathf.RoundToInt((huePos.x / largerCubeSize.x) * 360);
            saturation = Mathf.RoundToInt((saturationPos.y / largerCubeSize.y) * 100);
            value = Mathf.RoundToInt((valuePos.z / largerCubeSize.z) * 100);

            SelectedColor = Color.HSVToRGB(hue / 360f, saturation / 100f, value / 100f);
        }
        else
        {
            Vector3 localPos = innerCubeRGB.transform.localPosition + (largerCubeSize / 2);
            red = Mathf.RoundToInt((localPos.x / largerCubeSize.x) * 255);
            green = Mathf.RoundToInt((localPos.y / largerCubeSize.y) * 255);
            blue = Mathf.RoundToInt((localPos.z / largerCubeSize.z) * 255);

            SelectedColor = new Color(red / 255f, green / 255f, blue / 255f);
        }

        if (useHSV)
        {
            innerCubeHue.GetComponent<Renderer>().material.color = SelectedColor;
            innerCubeSaturation.GetComponent<Renderer>().material.color = SelectedColor;
            innerCubeValue.GetComponent<Renderer>().material.color = SelectedColor;
        }
        else
        {
            innerCubeRGB.GetComponent<Renderer>().material.color = SelectedColor;
        }
    }

    private void UpdateCubeVisibility()
    {
        innerCubeRGB.SetActive(!useHSV);
        innerCubeHue.SetActive(useHSV);
        innerCubeSaturation.SetActive(useHSV);
        innerCubeValue.SetActive(useHSV);
    }

    private void AddLabelsToCube(GameObject cube, string label)
    {
        // Add TextMesh components to each face of the cube
        AddTextMeshToFace(cube, label, new Vector3(0, 0, 0.51f), new Vector3(0, 0, 0));    // Front
        AddTextMeshToFace(cube, label, new Vector3(0, 0, -0.51f), new Vector3(0, 180, 0)); // Back
        AddTextMeshToFace(cube, label, new Vector3(0.51f, 0, 0), new Vector3(0, 90, 0));   // Right
        AddTextMeshToFace(cube, label, new Vector3(-0.51f, 0, 0), new Vector3(0, -90, 0)); // Left
    }

    private void AddTextMeshToFace(GameObject cube, string label, Vector3 localPosition, Vector3 localEulerAngles)
    {
        GameObject textObject = new GameObject("Label");
        textObject.transform.SetParent(cube.transform, false);
        textObject.transform.localPosition = localPosition;
        textObject.transform.localEulerAngles = localEulerAngles;

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = label;
        textMesh.fontSize = 50;
        textMesh.characterSize = 0.1f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.black; // Adjust color as needed

        // Adjust the size and alignment
        textMesh.characterSize = 0.2f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
    }

    private void UpdateInnerCubePositions()
    {
        if (useHSV)
        {
            innerCubeHue.transform.localPosition = new Vector3((hue / 360f) * largerCubeSize.x - largerCubeSize.x / 2, 0, 0);
            innerCubeSaturation.transform.localPosition = new Vector3(0, (saturation / 100f) * largerCubeSize.y - largerCubeSize.y / 2, 0);
            innerCubeValue.transform.localPosition = new Vector3(0, 0, (value / 100f) * largerCubeSize.z - largerCubeSize.z / 2);
        }
        else
        {
            innerCubeRGB.transform.localPosition = new Vector3((red / 255f) * largerCubeSize.x - largerCubeSize.x / 2,
                                                               (green / 255f) * largerCubeSize.y - largerCubeSize.y / 2,
                                                               (blue / 255f) * largerCubeSize.z - largerCubeSize.z / 2);
        }
    }
}
