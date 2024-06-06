using UnityEngine;
using UnityEngine.VFX;

public class VFXLineDrawer : LineDrawer
{
    private VisualEffect visualEffect;
    public VisualEffectAsset visualEffectAsset;

    public override void InitializeLine()
    {
        base.InitializeLine();
        GameObject currentLine = GetNewLine();
        visualEffect = currentLine.AddComponent<VisualEffect>();
        visualEffect.visualEffectAsset = visualEffectAsset;
    }

    protected override void OnLineComplete()
    {
        Mesh mesh = new Mesh { name = "Line" };
        GetDrawLine().BakeMesh(mesh);
        visualEffect.SetMesh("LineMesh", mesh);
    }
}
