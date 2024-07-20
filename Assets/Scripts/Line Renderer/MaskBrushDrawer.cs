using UnityEngine;

public class MaskBrushDrawer : LineDrawer
{
    public Material material;

    public override void InitializeLine()
    {
        base.InitializeLine();
        LineRenderer lineRenderer = GetDrawLine();
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material = material;
    }
}
