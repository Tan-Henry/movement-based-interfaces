using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public static ShaderManager Instance { get; private set; }

    [System.Serializable]
    public class ShaderKeyMapping
    {
        public string name;
        public Material material;
    }

    public List<ShaderKeyMapping> shaderMappings = new List<ShaderKeyMapping>();
    private int currentShaderIndex = 0;
    private bool isShaderApplied = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleShader();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollShader(-1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ScrollShader(1);
        }
    }

    private void ToggleShader()
    {
        if (shaderMappings.Count == 0)
        {
            Debug.LogWarning("No shaders available in ShaderManager.");
            return;
        }

        isShaderApplied = !isShaderApplied;

        foreach (var brush in FindObjectsOfType<DynamicBrushDrawer>())
        {
            if (isShaderApplied)
            {
                brush.ApplyMaterial(shaderMappings[currentShaderIndex].material);
            }
            else
            {
                brush.RevertMaterial();
            }
        }
    }

    private void ScrollShader(int direction)
    {
        if (shaderMappings.Count == 0)
        {
            Debug.LogWarning("No shaders available in ShaderManager.");
            return;
        }

        currentShaderIndex += direction;
        if (currentShaderIndex < 0)
        {
            currentShaderIndex = shaderMappings.Count - 1;
        }
        else if (currentShaderIndex >= shaderMappings.Count)
        {
            currentShaderIndex = 0;
        }

        if (isShaderApplied)
        {
            foreach (var brush in FindObjectsOfType<DynamicBrushDrawer>())
            {
                brush.ApplyMaterial(shaderMappings[currentShaderIndex].material);
            }
        }
    }

    public Material GetCurrentMaterial()
    {
        return shaderMappings.Count > 0 ? shaderMappings[currentShaderIndex].material : null;
    }

    public bool IsShaderApplied()
    {
        return isShaderApplied;
    }
}
