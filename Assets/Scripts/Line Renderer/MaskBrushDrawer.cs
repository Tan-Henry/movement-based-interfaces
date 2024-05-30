using UnityEngine;

public class MaskBrushDrawer : LineDrawer
{
    public Material material;
    protected override void InitializeLine()
    {
        base.InitializeLine();
        drawLine.textureMode = LineTextureMode.Tile;
        drawLine.material = material;
    }
    
}