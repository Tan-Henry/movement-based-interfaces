using UnityEngine;

public class MaskBrushDrawer : LineDrawer
{
    public Material material;

    public override void InitializeLine()
    {
        base.InitializeLine();
        drawLine.textureMode = LineTextureMode.Tile;
        drawLine.material = material;
    }
}
