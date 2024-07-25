using UnityEngine;

public class ColorPickerState : MonoBehaviour
{
    
    [SerializeField] private GameObject colorPicker;
    [SerializeField] private BaseInputManager inputManager;
    
    void Start()
    {
        inputManager.TurnOnColorPicker += () =>
        {
            colorPicker.SetActive(true);
        };
        
        inputManager.TurnOffColorPicker += () =>
        {
            colorPicker.SetActive(false);
        };
    }
}
