using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Brushes2D : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;

        [SerializeField] private Toggle lineBrushStandard2DToggle;
        [SerializeField] private Toggle lineBrushMask2DToggle;
        [SerializeField] private Toggle dynamicBrushSize2DToggle;
        [SerializeField] private Toggle dynamicBrushGradient2DToggle;
        [SerializeField] private Toggle dynamicBrushColor2DToggle;

        private void Start()
        {
            lineBrushStandard2DToggle.onValueChanged.AddListener(OnLineBrushStandard2DToggleValueChanged);
            lineBrushMask2DToggle.onValueChanged.AddListener(OnLineBrushMask2DToggleValueChanged);
            dynamicBrushSize2DToggle.onValueChanged.AddListener(OnDynamicBrushSize2DToggleValueChanged);
            dynamicBrushGradient2DToggle.onValueChanged.AddListener(OnDynamicBrushGradient2DToggleValueChanged);
            dynamicBrushColor2DToggle.onValueChanged.AddListener(OnDynamicBrushColor2DToggleValueChanged);
        }

        private void Update()
        {
            lineBrushStandard2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.LINE &&
                                             inputManager.Current2DLineBrush == ELineBrushes2D.STANDARD;
            lineBrushMask2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.LINE &&
                                         inputManager.Current2DLineBrush == ELineBrushes2D.MASK;
            dynamicBrushSize2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.DYNAMIC &&
                                            inputManager.Current2DDynamicBrush == EDynamicBrushes2D.SIZE;
            dynamicBrushGradient2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.DYNAMIC &&
                                                inputManager.Current2DDynamicBrush == EDynamicBrushes2D.GRADIENT;
            dynamicBrushColor2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.DYNAMIC &&
                                             inputManager.Current2DDynamicBrush == EDynamicBrushes2D.COLOR;
        }

        private void OnLineBrushStandard2DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current2DBrushType = EBrushType2D.LINE;
                inputManager.Current2DLineBrush = ELineBrushes2D.STANDARD;
            }
        }

        private void OnLineBrushMask2DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current2DBrushType = EBrushType2D.LINE;
                inputManager.Current2DLineBrush = ELineBrushes2D.MASK;
            }
        }

        private void OnDynamicBrushSize2DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current2DBrushType = EBrushType2D.DYNAMIC;
                inputManager.Current2DDynamicBrush = EDynamicBrushes2D.SIZE;
            }
        }

        private void OnDynamicBrushGradient2DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current2DBrushType = EBrushType2D.DYNAMIC;
                inputManager.Current2DDynamicBrush = EDynamicBrushes2D.GRADIENT;
            }
        }

        private void OnDynamicBrushColor2DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.Current2DBrushType = EBrushType2D.DYNAMIC;
                inputManager.Current2DDynamicBrush = EDynamicBrushes2D.COLOR;
            }
        }
    }
}