using UnityEngine;

public class SimpleLineDrawer : LineDrawer
{
    public Color color;

    public override void InitializeLine()
    {
        base.InitializeLine();
        LineRenderer lineRenderer = GetDrawLine();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
