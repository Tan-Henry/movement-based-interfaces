using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorToHandController : MonoBehaviour
{
    public GameObject cursor;
    
    public OVRHand leftHand;
    public OVRSkeleton leftHandSkeleton;
    private bool isIndexFingerPinching;
    public CursorController controller;

    /*private void Awake()
    {
        controller = GameObject.Find("cursor").GetComponent<CursorController>();
    }*/

    void Update()
    {
        if (leftHand.IsTracked)
        {
            // Gather info whether left hand is pinching
            isIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            cursor.transform.position = leftHand.transform.position;
            // Proceed only if left hand is pinching
            if (isIndexFingerPinching)
            {
                if (!controller.isPainting)
                {
                    controller.isPainting = true;
                }

            }
            else
            {
                if (controller.isPainting)
                {
                    controller.isPainting = false;
                }
            }
        }

    }
}


