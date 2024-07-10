using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager: BaseInputManager
{
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private OVRSkeleton rightHandSkeleton;
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRSkeleton leftHandSkeleton;
    
    public override bool RightHandIsDrawing { get; protected set; }
    public override Vector3 RightHandDrawPosition { get; protected set; }
    public override event Action ChangeEffect;
    public override event Action ToggleBrushEraser;
    public override event Action Redo;
    public override event Action SwitchMode;
    public override event Action MainMenu;
    public override event Action TurnOnColorPicker;
    public override event Action TurnOffColorPicker;
    public override event Action Undo;

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateRightHandDrawing()
    {
        if (rightHand.IsTracked)
        {
            RightHandIsDrawing = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (RightHandIsDrawing)
            {
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                    {
                        RightHandDrawPosition = b.Transform.position;
                        break;
                    }
                }
            }
        }
    }
    
    protected override void OnChangeEffect()
    {
        ChangeEffect?.Invoke();
    }
    
    protected override void OnSwitchMode()
    {
        SwitchMode?.Invoke();
    }
    
    protected override void OnMainMenu()
    {
        MainMenu?.Invoke();
    }
    
    protected override void OnTurnOnColorPicker()
    {
        TurnOnColorPicker?.Invoke();
    }

    
    protected override void OnTurnOffColorPicker()
    {
        TurnOffColorPicker?.Invoke();
    }
    
    protected override void OnUndo()
    {
        Undo?.Invoke();
    }

    
    protected override void OnRedo()
    {
        Redo?.Invoke();
    }

    protected override void OnToggleBrushEraser()
    {
        ToggleBrushEraser?.Invoke();
    }

    private void checkChangeEffect()
    {
        if(!leftHand.IsTracked) return;
        if(!rightHand.IsTracked) return;

        if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            
        }
    }
}

