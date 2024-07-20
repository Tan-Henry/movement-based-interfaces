using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BrushCategoryMenu : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Toggle brushCategory2DToggle;
        [SerializeField] private Toggle brushCategory3DToggle;
        
        [SerializeField] private GameObject brushMenu2D;
        [SerializeField] private GameObject brushMenu3D;
        
        [SerializeField] private GameObject brushSettings2D;
        [SerializeField] private GameObject brushSettings3D;

        private void Start()
        {
            brushCategory2DToggle.onValueChanged.AddListener(OnBrushCategory2DToggleValueChanged);
            brushCategory3DToggle.onValueChanged.AddListener(OnBrushCategory3DToggleValueChanged);
            inputManager.OnBrushCategoryChanged += OnBrushCategoryChanged;
            
            UpdateUI(inputManager.CurrentBrushCategory);
        }

        private void OnDestroy()
        {
            inputManager.OnBrushCategoryChanged -= OnBrushCategoryChanged;
        }
        
        private void OnBrushCategoryChanged(EBrushCategory brushCategory)
        {
            Debug.Log("changed brush category to " + brushCategory);
            UpdateUI(brushCategory);
        }

        private void UpdateUI(EBrushCategory brushCategory)
        {
            brushMenu2D.SetActive(brushCategory == EBrushCategory.BRUSH_2D);
            brushMenu3D.SetActive(brushCategory == EBrushCategory.BRUSH_3D);
            
            brushSettings2D.SetActive(brushCategory == EBrushCategory.BRUSH_2D);
            brushSettings3D.SetActive(brushCategory == EBrushCategory.BRUSH_3D);
            
            brushCategory2DToggle.SetIsOnWithoutNotify(brushCategory == EBrushCategory.BRUSH_2D);
            brushCategory3DToggle.SetIsOnWithoutNotify(brushCategory == EBrushCategory.BRUSH_3D);
        }

        private void OnBrushCategory2DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.CurrentBrushCategory = EBrushCategory.BRUSH_2D;
            }
            else
            {
                if (brushCategory3DToggle.isOn)
                {
                    inputManager.CurrentBrushCategory = EBrushCategory.BRUSH_3D;
                }
                else
                {
                    inputManager.CurrentBrushCategory = EBrushCategory.NONE;
                }
            }
        }
        
        private void OnBrushCategory3DToggleValueChanged(bool value)
        {
            if (value)
            {
                inputManager.CurrentBrushCategory = EBrushCategory.BRUSH_3D;
            }
            else
            {
                if (brushCategory2DToggle.isOn)
                {
                    inputManager.CurrentBrushCategory = EBrushCategory.BRUSH_2D;
                }
                else
                {
                    inputManager.CurrentBrushCategory = EBrushCategory.NONE;
                }
            }
        }
    }
}
