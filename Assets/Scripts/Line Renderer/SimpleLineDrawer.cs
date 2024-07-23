using UnityEngine;

public class SimpleLineDrawer : LineDrawer
{
    public Color color;

    public override void InitializeLine()
    {
        base.InitializeLine();
        drawLine.startColor = color;
        drawLine.endColor = color;
    }
}
