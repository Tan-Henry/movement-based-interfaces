using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Effects : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Toggle bubblesToggle;
        [SerializeField] private Toggle coolToggle;
        [SerializeField] private Toggle heatMapToggle;
        [SerializeField] private Button noneToggle;
        
        private void Start()
        {
            bubblesToggle.onValueChanged.AddListener(OnBubblesToggleValueChanged);
            coolToggle.onValueChanged.AddListener(OnCoolToggleValueChanged);
            heatMapToggle.onValueChanged.AddListener(OnHeatMapToggleValueChanged);
            noneToggle.onClick.AddListener(OnNoneToggleClicked);
        }

        private void Update()
        {
            bubblesToggle.SetIsOnWithoutNotify(inputManager.CurrentEffect == EEffects.BUBBLES);
            coolToggle.SetIsOnWithoutNotify(inputManager.CurrentEffect == EEffects.COOL);
            heatMapToggle.SetIsOnWithoutNotify(inputManager.CurrentEffect == EEffects.HEATMAP);
            noneToggle.interactable = inputManager.CurrentEffect != EEffects.NONE;
        }

        private void OnBubblesToggleValueChanged(bool value)
        {
            inputManager.CurrentEffect = EEffects.BUBBLES;
        }
        
        private void OnCoolToggleValueChanged(bool value)
        {
            inputManager.CurrentEffect = EEffects.COOL;
        }
        
        private void OnHeatMapToggleValueChanged(bool value)
        {
            inputManager.CurrentEffect = EEffects.HEATMAP;
        }
        
        public void OnNoneToggleClicked()
        {
            inputManager.CurrentEffect = EEffects.NONE;
        }
        
    }
}