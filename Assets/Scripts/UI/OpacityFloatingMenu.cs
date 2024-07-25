using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OpacityFloatingMenu : MonoBehaviour
    {
       
        [SerializeField] private BaseInputManager inputManager;
        [SerializeField] private GameObject floatingMenu;
        [SerializeField] private Slider opacitySlider;
        [SerializeField] private LineRenderer previewLine;

        private void Update()
        {

            if (!inputManager.whatever)
            {
                floatingMenu.SetActive(false);
            } else
            {
                floatingMenu.SetActive(true);
                opacitySlider.value = inputManager.Current2DBrushSettings.opacity;
                previewLine.material.color = new Color(previewLine.material.color.r, previewLine.material.color.g, previewLine.material.color.b, inputManager.Current2DBrushSettings.opacity);
                previewLine.material.color = new Color(previewLine.material.color.r, previewLine.material.color.g, previewLine.material.color.b, inputManager.Current2DBrushSettings.opacity);
                previewLine.startWidth = inputManager.Current2DBrushSettings.brushSize * (0.3f / 5f);
                previewLine.endWidth = inputManager.Current2DBrushSettings.brushSize * (0.3f / 5f);
            }
        }
    }
}
