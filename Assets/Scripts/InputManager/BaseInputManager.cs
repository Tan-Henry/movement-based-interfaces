using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputManager : MonoBehaviour
{
    //draw-action right hand
    public abstract bool RightHandIsDrawing { get; protected set; }
    public abstract Vector3 RightHandDrawPosition { get; protected set; }
    protected abstract void UpdateRightHandDrawing();
    public abstract Vector3 HeadDrawPosition { get; protected set; }
    protected abstract void UpdateHeadDrawing();
    
    //changeEffect
    public abstract event Action ChangeEffect;
    protected abstract void OnChangeEffect();
    
    //switchMode
    public abstract event Action SwitchMode;
    protected abstract void OnSwitchMode();
    
    //mainMenu
    public abstract event Action MainMenu;
    protected abstract void OnMainMenu();
    
    //turnOnColorPicker
    public abstract event Action TurnOnColorPicker;
    protected abstract void OnTurnOnColorPicker();
    
    //turnOffColorPicker
    public abstract event Action TurnOffColorPicker;
    protected abstract void OnTurnOffColorPicker();
    
    //undo
    public abstract event Action Undo;
    protected abstract void OnUndo();
    
    //redo
    public abstract event Action Redo;
    protected abstract void OnRedo();
    
    //toggleBrushEraser
    public abstract event Action ToggleBrushEraser;
    protected abstract void OnToggleBrushEraser();
    
    

    protected virtual void Update()
    {
        UpdateRightHandDrawing();
        UpdateHeadDrawing();
    }
}
