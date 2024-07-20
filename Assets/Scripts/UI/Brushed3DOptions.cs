using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Brushed3DOptions : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Slider brushSizeSlider3D;
        
        private void Start()
        {
            brushSizeSlider3D.onValueChanged.AddListener(OnBrushSizeSlider3DValueChanged);
        }
        
        private void Update()
        {
            brushSizeSlider3D.value = inputManager.Current3DBrushSettings.brushSize;
        }
        
        private void OnBrushSizeSlider3DValueChanged(float value)
        {
            inputManager.Current3DBrushSettings.brushSize = value;
        }
    }
}