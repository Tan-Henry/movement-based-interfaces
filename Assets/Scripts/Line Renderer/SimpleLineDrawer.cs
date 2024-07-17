using UnityEngine;

public class SimpleLineDrawer : LineDrawer
{
    public Color color;
    private Material defaultMaterial;

    protected override void Start()
    {
        base.Start();
        defaultMaterial = new Material(Shader.Find("Sprites/Default"));
    }

    public override void InitializeLine()
    {
        base.InitializeLine();
        LineRenderer lineRenderer = GetDrawLine();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.material = defaultMaterial; // Always use the default material
    }
}
