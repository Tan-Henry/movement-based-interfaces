using UnityEngine;
using UnityEngine.VFX;

public class VFXLineDrawer : LineDrawer
{
    private VisualEffect visualEffect;
    public VisualEffectAsset visualEffectAsset;
    public float vfxSpawntime = 5;
    private float elapsedTime;

    protected override void Update()
    {
        if (inputManager.RightHandIsEffecting)
        {
            if (!isDrawing)
            {
                InitializeLine();
                isDrawing = true;
            }
            linePoints.Add(inputManager.RightHandPosition);
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());
            timer = timerDelay;
        }
        else
        {
            if (isDrawing)
            {
                OnLineComplete();
                linePoints.Clear();
                isDrawing = false;
            }
        }
        if (isDrawing)
        {
            Mesh mesh = new Mesh { name = "Line" };
            drawLine.BakeMesh(mesh);
            visualEffect.SetMesh("LineMesh", mesh);
            visualEffect.Play();
        }
        if (elapsedTime <= vfxSpawntime && isDrawing)
        {
            visualEffect.Play();
            elapsedTime += Time.deltaTime;
        }
    }
    
    
    // protected override void InitializeLine()
    // {
    //     base.InitializeLine();
    //     visualEffect = newLine.AddComponent<VisualEffect>();
    //     visualEffect.visualEffectAsset = visualEffectAsset;
    // }

    protected override void OnLineComplete()
    {
        base.OnLineComplete();
        Mesh mesh = new Mesh { name = "Line" };
        drawLine.BakeMesh(mesh);
        visualEffect.SetMesh("LineMesh", mesh);
        elapsedTime = 0f;
    }
}