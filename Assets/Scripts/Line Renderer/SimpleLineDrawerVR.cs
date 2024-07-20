using UnityEngine;

public class SimpleLineDrawerVR : LineDrawer
{
    public Color color;

    public override void InitializeLine()
    {
        base.InitializeLine();
        drawLine.startColor = color;
        drawLine.endColor = color;
    }
    
}