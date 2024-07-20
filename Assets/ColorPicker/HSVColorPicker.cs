using UnityEngine;

public class HSVColorPicker : ColorPickerBase
{
    public GameObject innerCubeHue;
    public GameObject innerCubeSaturation;
    public GameObject innerCubeValue;
    [Range(0, 360)] public int hue = 0;
    [Range(0, 100)] public int saturation = 0;
    [Range(0, 100)] public int value = 0;

    protected override void Start()
    {
        innerCubes = new GameObject[] { innerCubeHue, innerCubeSaturation, innerCubeValue };
        innerCubeSize = innerCubeHue.GetComponent<Renderer>().bounds.size;
        base.Start();
    }

    protected override Color CalculateColor(Vector3 position)
    {
        float h = position.x / largerCubeSize.x;
        float s = position.y / largerCubeSize.y;
        float v = position.z / largerCubeSize.z;
        return Color.HSVToRGB(h, s, v);
    }

    protected override bool GetUseHSV() => true;

    protected override void UpdateInnerCubePositions()
    {
        innerCubeHue.transform.localPosition = new Vector3(
            (hue / 360f) * largerCubeSize.x - largerCubeSize.x / 2, 0, 0);
        innerCubeSaturation.transform.localPosition = new Vector3(
            0, (saturation / 100f) * largerCubeSize.y - largerCubeSize.y / 2, 0);
        innerCubeValue.transform.localPosition = new Vector3(
            0, 0, (value / 100f) * largerCubeSize.z - largerCubeSize.z / 2);
    }
    

    protected override void UpdateSelectedColor()
    {
        Vector3 huePos = innerCubeHue.transform.localPosition + (largerCubeSize / 2);
        Vector3 saturationPos = innerCubeSaturation.transform.localPosition + (largerCubeSize / 2);
        Vector3 valuePos = innerCubeValue.transform.localPosition + (largerCubeSize / 2);

        hue = Mathf.RoundToInt((huePos.x / largerCubeSize.x) * 360);
        saturation = Mathf.RoundToInt((saturationPos.y / largerCubeSize.y) * 100);
        value = Mathf.RoundToInt((valuePos.z / largerCubeSize.z) * 100);

        SelectedColor = Color.HSVToRGB(hue / 360f, saturation / 100f, value / 100f);
        innerCubeHue.GetComponent<Renderer>().material.color = SelectedColor;
        innerCubeSaturation.GetComponent<Renderer>().material.color = SelectedColor;
        innerCubeValue.GetComponent<Renderer>().material.color = SelectedColor;
    }

    protected override void UpdateCubeVisibility() 
    {
        innerCubeHue.SetActive(true);
        innerCubeSaturation.SetActive(true);
        innerCubeValue.SetActive(true);
    }
}
