using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager: BaseInputManager
{
    [SerializeField] OVRHand rightHand;
    [SerializeField] OVRSkeleton rightHandSkeleton;
    [SerializeField] OVRHand leftHand;
    [SerializeField] OVRSkeleton leftHandSkeleton;
    
    public override bool RightHandIsDrawing { get; protected set; }
    public override Vector3 RightHandDrawPosition { get; protected set; }
    
    public override bool LeftHandIsDrawing { get; protected set; }
    public override Vector3 LeftHandDrawPosition { get; protected set; }

    private void Update()
    {
        UpdateRightHandDrawing();
        UpdateLeftHandDrawing();
    }

    private void UpdateRightHandDrawing()
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
    
    private void UpdateLeftHandDrawing()
    {
        if (leftHand.IsTracked)
        {
            LeftHandIsDrawing = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (LeftHandIsDrawing)
            {
                foreach (var b in leftHandSkeleton.Bones)
                {
                    if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                    {
                        LeftHandDrawPosition = b.Transform.position;
                        break;
                    }
                }
            }
        }
    }
}

