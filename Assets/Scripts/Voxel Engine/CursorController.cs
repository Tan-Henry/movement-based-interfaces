using System;
using System.Numerics;
using Marching_Cubes;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CursorController : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    public WorldGenerator worldGenerator;

    private int finalBrushType;
    private bool isPainting;
    private Vector3 handPos;
    private float brushSize;

    void setBrushSettings()
    {
        handPos = inputManager.RightHandPosition;
        transform.position = handPos;

        brushSize = inputManager.Current3DBrushSettings.brushSize;
        // Additional Settings can be added here
        EBrushType3D brushType = inputManager.Current3DBrushType;
        switch (brushType)
        {
            case EBrushType3D.NONE:
                break;
            case EBrushType3D.LINE:
                var lineBrush = inputManager.Current3DLineBrush;
                switch (lineBrush)
                {
                    case ELineBrushes3D.NONE:
                        break;
                    case ELineBrushes3D.SHARP:
                        finalBrushType = 0;
                        break;
                    case ELineBrushes3D.SMOOTH:
                        finalBrushType = 1;
                        break;
                    case ELineBrushes3D.PSEUDO_SPACED:
                        finalBrushType = 2;
                        break;
                }

                break;
            case EBrushType3D.TEXTURED:
                var texturedBrush = inputManager.Current3DTexturedBrush;
                switch (texturedBrush)
                {
                    case ETexturedBrushes3D.NONE:
                        break;
                    case ETexturedBrushes3D.SIMPLE_NOISE:
                        finalBrushType = 3;
                        break;
                    case ETexturedBrushes3D.COLUMN_NOISE:
                        finalBrushType = 4;
                        break;
                    case ETexturedBrushes3D.NOISE_FIELD:
                        finalBrushType = 5;
                        break;
                }

                break;
            case EBrushType3D.STRUCTURAL:
                var structuralBrush = inputManager.Current3DStructuralBrush;
                switch (structuralBrush)
                {
                    case EStructuralBrushes3D.NONE:
                        break;
                    case EStructuralBrushes3D.SLICES:
                        finalBrushType = 6;
                        break;
                    case EStructuralBrushes3D.SPIKES:
                        finalBrushType = 7;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Update()
    {
        if (inputManager.RightHandIsDrawing3D)
        {
            setBrushSettings();
            isPainting = true;
            ChangeVoxelAt(handPos, brushSize, isPainting);
        }
        else if (inputManager.RightHandIsErasing3D)
        {
            setBrushSettings();
            isPainting = false;
            ChangeVoxelAt(handPos, brushSize, isPainting);
        }
    }

    private void ChangeVoxelAt(Vector3 worldPosition, float brushSize, bool isPainting)
    {
        worldGenerator.TerraformAtPoint(worldPosition, brushSize, isPainting, finalBrushType);
    }
}