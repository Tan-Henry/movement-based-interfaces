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

    //Input-Events and Values

    public override bool RightHandIsDrawing2D { get; protected set; }
    public override bool RightHandIsDrawing3D { get; protected set; }
    public override bool RightHandIsErasing2D { get; protected set; }
    public override bool RightHandIsErasing3D { get; protected set; }
    public override Vector3 RightHandPosition { get; protected set; }
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


    // App-State

    public override bool IsDrawingState { get; set; }
    public override EMode CurrentMode { get; set; }
    public override EBrushCategory CurrentBrushCategory { get; set; }
    public override EBrushType2D Current2DBrushType { get; set; }
    public override List<ELineBrushes2D> Available2DLineBrushes { get; set; }
    public override ELineBrushes2D Current2DLineBrush { get; set; }
    public override List<EDynamicBrushes2D> Available2DDynamicBrushes { get; set; }
    public override EDynamicBrushes2D Current2DDynamicBrush { get; set; }
    public override EBrushType3D Current3DBrushType { get; set; }
    public override List<ELineBrushes3D> Available3DLineBrushes { get; set; }
    public override ELineBrushes3D Current3DLineBrush { get; set; }
    public override List<ETexturedBrushes3D> Available3DTexturedBrushes { get; set; }
    public override ETexturedBrushes3D Current3DTexturedBrush { get; set; }
    public override List<EStructuralBrushes3D> Available3DStructuralBrushes { get; set; }
    public override EStructuralBrushes3D Current3DStructuralBrush { get; set; }
    public override BrushSettings2D Current2DBrushSettings { get; set; }
    public override BrushSettings3D Current3DBrushSettings { get; set; }
    public override List<EEffects> AvailableEffects { get; set; }
    public override EEffects CurrentEffect { get; set; }

    private void Start()
    {
        InitializeState();
    }

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

    protected override void UpdateRightHand()
    {
        if (rightHand.IsTracked)
        {
            RightHandIsDrawing2D = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (RightHandIsDrawing2D)
            {
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                    {
                        RightHandPosition = b.Transform.position;
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

    private void InitializeState()
    {
        IsDrawingState = false;
        CurrentMode = EMode.MAIN_MENU;
        CurrentBrushCategory = EBrushCategory.NONE;
        Current2DBrushType = EBrushType2D.NONE;
        Available2DLineBrushes = new List<ELineBrushes2D>((ELineBrushes2D[])Enum.GetValues(typeof(ELineBrushes2D)));
        Current2DLineBrush = ELineBrushes2D.NONE;
        Available2DDynamicBrushes = new List<EDynamicBrushes2D>((EDynamicBrushes2D[])Enum.GetValues(typeof(EDynamicBrushes2D)));
        Current2DDynamicBrush = EDynamicBrushes2D.NONE;
        Current3DBrushType = EBrushType3D.NONE;
        Available3DLineBrushes = new List<ELineBrushes3D>((ELineBrushes3D[])Enum.GetValues(typeof(ELineBrushes3D)));
        Current3DLineBrush = ELineBrushes3D.NONE;
        Available3DTexturedBrushes = new List<ETexturedBrushes3D>((ETexturedBrushes3D[])Enum.GetValues(typeof(ETexturedBrushes3D)));
        Current3DTexturedBrush = ETexturedBrushes3D.NONE;
        Available3DStructuralBrushes = new List<EStructuralBrushes3D>((EStructuralBrushes3D[])Enum.GetValues(typeof(EStructuralBrushes3D)));
        Current3DStructuralBrush = EStructuralBrushes3D.NONE;
        Current2DBrushSettings = new BrushSettings2D { brushSize = 1f, opacity = 1 };
        Current3DBrushSettings = new BrushSettings3D { brushSize = 1f };
        AvailableEffects = new List<EEffects>((EEffects[])Enum.GetValues(typeof(EEffects)));
        CurrentEffect = EEffects.NONE;
    }
}