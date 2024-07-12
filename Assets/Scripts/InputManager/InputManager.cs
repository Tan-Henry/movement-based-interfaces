using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class InputManager : BaseInputManager
{
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private OVRSkeleton rightHandSkeleton;
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRSkeleton leftHandSkeleton;
    [SerializeField] private Hmd Head;

    public override bool RightHandIsDrawing { get; protected set; }
    public override Vector3 RightHandDrawPosition { get; protected set; }
    public override Vector3 HeadDrawPosition { get; protected set; }
    public override event Action ChangeEffect;
    public override event Action ToggleBrushEraser;
    public override event Action Redo;
    public override event Action SwitchMode;
    public override event Action MainMenu;
    public override event Action TurnOnColorPicker;
    public override event Action TurnOffColorPicker;
    public override event Action Undo;
    
    private int pinchCounter = 0;
    private float lastPinchTime = 0.0f;
    private bool isPinching = false;

    protected override void Update()
    {
        base.Update();
        CheckChangeEffect();
        CheckTurnOnColorPicker();
        CheckTurnOffColorPicker();
        CheckSwitchMode();
        CheckMainMenu();
        CheckUndo();
        CheckRedo();
        CheckToggleBrushEraser();
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
    
    protected override void UpdateHeadDrawing()
    {
        if (Head)
        {
            HeadDrawPosition = Head.transform.position;
        }
    }

    private void CheckChangeEffect()
    {
        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;

        Vector3 LeftHandIndexPosition = Vector3.zero;

        foreach (var b in leftHandSkeleton.Bones)
        {
            if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                LeftHandIndexPosition = b.Transform.position;
                break;
            }
        }

        if (rightHand.transform.rotation.eulerAngles is { y: > 250.0f } &&
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) &&
            !leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            Vector3.Distance(LeftHandIndexPosition, rightHand.transform.position) < 10.0f)
        {
            OnChangeEffect();
        }
    }

    private void CheckSwitchMode()
    {
        //rightHand fist on leftHand palm

        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;

        //check if all fingers of the left hand are pinching
        if (!leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            !leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
            !leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) ||
            !leftHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            return;
        }

        if (rightHand.transform.rotation.eulerAngles is { y: > 250.0f } &&
            Vector3.Distance(leftHand.transform.position, rightHand.transform.position) < 10.0f)
        {
            OnSwitchMode();
        }
    }

    private void CheckMainMenu()
    {
        //both palms together

        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;

        //check no fingers on both hands are pinching
        if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            return;
        }

        if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
            rightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) ||
            rightHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            return;
        }

        //TODO: check if both palms are facing each other

        if (Vector3.Distance(leftHand.transform.position, rightHand.transform.position) < 10.0f)
        {
            OnMainMenu();
        }
    }

    private void CheckTurnOnColorPicker()
    {
        if (!leftHand.IsTracked) return;

        //check if no fingers are pinching
        if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            return;
        }

        //leftHand palm facing up
        if (leftHand.transform.rotation.eulerAngles is { y: > 250.0f })
        {
            OnTurnOnColorPicker();
        }
    }

    private void CheckTurnOffColorPicker()
    {
        if (!leftHand.IsTracked) return;

        //leftHand palm facing down
        if (leftHand.transform.rotation.eulerAngles is { y: < 80.0f })
        {
            OnTurnOffColorPicker();
        }
    }

    private void CheckUndo()
    {
        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;

        //shake left leg
    }

    private void CheckRedo()
    {
        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;

        //shake right leg
    }

    private void CheckToggleBrushEraser()
    {
        //rightHand pinch twice quickly
        //TODO: block drawing while double clicking
        if (!rightHand.IsTracked) return;

        //check if right index finger pinches twice quickly in a short time
        if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            if (!isPinching)
            {
                isPinching = true;
                float currentTime = Time.time;
        
                if (pinchCounter == 0 || (currentTime - lastPinchTime) < 0.5f)
                {
                    pinchCounter++;
                    lastPinchTime = currentTime;

                    if (pinchCounter == 2)
                    {
                        pinchCounter = 0;
                        OnToggleBrushEraser();
                    }
                }
                else
                {
                    pinchCounter = 0;
                }
            }
            else
            {
                isPinching = false;
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

    
}
