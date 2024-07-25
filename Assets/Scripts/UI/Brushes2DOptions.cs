using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Brushes2DOptions : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Slider brushSizeSlider2D;
        [SerializeField] private Slider brushOpacitySlider2D;
        
        [SerializeField] private GameObject opacityObject;
        
        private void Start()
        {
            brushSizeSlider2D.onValueChanged.AddListener(OnBrushSizeSlider2DValueChanged);
            brushOpacitySlider2D.onValueChanged.AddListener(OnBrushOpacitySlider2DValueChanged);
        }
        
        private void Update()
        {
            brushSizeSlider2D.value = inputManager.Current2DBrushSettings.brushSize;
            brushOpacitySlider2D.value = inputManager.Current2DBrushSettings.opacity;
            
            opacityObject.gameObject.SetActive(!OpacityShouldBeHidden());
        }
        
        private bool OpacityShouldBeHidden()
        {
            return inputManager.Current2DBrushType == EBrushType2D.LINE && inputManager.Current2DLineBrush == ELineBrushes2D.MASK;
        }
        
        private void OnBrushSizeSlider2DValueChanged(float value)
        {
            inputManager.Current2DBrushSettings.brushSize = value;
        }
        
        private void OnBrushOpacitySlider2DValueChanged(float value)
        {
            inputManager.Current2DBrushSettings.opacity = value;
        }
        
    }
}