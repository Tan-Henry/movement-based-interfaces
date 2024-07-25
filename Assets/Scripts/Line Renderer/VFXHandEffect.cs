using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXHandEffect : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    private VisualEffect visualEffectLeftHand;
    private VisualEffect visualEffectRightHand;
    private GameObject visualEffectGameobjectLeft;
    private GameObject visualEffectGameobjectRight;
    [SerializeField] private VisualEffectAsset bubbles;
    [SerializeField] private VisualEffectAsset butterflies;

    private void Start()
    {
        visualEffectGameobjectLeft = new GameObject();
        visualEffectGameobjectRight = new GameObject();
        visualEffectLeftHand = visualEffectGameobjectLeft.AddComponent<VisualEffect>();
        visualEffectLeftHand.visualEffectAsset = bubbles;
        visualEffectRightHand = visualEffectGameobjectRight.AddComponent<VisualEffect>();
        visualEffectRightHand.visualEffectAsset = bubbles;
    }

    void Update()
    {
        if (inputManager.RightHandIsEffecting)
        {
            visualEffectRightHand.transform.position = inputManager.RightHandPosition;
            visualEffectRightHand.Play();
        }

        if (inputManager.LeftHandIsEffecting)
        {
            visualEffectLeftHand.transform.position = inputManager.LeftHandPosition;
            visualEffectLeftHand.Play();
        }
    }

    public void ChangeEffectBubbles()
    {
        visualEffectLeftHand.visualEffectAsset = bubbles;
        visualEffectRightHand.visualEffectAsset = bubbles;
    }
    
    public void ChangeEffectButterflies()
    {
        visualEffectLeftHand.visualEffectAsset = butterflies;
        visualEffectRightHand.visualEffectAsset = butterflies;
    }
}