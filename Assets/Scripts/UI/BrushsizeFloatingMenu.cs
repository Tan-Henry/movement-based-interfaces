using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

namespace UI
{
    public class BrushSizeFloatingMenu: MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        [SerializeField] private GameObject floatingMenu;
        [SerializeField] private Slider brushSizeSlider;
        [SerializeField] private LineRenderer previewLine;

        private void Update()
        {
            if (inputManager.rightMiddleFingerPinching)
            {
                floatingMenu.SetActive(true);
                brushSizeSlider.value = inputManager.Current2DBrushSettings.brushSize;
                previewLine.startWidth = inputManager.Current2DBrushSettings.brushSize * (0.3f / 5f);
                previewLine.endWidth = inputManager.Current2DBrushSettings.brushSize * (0.3f / 5f);
                previewLine.material.color = new Color(previewLine.material.color.r, previewLine.material.color.g, previewLine.material.color.b, inputManager.Current2DBrushSettings.opacity);
                previewLine.material.color = new Color(previewLine.material.color.r, previewLine.material.color.g, previewLine.material.color.b, inputManager.Current2DBrushSettings.opacity);

            }
            else
            {
                floatingMenu.SetActive(false);
            }
        }
    }
}
