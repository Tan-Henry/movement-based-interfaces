using UnityEngine;
using UnityEngine.VFX;

public class VFXLineDrawer : LineDrawer
{
    private VisualEffect visualEffect;
    public VisualEffectAsset visualEffectAsset;

    protected override void InitializeLine()
    {
        base.InitializeLine();
        visualEffect = newLine.AddComponent<VisualEffect>();
        visualEffect.visualEffectAsset = visualEffectAsset;
    }

    protected override void OnLineComplete()
    {
        Mesh mesh = new Mesh { name = "Line" };
        drawLine.BakeMesh(mesh);
        visualEffect.SetMesh("LineMesh", mesh);
    }
}