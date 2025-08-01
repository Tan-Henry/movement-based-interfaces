﻿using System;
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

        private void Start()
        {
            lineBrushStandard2DToggle.onValueChanged.AddListener(OnLineBrushStandard2DToggleValueChanged);
            lineBrushMask2DToggle.onValueChanged.AddListener(OnLineBrushMask2DToggleValueChanged);
            dynamicBrushSize2DToggle.onValueChanged.AddListener(OnDynamicBrushSize2DToggleValueChanged);
           
        }

        private void Update()
        {
            lineBrushStandard2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.LINE &&
                                             inputManager.Current2DLineBrush == ELineBrushes2D.STANDARD;
            lineBrushMask2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.LINE &&
                                         inputManager.Current2DLineBrush == ELineBrushes2D.MASK;
            dynamicBrushSize2DToggle.isOn = inputManager.Current2DBrushType == EBrushType2D.DYNAMIC &&
                                            inputManager.Current2DDynamicBrush == EDynamicBrushes2D.SIZE;
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
    }
}