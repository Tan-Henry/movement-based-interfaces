using UnityEngine;

public class DynamicBrushDrawer : LineDrawer
{
    public float initialLineWidth = 0.1f; // Starting width
    public float maxLineWidth = 1.0f;     // Maximum width
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
                float midPoint = linePoints.Count / 2.0f;
                for (int i = 0; i < linePoints.Count; i++)
                {
                    float t = Mathf.Abs(i - midPoint) / midPoint; // Normalized value [0, 1]
                    float width = Mathf.Lerp(maxLineWidth, initialLineWidth, t);
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
