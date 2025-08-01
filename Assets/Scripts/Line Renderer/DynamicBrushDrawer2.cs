using UnityEngine;

public class DynamicBrushDrawer2 : LineDrawer
{
    public float initialLineWidth = 0.1f; // Starting width
    public float maxLineWidth = 1.0f;     // Maximum width
    public float transitionLength = 10.0f; // Length over which the transition happens
    public Color color = Color.white;     // Color of the line

    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeLine()
    {
        base.InitializeLine();
        drawLine.startColor = color;
        drawLine.endColor = color;
        drawLine.widthMultiplier = 1.0f;
        drawLine.widthCurve = new AnimationCurve(); // Reset width curve

        // Apply the current material from the ShaderManager if shader is applied
        if (ShaderManager.Instance != null && ShaderManager.Instance.GetCurrentMaterial() != null)
        {
            drawLine.material = ShaderManager.Instance.GetCurrentMaterial();
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
                drawLine.positionCount = linePoints.Count;
                drawLine.SetPositions(linePoints.ToArray());

                // Apply widths dynamically
                AnimationCurve widthCurve = new AnimationCurve();
                for (int i = 0; i < linePoints.Count; i++)
                {
                    float t = Mathf.Clamp01((float)i / transitionLength); // Normalize the transition
                    float width = Mathf.Lerp(initialLineWidth, maxLineWidth, t);
                    widthCurve.AddKey((float)i / (linePoints.Count - 1), width);
                }
                drawLine.widthCurve = widthCurve;

                timer = timerDelay;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnLineComplete();
            linePoints.Clear();
        }
    }
}
