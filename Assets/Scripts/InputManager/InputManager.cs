using System;
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
    [SerializeField] private BaseSensorServer sensorServer;
    [SerializeField] private GameObject rightHandCursor;
    [SerializeField] private GameObject leftHandCursor;


    //Input-Events and Values

    public override bool RightHandIsDrawing2D { get; protected set; }
    public override bool RightHandIsDrawing3D { get; protected set; }
    public override bool RightHandIsErasing2D { get; protected set; }
    public override bool RightHandIsErasing3D { get; protected set; }
    public override bool RightHandIsEffecting { get; protected set; }
    public override bool LeftHandIsEffecting { get; protected set; }
    public override bool RightHandIsColorPicking { get; protected set; }
    public override Vector3 RightHandPosition { get; protected set; }
    public override Vector3 LeftHandPosition { get; protected set; }
    public override Vector3 HeadDrawPosition { get; protected set; }
    public override event Action ChangeEffect;
    public override event Action Redo;
    public override event Action SwitchMode;
    public override event Action MainMenu;
    public override event Action TurnOnColorPicker;
    public override event Action TurnOffColorPicker;
    public override event Action Undo;
    public override event Action ResetMenu;
    
    public override bool rightMiddleFingerPinching {get; set;}
    private Vector3 lastMiddleFingerPosition = Vector3.zero;
    public override bool rightRingFingerPinching {get; set;}
    private Vector3 lastRingFingerPosition = Vector3.zero;
    private const float SizeSensitivity = 0.5f;
    private const float OpacitySensitivity = 0.1f;
    private bool leftIndexFingerPinching = false;
    private bool isPointingAtUI = false;
    private bool isChangingEffect = false;
    private bool isSwitchingMode = false;

    public override bool BlockedByHandle { get; set; }
    public override bool BlockedByColorPicker { get; set; }

    // App-State

    //True -> Drawing, False -> Erasing
    public override bool IsDrawingState { get; set; }
    public override EMode CurrentMode { get; set; }
    
    private EBrushCategory currentBrushCategory;
    public override EBrushCategory CurrentBrushCategory
    {
        get => currentBrushCategory;
        set
        {
            if (currentBrushCategory == value) return;
            currentBrushCategory = value;
            OnBrushCategoryChanged?.Invoke(currentBrushCategory);
        }
    }
    public override event Action<EBrushCategory> OnBrushCategoryChanged;
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
        sensorServer.ShakeLeft += OnUndo;
        sensorServer.ShakeRight += OnRedo;
        sensorServer.ShakeBoth += OnResetMenu;
    }

    protected override void Update()
    {
        CheckIsPointingAtUI();
        base.Update();
        CheckChangeEffect();
        CheckTurnOnColorPicker();
        CheckTurnOffColorPicker();
        CheckSwitchMode();
        CheckMainMenu();
        CheckToggleBrushEraser();
        CheckHandsEffecting();
    }
    
    private void CheckIsPointingAtUI()
    {
        if (!rightHand.IsTracked && !leftHand.IsTracked) return;
        
        if (rightHandCursor.activeSelf || leftHandCursor.activeSelf)
        {
            isPointingAtUI = true;
        }
        else
        {
            isPointingAtUI = false;
        }
    }

    protected override void UpdateRightHand()
    {
        if (!rightHand.IsTracked) return;
        
        foreach (var b in rightHandSkeleton.Bones)
        {
            if (b.Id != OVRSkeleton.BoneId.Hand_IndexTip) continue;
            RightHandPosition = b.Transform.position;
            break;
        }
        
        //index finger pinching
        CheckIndexFingerPinching();

        //middle finger pinching
        CheckMiddleFingerPinching();
            
        //ring finger pinching
        CheckRingFingerPinching();
    }

    private void CheckIndexFingerPinching()
    {
        RightHandIsColorPicking = BlockedByColorPicker && rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (IsDrawingState)
        {
            if (((BlockedByHandle || isPointingAtUI || BlockedByColorPicker) && !RightHandIsDrawing2D && !RightHandIsDrawing3D && !RightHandIsErasing2D && !RightHandIsErasing3D) || CurrentMode != EMode.CREATE)
            {
                RightHandIsDrawing2D = false;
                RightHandIsErasing2D = false;
                
                RightHandIsDrawing3D = false;
                RightHandIsErasing3D = false;
                return;
            }
            
            if (CurrentBrushCategory == EBrushCategory.BRUSH_2D)
            {
                RightHandIsDrawing2D = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                RightHandIsErasing2D = false;

                RightHandIsErasing3D = false;
                RightHandIsDrawing3D = false;
            }

            if (CurrentBrushCategory == EBrushCategory.BRUSH_3D)
            {
                RightHandIsDrawing3D = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                RightHandIsErasing3D = false;

                RightHandIsErasing2D = false;
                RightHandIsDrawing2D = false;
            }
        }
        else
        {
            if (((BlockedByHandle || isPointingAtUI || BlockedByColorPicker) && !RightHandIsDrawing2D && !RightHandIsDrawing3D && !RightHandIsErasing2D && !RightHandIsErasing3D) || CurrentMode != EMode.CREATE)
            {
                RightHandIsDrawing2D = false;
                RightHandIsErasing2D = false;
                
                RightHandIsDrawing3D = false;
                RightHandIsErasing3D = false;
                return;
            }
            
            if (CurrentBrushCategory == EBrushCategory.BRUSH_2D)
            {
                RightHandIsErasing2D = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                RightHandIsDrawing2D = false;

                RightHandIsDrawing3D = false;
                RightHandIsErasing3D = false;
            }

            if (CurrentBrushCategory == EBrushCategory.BRUSH_3D)
            {
                RightHandIsErasing3D = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                RightHandIsDrawing3D = false;

                RightHandIsDrawing2D = false;
                RightHandIsErasing2D = false;
            }
        }
    }

    private void CheckMiddleFingerPinching()
    {
        if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            //if index finger is also pinching, no action
            if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) || CurrentMode != EMode.CREATE || CurrentBrushCategory == EBrushCategory.NONE || rightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
            {
                rightMiddleFingerPinching = false;
                return;
            }
            
            // map middle finger movement to brush size
            if (!rightMiddleFingerPinching)
            {
                rightMiddleFingerPinching = true;
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id != OVRSkeleton.BoneId.Hand_MiddleTip) continue;
                    lastMiddleFingerPosition = b.Transform.position;
                    break;
                }
            }
            else
            {
                Vector3 currentMiddleFingerPosition = Vector3.zero;
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id != OVRSkeleton.BoneId.Hand_MiddleTip) continue;
                    currentMiddleFingerPosition = b.Transform.position;
                    break;
                }

                float verticalMovement = currentMiddleFingerPosition.y - lastMiddleFingerPosition.y;
                float valueChange = verticalMovement * SizeSensitivity;
                lastMiddleFingerPosition = currentMiddleFingerPosition;

                if (CurrentBrushCategory == EBrushCategory.BRUSH_2D)
                {
                    Current2DBrushSettings.brushSize = Mathf.Clamp(Current2DBrushSettings.brushSize + valueChange,
                        Limits.MIN_BRUSH_SIZE, Limits.MAX_BRUSH_SIZE);
                }

                if (CurrentBrushCategory == EBrushCategory.BRUSH_3D)
                {
                    Current3DBrushSettings.brushSize = Mathf.Clamp(Current3DBrushSettings.brushSize + valueChange,
                        Limits.MIN_BRUSH_SIZE, Limits.MAX_BRUSH_SIZE);
                }
            }
        }
        else
        {
            rightMiddleFingerPinching = false;
        }
    }

    private void CheckRingFingerPinching()
    {
        if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            //if index finger is also pinching, no action
            if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) || CurrentMode != EMode.CREATE || CurrentBrushCategory != EBrushCategory.BRUSH_2D || rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
            {
                rightRingFingerPinching = false;
                return;
            }
                
            // map ring finger movement to brush size
            if (!rightRingFingerPinching)
            {
                rightRingFingerPinching = true;
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id != OVRSkeleton.BoneId.Hand_RingTip) continue;
                    lastRingFingerPosition = b.Transform.position;
                    break;
                }
            }
            else
            {
                Vector3 currentRingFingerPosition = Vector3.zero;
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id != OVRSkeleton.BoneId.Hand_RingTip) continue;
                    currentRingFingerPosition = b.Transform.position;
                    break;
                }
                    
                float verticalMovement = currentRingFingerPosition.y - lastRingFingerPosition.y;
                float valueChange = verticalMovement * OpacitySensitivity;
                lastRingFingerPosition = currentRingFingerPosition;
                Current2DBrushSettings.opacity = Mathf.Clamp(Current2DBrushSettings.opacity + valueChange, Limits.MIN_OPACITY, Limits.MAX_OPACITY);
            }
        }
        else
        {
            rightRingFingerPinching = false;
        }
    }

    private void CheckHandsEffecting()
    {
        if (BlockedByHandle || isPointingAtUI || BlockedByColorPicker || CurrentMode != EMode.PRESENT)
        {
            RightHandIsEffecting = false;
            LeftHandIsEffecting = false;
            return;
        }

        RightHandIsEffecting = rightHand.IsTracked;
        LeftHandIsEffecting = leftHand.IsTracked;
    }

    protected override void UpdateLeftHand()
    {
        if (!leftHand.IsTracked) return;
        foreach (var b in leftHandSkeleton.Bones)
        {
            if (b.Id != OVRSkeleton.BoneId.Hand_IndexTip) continue;
            LeftHandPosition = b.Transform.position;
            break;
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
        if (CurrentMode != EMode.PRESENT) return;

        Vector3 leftHandIndexPosition = Vector3.zero;

        foreach (var b in leftHandSkeleton.Bones)
        {
            if (b.Id != OVRSkeleton.BoneId.Hand_IndexTip) continue;
            leftHandIndexPosition = b.Transform.position;
            break;
        }
        
        if (rightHand.transform.rotation.eulerAngles is { y: > 215.0f } &&
            !leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            Vector3.Distance(leftHandIndexPosition, rightHand.transform.position) < 2.5f)
        {
            if (!isChangingEffect)
            {
                isChangingEffect = true;
                OnChangeEffect();
            }
        }else
        {
            isChangingEffect = false;
        }
    }

    private void CheckSwitchMode()
    {
        //leftHand fist on rightHand palm

        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;
        if (CurrentMode == EMode.TUTORIAL || CurrentMode == EMode.MAIN_MENU) return;

        if(!IsFingerBent(leftHandSkeleton, OVRSkeleton.BoneId.Hand_IndexTip) || !IsFingerBent(leftHandSkeleton, OVRSkeleton.BoneId.Hand_MiddleTip)) return;

        if (rightHand.transform.rotation.eulerAngles is { y: > 225.0f } &&
            Vector3.Distance(leftHand.transform.position, rightHand.transform.position) < 4f)
        {
            if (!isSwitchingMode)
            {
                OnSwitchMode();
                isSwitchingMode = true;
            }
        }
        else
        {
            isSwitchingMode = false;
        }
    }

    private void CheckMainMenu()
    {
        //both palms together

        if (!leftHand.IsTracked) return;
        if (!rightHand.IsTracked) return;
        if (CurrentMode == EMode.MAIN_MENU) return;

        //check all fingers are straight on both hands
        if(!IsFingerStraight(leftHandSkeleton, OVRSkeleton.BoneId.Hand_IndexTip) || !IsFingerStraight(leftHandSkeleton, OVRSkeleton.BoneId.Hand_MiddleTip)) return;
        if(!IsFingerStraight(rightHandSkeleton, OVRSkeleton.BoneId.Hand_IndexTip) || !IsFingerStraight(rightHandSkeleton, OVRSkeleton.BoneId.Hand_MiddleTip)) return;
        
        //check if palms are facing each other
        if (leftHand.transform.rotation.eulerAngles.x < 5f || leftHand.transform.rotation.eulerAngles.x > 30f ||
            leftHand.transform.rotation.eulerAngles.y < 150f || leftHand.transform.rotation.eulerAngles.y > 190f ||
            leftHand.transform.rotation.eulerAngles.z < 75f || leftHand.transform.rotation.eulerAngles.z > 95f) return;
        
        if (rightHand.transform.rotation.eulerAngles.x < 300f || rightHand.transform.rotation.eulerAngles.x > 355f ||
            rightHand.transform.rotation.eulerAngles.y < 340f || rightHand.transform.rotation.eulerAngles.y > 360f ||
            rightHand.transform.rotation.eulerAngles.z < 270f || rightHand.transform.rotation.eulerAngles.z > 290f) return;
        
        if (Vector3.Distance(leftHand.transform.position, rightHand.transform.position) < 4.0f)
        {
            OnMainMenu();
        }
    }

    private void CheckTurnOnColorPicker()
    {
        if (!leftHand.IsTracked) return;

        if (RightHandIsDrawing2D || RightHandIsErasing2D || RightHandIsDrawing3D || RightHandIsErasing3D ||
            BlockedByHandle || CurrentMode != EMode.CREATE)
        {
            OnTurnOffColorPicker();
            return;
        }

        //check if no fingers are pinching
        if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) ||
            leftHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            return;
        }
        
        Debug.Log(leftHand.transform.rotation.eulerAngles.y);
        //leftHand palm facing up
        if (leftHand.transform.rotation.eulerAngles is { y: > 240.0f })
        {
            OnTurnOnColorPicker();
        }
    }

    private void CheckTurnOffColorPicker()
    {
        //leftHand palm facing down
        if (leftHand.transform.rotation.eulerAngles is { y: < 235.0f } || IsFingerBent(leftHandSkeleton, OVRSkeleton.BoneId.Hand_IndexTip) || !leftHand.IsTracked)
        {
            OnTurnOffColorPicker();
        }
    }

    private void CheckToggleBrushEraser()
    {
        //left hand index finger pinching
        if (!leftHand.IsTracked || RightHandIsDrawing2D || RightHandIsErasing2D || RightHandIsDrawing3D || RightHandIsErasing3D || isPointingAtUI || BlockedByHandle || BlockedByColorPicker) return;

        if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            if (leftIndexFingerPinching)
            {
                return;
            }
            leftIndexFingerPinching = true;
            OnToggleBrushEraser();
        }
        else
        {
            leftIndexFingerPinching = false;
        }
    }

    protected override void OnChangeEffect()
    {
        if (CurrentMode != EMode.PRESENT)
        {
            return;
        }
        
        //switch current effect to next effect in list of available effects, if current effect is the last one, switch to the first one
        int currentIndex = AvailableEffects.IndexOf(CurrentEffect);
        if (currentIndex == AvailableEffects.Count - 1)
        {
            CurrentEffect = AvailableEffects[0];
        }
        else
        {
            CurrentEffect = AvailableEffects[currentIndex + 1];
        }
        
        ChangeEffect?.Invoke();
    }

    protected override void OnSwitchMode()
    {
        switch (CurrentMode)
        {
            case EMode.MAIN_MENU or EMode.TUTORIAL:
                return;
            case EMode.CREATE:
                CurrentMode = EMode.PRESENT;
                break;
            default:
                CurrentMode = EMode.CREATE;
                break;
        }

        SwitchMode?.Invoke();
    }

    public override void OnMainMenu()
    {
        CurrentMode = EMode.MAIN_MENU;
        CurrentEffect = EEffects.NONE;
        Current2DBrushType = EBrushType2D.NONE;
        Current3DBrushType = EBrushType3D.NONE;
        CurrentBrushCategory = EBrushCategory.NONE;
        Current2DLineBrush = ELineBrushes2D.NONE;
        Current2DDynamicBrush = EDynamicBrushes2D.NONE;
        Current3DLineBrush = ELineBrushes3D.NONE;
        Current3DTexturedBrush = ETexturedBrushes3D.NONE;
        Current3DStructuralBrush = EStructuralBrushes3D.NONE;
        
        MainMenu?.Invoke();
    }

    protected override void OnTurnOnColorPicker()
    {
        if (CurrentMode != EMode.CREATE) return;
        TurnOnColorPicker?.Invoke();
    }
    
    protected override void OnTurnOffColorPicker()
    {
        TurnOffColorPicker?.Invoke();
    }

    public override void OnUndo()
    {
        if (CurrentMode != EMode.CREATE) return;
        Undo?.Invoke();
    }
    
    public override void OnRedo()
    {
        if (CurrentMode != EMode.CREATE) return;
        Redo?.Invoke();
    }

    public override void OnToggleBrushEraser()
    {
        if (CurrentMode != EMode.CREATE) return;
        IsDrawingState = !IsDrawingState;
    }
    
    protected override void OnResetMenu()
    {
        ResetMenu?.Invoke();
    }
    
    private bool IsFingerBent(OVRSkeleton skeleton, OVRSkeleton.BoneId fingerTipId)
    {
        var fingerTip = skeleton.Bones[(int)fingerTipId].Transform;
        var palm = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform;

        float distance = Vector3.Distance(fingerTip.position, palm.position);
        return distance < 2.35f;
    }
    
    private bool IsFingerStraight(OVRSkeleton skeleton, OVRSkeleton.BoneId fingerTipId)
    {
        var fingerTip = skeleton.Bones[(int)fingerTipId].Transform;
        var palm = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform;

        float distance = Vector3.Distance(fingerTip.position, palm.position);
        return distance > 5f;
    }

    private void InitializeState()
    {
        IsDrawingState = true;
        CurrentMode = EMode.CREATE;
        CurrentBrushCategory = EBrushCategory.BRUSH_2D;
        Current2DBrushType = EBrushType2D.LINE;
        Available2DLineBrushes = new List<ELineBrushes2D>((ELineBrushes2D[])Enum.GetValues(typeof(ELineBrushes2D)));
        Current2DLineBrush = ELineBrushes2D.STANDARD;
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
        Current3DBrushSettings = new BrushSettings3D { brushSize = 2f };
        AvailableEffects = new List<EEffects>((EEffects[])Enum.GetValues(typeof(EEffects)));
        CurrentEffect = EEffects.NONE;
    }
}