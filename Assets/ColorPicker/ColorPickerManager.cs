using UnityEngine;

public class ColorPickerManager : MonoBehaviour
{
    public GameObject rgbColorPickerPrefab;
    public GameObject hsvColorPickerPrefab;

    private ColorPickerBase colorPicker;

    void Start()
    {
        // Instantiate the RGB color picker prefab
      //  GameObject rgbColorPickerObject = Instantiate(rgbColorPickerPrefab);
      //  colorPicker = rgbColorPickerObject.GetComponent<ColorPickerBase>();

        // If you want to use HSV color picker instead, comment the above two lines and uncomment the below lines
         GameObject hsvColorPickerObject = Instantiate(hsvColorPickerPrefab);
         colorPicker = hsvColorPickerObject.GetComponent<ColorPickerBase>();
    }

    void Update()
    {
        if (colorPicker != null)
        {
            Debug.Log("Selected Color: " + colorPicker.SelectedColor);
        }
    }
}
