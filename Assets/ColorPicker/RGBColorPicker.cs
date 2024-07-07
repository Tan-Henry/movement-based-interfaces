using UnityEngine;

public class RGBColorPicker : ColorPickerBase
{
    public GameObject innerCubeRGB;
    [Range(0, 255)] public int red = 0;
    [Range(0, 255)] public int green = 0;
    [Range(0, 255)] public int blue = 0;

    [SerializeField]
    private Color selectedColor;
    public Color SelectedColor
    {
        get { return selectedColor; }
        private set { selectedColor = value; }
    }

    protected override void Start()
    {
        innerCubes = new GameObject[] { innerCubeRGB };
        innerCubeSize = innerCubeRGB.GetComponent<Renderer>().bounds.size;
        base.Start();
    }

    protected override Color CalculateColor(Vector3 position)
    {
        return new Color(position.x / largerCubeSize.x, position.y / largerCubeSize.y, position.z / largerCubeSize.z);
    }

    protected override bool GetUseHSV() => false;

    protected override void UpdateInnerCubePositions()
    {
        innerCubeRGB.transform.localPosition = new Vector3(
            (red / 255f) * largerCubeSize.x - largerCubeSize.x / 2,
            (green / 255f) * largerCubeSize.y - largerCubeSize.y / 2,
            (blue / 255f) * largerCubeSize.z - largerCubeSize.z / 2);
    }

    protected override void UpdateSelectedColor()
    {
        Vector3 localPos = innerCubeRGB.transform.localPosition + (largerCubeSize / 2);
        red = Mathf.RoundToInt((localPos.x / largerCubeSize.x) * 255);
        green = Mathf.RoundToInt((localPos.y / largerCubeSize.y) * 255);
        blue = Mathf.RoundToInt((localPos.z / largerCubeSize.z) * 255);

        SelectedColor = new Color(red / 255f, green / 255f, blue / 255f);
        innerCubeRGB.GetComponent<Renderer>().material.color = SelectedColor;
    }

    protected override void UpdateCubeVisibility()
    {
        innerCubeRGB.SetActive(true);
    }
}
