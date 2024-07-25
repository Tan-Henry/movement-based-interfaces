using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXHandEffect : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    private VisualEffect visualEffect;
    private GameObject visualEffectGameobject;
    [SerializeField] private VisualEffectAsset bubbles;
    [SerializeField] private VisualEffectAsset butterflies;

    private void Start()
    {
        visualEffectGameobject = new GameObject();
        visualEffect = visualEffectGameobject.AddComponent<VisualEffect>();
        visualEffect.visualEffectAsset = bubbles;
    }

    void Update()
    {
        if (inputManager.RightHandIsEffecting)
        {
            visualEffect.transform.position = inputManager.RightHandPosition;
            visualEffect.Play();
        }

        if (inputManager.LeftHandIsEffecting)
        {
            visualEffect.transform.position = inputManager.LeftHandPosition;
            visualEffect.Play();
        }
    }

    public void ChangeEffectBubbles()
    {
        visualEffect.visualEffectAsset = bubbles;
    }
    
    public void ChangeEffectButterflies()
    {
        visualEffect.visualEffectAsset = butterflies;
    }
}