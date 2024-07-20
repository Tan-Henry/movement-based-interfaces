using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputManager : MonoBehaviour
{
    // Input-Events and Values

    //right-hand 
    public abstract Vector3 RightHandPosition { get; protected set; }
    protected abstract void UpdateRightHand();
    
    //left-hand
    public abstract Vector3 LeftHandPosition { get; protected set; }
    protected abstract void UpdateLeftHand();

    //draw-action 2D right hand
    public abstract bool RightHandIsDrawing2D { get; protected set; }

    //draw-action 3D right hand
    public abstract bool RightHandIsDrawing3D { get; protected set; }

    //erase-action 2D right hand
    public abstract bool RightHandIsErasing2D { get; protected set; }

    //erase-action 3D right hand
    public abstract bool RightHandIsErasing3D { get; protected set; }

    // draw-action head
    public abstract Vector3 HeadDrawPosition { get; protected set; }
    protected abstract void UpdateHeadDrawing();

    //ChangeEffect
    public abstract event Action ChangeEffect;
    protected abstract void OnChangeEffect();

    //SwitchMode
    public abstract event Action SwitchMode;
    protected abstract void OnSwitchMode();

    //MainMenu
    public abstract event Action MainMenu;
    protected abstract void OnMainMenu();

    //TurnOnColorPicker
    public abstract event Action TurnOnColorPicker;
    protected abstract void OnTurnOnColorPicker();

    //TurnOffColorPicker
    public abstract event Action TurnOffColorPicker;
    protected abstract void OnTurnOffColorPicker();

    //Undo
    public abstract event Action Undo;
    public abstract void OnUndo();

    //Redo
    public abstract event Action Redo;
    public abstract void OnRedo();

    //ToggleBrushEraser
    public abstract void OnToggleBrushEraser();
    
    //Reset
    public abstract event Action ResetMenu;
    protected abstract void OnResetMenu();


    // App-State

    // App-Mode
    public abstract EMode CurrentMode { get; set; }

    // Brush-Category

    public abstract EBrushCategory CurrentBrushCategory { get; set; }
    public abstract event Action<EBrushCategory> OnBrushCategoryChanged;

    // 2D Brushes
    public abstract EBrushType2D Current2DBrushType { get; set; }
    public abstract List<ELineBrushes2D> Available2DLineBrushes { get; set; }
    public abstract ELineBrushes2D Current2DLineBrush { get; set; }
    public abstract List<EDynamicBrushes2D> Available2DDynamicBrushes { get; set; }
    public abstract EDynamicBrushes2D Current2DDynamicBrush { get; set; }

    // 3D Brushes
    public abstract EBrushType3D Current3DBrushType { get; set; }
    public abstract List<ELineBrushes3D> Available3DLineBrushes { get; set; }
    public abstract ELineBrushes3D Current3DLineBrush { get; set; }
    public abstract List<ETexturedBrushes3D> Available3DTexturedBrushes { get; set; }
    public abstract ETexturedBrushes3D Current3DTexturedBrush { get; set; }
    public abstract List<EStructuralBrushes3D> Available3DStructuralBrushes { get; set; }
    public abstract EStructuralBrushes3D Current3DStructuralBrush { get; set; }

    // Brush Settings
    public abstract BrushSettings2D Current2DBrushSettings { get; set; }
    public abstract BrushSettings3D Current3DBrushSettings { get; set; }

    // Effects
    public abstract List<EEffects> AvailableEffects { get; set; }
    public abstract EEffects CurrentEffect { get; set; }

    // Other Functions
    public abstract bool IsDrawingState { get; set; }

    protected virtual void Update()
    {
        UpdateRightHand();
        UpdateLeftHand();
        UpdateHeadDrawing();
    }
}

public enum EMode
{
    MAIN_MENU,
    TUTORIAL,
    CREATE,
    PRESENT
}

public enum EBrushCategory
{
    NONE,
    BRUSH_2D,
    BRUSH_3D
}

public enum EBrushType2D
{
    NONE,
    LINE,
    DYNAMIC
}

public enum ELineBrushes2D
{
    NONE,
    STANDARD,
    MASK
}

public enum EDynamicBrushes2D
{
    NONE,
    SIZE,
    GRADIENT,
    COLOR
}

public enum EBrushType3D
{
    NONE,
    LINE,
    TEXTURED,
    STRUCTURAL
}

public enum ELineBrushes3D
{
    NONE,
    SHARP,
    SMOOTH,
    PSEUDO_SPACED
}

public enum ETexturedBrushes3D
{
    NONE,
    SIMPLE_NOISE,
    COLUMN_NOISE,
    NOISE_FIELD
}

public enum EStructuralBrushes3D
{
    NONE,
    SLICES,
    SPIKES
}

public class BrushSettings2D
{
    public float brushSize;
    public float opacity;
}

public class BrushSettings3D 
{
    public float brushSize { get; set; }
}

public enum EEffects
{
    NONE,
    BUBBLES,
    COOL,
    HEATMAP,
}

public static class Limits
{
    public const int MAX_BRUSH_SIZE = 5;
    public const int MIN_BRUSH_SIZE = 1;
    public const float MAX_OPACITY = 1.0f;
    public const float MIN_OPACITY = 0.1f;
}