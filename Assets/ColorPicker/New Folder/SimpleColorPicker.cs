using UnityEngine;
using UnityEngine.UI;

public class SimpleColorPicker : MonoBehaviour
{
    public RawImage colorWheel;
    public Slider brightnessSlider;
    public Text rValueText;
    public Text gValueText;
    public Text bValueText;
    public Image selectedColorDisplay;

    private Texture2D colorWheelTexture;
    private Color selectedColor;

    private void Start()
    {
        colorWheelTexture = colorWheel.texture as Texture2D;
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(colorWheel.rectTransform, Input.mousePosition, null, out Vector2 localPoint);
            if (RectTransformUtility.RectangleContainsScreenPoint(colorWheel.rectTransform, Input.mousePosition))
            {
                localPoint += new Vector2(colorWheel.rectTransform.rect.width * 0.5f, colorWheel.rectTransform.rect.height * 0.5f);
                int x = Mathf.Clamp((int)localPoint.x, 0, colorWheelTexture.width);
                int y = Mathf.Clamp((int)localPoint.y, 0, colorWheelTexture.height);
                selectedColor = colorWheelTexture.GetPixel(x, y);
                UpdateColorDisplay();
            }
        }
    }

    private void OnBrightnessChanged(float value)
    {
        Color.RGBToHSV(selectedColor, out float h, out float s, out _);
        selectedColor = Color.HSVToRGB(h, s, value);
        UpdateColorDisplay();
    }

    private void UpdateColorDisplay()
    {
        rValueText.text = Mathf.RoundToInt(selectedColor.r * 255).ToString();
        gValueText.text = Mathf.RoundToInt(selectedColor.g * 255).ToString();
        bValueText.text = Mathf.RoundToInt(selectedColor.b * 255).ToString();
        selectedColorDisplay.color = selectedColor;
    }
}
