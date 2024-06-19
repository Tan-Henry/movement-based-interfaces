using UnityEngine;

public class SimpleLineDrawer : LineDrawer
{
    public Color color;

    protected override void Start()
    {
        base.Start();
    }

    public override void InitializeLine()
    {
        base.InitializeLine();
        LineRenderer lineRenderer = GetDrawLine();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // Apply the current material from the ShaderManager if shader is applied
        if (ShaderManager.Instance != null && ShaderManager.Instance.GetCurrentMaterial() != null)
        {
            lineRenderer.material = ShaderManager.Instance.GetCurrentMaterial();
        }
    }
}
