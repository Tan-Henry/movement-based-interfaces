using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Effects : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Toggle bubblesToggle;
        [SerializeField] private Toggle noneToggle;
        
        private void Start()
        {
            bubblesToggle.onValueChanged.AddListener(OnBubblesToggleValueChanged);
            noneToggle.onValueChanged.AddListener(OnNoneToggleValueChanged);
        }

        private void Update()
        {
            bubblesToggle.SetIsOnWithoutNotify(inputManager.CurrentEffect == EEffects.BUBBLES);
            noneToggle.SetIsOnWithoutNotify(inputManager.CurrentEffect == EEffects.NONE);
        }

        private void OnBubblesToggleValueChanged(bool value)
        {
            inputManager.CurrentEffect = EEffects.BUBBLES;
        }
        
        private void OnNoneToggleValueChanged(bool value)
        {
            inputManager.CurrentEffect = EEffects.NONE;
        }
        
    }
}