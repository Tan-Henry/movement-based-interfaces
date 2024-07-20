using UnityEngine;
using System.Collections.Generic;

public class DynamicBrushDrawer : LineDrawer
{
    public float initialLineWidth = 0.1f; // Starting width
    public float maxLineWidth = 1.0f;     // Maximum width
    public Color color = Color.white;     // Default color of the line
    private Material defaultMaterial;     // Store the default material

    private static List<LineRenderer> shaderLines = new List<LineRenderer>(); // Store lines with shaders
    private static List<LineRenderer> nonShaderLines = new List<LineRenderer>(); // Store lines without shaders

    protected override void Start()
    {
        base.Start();

        // Ensure ShaderManager.Instance is not null before trying to use it
        if (ShaderManager.Instance != null)
        {
            if (ShaderManager.Instance.defaultNonShaderLinesColor != null)
            {
                color = ShaderManager.Instance.defaultNonShaderLinesColor;
            }
        }

        // Ensure the Shader is found before creating a material
        Shader shader = Shader.Find("Sprites/Default");
        if (shader != null)
        {
            defaultMaterial = new Material(shader);
        }
        else
        {
            Debug.LogError("Shader 'Sprites/Default' not found.");
        }

        // Initialize drawLine with default values
        if (drawLine != null)
        {
            drawLine.startColor = color; // Set initial color in Start
            drawLine.endColor = color;   // Set initial color in Start
            drawLine.material = defaultMaterial; // Set initial material
        }
        else
        {
            
        }
    }

    public override void InitializeLine()
    {
        base.InitializeLine();

        if (drawLine != null)
        {
            drawLine.startColor = color;
            drawLine.endColor = color;
            drawLine.widthMultiplier = 1.0f;
            drawLine.widthCurve = new AnimationCurve(); // Reset width curve

            if (ShaderManager.Instance != null && ShaderManager.Instance.IsShaderApplied())
            {
                drawLine.material = ShaderManager.Instance.GetCurrentMaterial();
            }
            else
            {
                drawLine.material = defaultMaterial;
            }
        }
        else
        {
            Debug.LogError("drawLine is not set in the inspector.");
        }
    }

    protected override void Update()
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
                if (drawLine != null)
                {
                    drawLine.positionCount = linePoints.Count;
                    drawLine.SetPositions(linePoints.ToArray());

                    // Apply widths dynamically
                    AnimationCurve widthCurve = new AnimationCurve();
                    float midPoint = linePoints.Count / 2.0f;
                    for (int i = 0; i < linePoints.Count; i++)
                    {
                        float t = Mathf.Abs(i - midPoint) / midPoint; // Normalized value [0, 1]
                        float width = Mathf.Lerp(maxLineWidth, initialLineWidth, t);
                        widthCurve.AddKey((float)i / (linePoints.Count - 1), width);
                    }
                    drawLine.widthCurve = widthCurve;
                }
                timer = timerDelay;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnLineComplete();
            linePoints.Clear();
            if (ShaderManager.Instance != null && ShaderManager.Instance.IsShaderApplied())
            {
                shaderLines.Add(drawLine);
            }
            else
            {
                nonShaderLines.Add(drawLine);
            }
        }
    }

    public void ApplyMaterialToShaderLines(Material material)
    {
        foreach (var line in shaderLines)
        {
            line.material = material;
        }
    }

    public void SetNonShaderLinesColor(Color color)
    {
        if (ShaderManager.Instance != null)
        {
            foreach (var line in nonShaderLines)
            {
                // Only set the color if it's not already set to something other than the default
                if (line.startColor == ShaderManager.Instance.defaultNonShaderLinesColor &&
                    line.endColor == ShaderManager.Instance.defaultNonShaderLinesColor)
                {
                    line.startColor = color;
                    line.endColor = color;
                }
            }
        }
        else
        {
            Debug.LogError("ShaderManager.Instance is not available.");
        }
    }

    public void ApplyMaterial(Material material)
    {
        if (drawLine != null)
        {
            drawLine.material = material;
        }
        else
        {
            Debug.LogError("drawLine is not set in the inspector.");
        }
    }

    public void RevertMaterial()
    {
        if (drawLine != null)
        {
            drawLine.material = defaultMaterial;
        }
        else
        {
            Debug.LogError("drawLine is not set in the inspector.");
        }
    }
}
