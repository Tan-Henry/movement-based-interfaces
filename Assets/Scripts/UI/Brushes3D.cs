using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Brushes3D : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Toggle lineBrushSharp3DToggle;
        [SerializeField] private Toggle lineBrushSmooth3DToggle;
        [SerializeField] private Toggle lineBrushPseudoSpaced3DToggle;
        [SerializeField] private Toggle textureBrushSimpleNoise3DToggle;
        [SerializeField] private Toggle textureBrushNoiseField3DToggle;
        [SerializeField] private Toggle textureBrushColumnNoise3DToggle;
        [SerializeField] private Toggle structuralBrushSlices3DToggle;
        [SerializeField] private Toggle structuralBrushSpikes3DToggle;
        
        private void Start()
        {
            lineBrushSharp3DToggle.onValueChanged.AddListener(OnLineBrushSharp3DToggleValueChanged);
            lineBrushSmooth3DToggle.onValueChanged.AddListener(OnLineBrushSmooth3DToggleValueChanged);
            lineBrushPseudoSpaced3DToggle.onValueChanged.AddListener(OnLineBrushPseudoSpaced3DToggleValueChanged);
            textureBrushSimpleNoise3DToggle.onValueChanged.AddListener(OnTextureBrushSimpleNoise3DToggleValueChanged);
            textureBrushNoiseField3DToggle.onValueChanged.AddListener(OnTextureBrushNoiseField3DToggleValueChanged);
            textureBrushColumnNoise3DToggle.onValueChanged.AddListener(OnTextureBrushColumnNoise3DToggleValueChanged);
            structuralBrushSlices3DToggle.onValueChanged.AddListener(OnStructuralBrushSlices3DToggleValueChanged);
            structuralBrushSpikes3DToggle.onValueChanged.AddListener(OnStructuralBrushSpikes3DToggleValueChanged);
        }

        private void Update()
        {
            lineBrushSharp3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.LINE && inputManager.Current3DLineBrush == ELineBrushes3D.SHARP;
            lineBrushSmooth3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.LINE && inputManager.Current3DLineBrush == ELineBrushes3D.SMOOTH;
            lineBrushPseudoSpaced3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.LINE && inputManager.Current3DLineBrush == ELineBrushes3D.PSEUDO_SPACED;
            textureBrushSimpleNoise3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.TEXTURED && inputManager.Current3DTexturedBrush == ETexturedBrushes3D.SIMPLE_NOISE;
            textureBrushNoiseField3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.TEXTURED && inputManager.Current3DTexturedBrush == ETexturedBrushes3D.NOISE_FIELD;
            textureBrushColumnNoise3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.TEXTURED && inputManager.Current3DTexturedBrush == ETexturedBrushes3D.COLUMN_NOISE;
            structuralBrushSlices3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.STRUCTURAL && inputManager.Current3DStructuralBrush == EStructuralBrushes3D.SLICES;
            structuralBrushSpikes3DToggle.isOn = inputManager.Current3DBrushType == EBrushType3D.STRUCTURAL && inputManager.Current3DStructuralBrush == EStructuralBrushes3D.SPIKES;
        }

        private void OnLineBrushSharp3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.LINE;
                inputManager.Current3DLineBrush = ELineBrushes3D.SHARP;
            }
        }
        
        private void OnLineBrushSmooth3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.LINE;
                inputManager.Current3DLineBrush = ELineBrushes3D.SMOOTH;
            }
        }
        
        private void OnLineBrushPseudoSpaced3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.LINE;
                inputManager.Current3DLineBrush = ELineBrushes3D.PSEUDO_SPACED;
            }
        }
        
        private void OnTextureBrushSimpleNoise3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.TEXTURED;
                inputManager.Current3DTexturedBrush = ETexturedBrushes3D.SIMPLE_NOISE;
            }
        }
        
        private void OnTextureBrushNoiseField3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.TEXTURED;
                inputManager.Current3DTexturedBrush = ETexturedBrushes3D.NOISE_FIELD;
            }
        }
        
        private void OnTextureBrushColumnNoise3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.TEXTURED;
                inputManager.Current3DTexturedBrush = ETexturedBrushes3D.COLUMN_NOISE;
            }
        }
        
        private void OnStructuralBrushSlices3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.STRUCTURAL;
                inputManager.Current3DStructuralBrush = EStructuralBrushes3D.SLICES;
            }
        }
        
        private void OnStructuralBrushSpikes3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current3DBrushType = EBrushType3D.STRUCTURAL;
                inputManager.Current3DStructuralBrush = EStructuralBrushes3D.SPIKES;
            }
        }
    }
}