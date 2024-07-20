using UnityEngine;
using UnityEngine.UI;

public class ColorWheelGenerator : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;

    public Texture2D GenerateColorWheelTexture()
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        float radius = textureWidth / 2f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                Vector2 pos = new Vector2(x, y);
                float distance = Vector2.Distance(pos, center);

                if (distance <= radius)
                {
                    float angle = Mathf.Atan2(pos.y - center.y, pos.x - center.x) * Mathf.Rad2Deg;
                    if (angle < 0) angle += 360;
                    float hue = angle / 360f;
                    float saturation = distance / radius;
                    Color color = Color.HSVToRGB(hue, saturation, 1);
                    texture.SetPixel(x, y, color);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();
        return texture;
    }

    void Start()
    {
        Texture2D colorWheelTexture = GenerateColorWheelTexture();
        // Assign this texture to the RawImage component in your UI
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = colorWheelTexture;
    }
}
