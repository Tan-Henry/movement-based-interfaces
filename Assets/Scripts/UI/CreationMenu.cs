using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreationMenu : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private GameObject toolbar;
        [SerializeField] private GameObject colorMenu;
        [SerializeField] private GameObject brushMenu;
        
        [SerializeField] private Toggle brushMenuToggle;
        [SerializeField] private Toggle colorMenuToggle;
        [SerializeField] private Button modeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Toggle eraserToggle;
        [SerializeField] private Button undoToggle;
        [SerializeField] private Button redoToggle;

        private void Start()
        {
            ToolbarActive = true;
            brushMenuToggle.onValueChanged.AddListener(OnBrushMenuToggleValueChanged);
            colorMenuToggle.onValueChanged.AddListener(OnColorMenuToggleValueChanged);
            modeButton.onClick.AddListener(OnModeButtonClicked);
            homeButton.onClick.AddListener(OnHomeButtonClicked);
            helpButton.onClick.AddListener(OnHelpButtonClicked);
            eraserToggle.onValueChanged.AddListener(OnEraserToggleValueChanged);
            undoToggle.onClick.AddListener(OnUndoToggleClicked);
            redoToggle.onClick.AddListener(OnRedoToggleClicked);
        }

        private void Update()
        {
            if (inputManager.IsDrawingState == eraserToggle.isOn)
            {
                eraserToggle.SetIsOnWithoutNotify(!inputManager.IsDrawingState);
            }
        }

        private bool ToolbarActive
        {
            set => toolbar.SetActive(value);
        }

        private bool BrushMenuActive
        {
            set => brushMenu.SetActive(value);
        }

        private bool ColorMenuActive
        {
            set => colorMenu.SetActive(value);
        }
        
        private void OnBrushMenuToggleValueChanged(bool value)
        {
           BrushMenuActive = value;
        }
        
        private void OnColorMenuToggleValueChanged(bool value)
        {
            ColorMenuActive = value;
        }
        
        private void OnModeButtonClicked()
        {
            inputManager.CurrentMode = EMode.PRESENT;
        }
        
        private void OnHomeButtonClicked()
        {
            inputManager.CurrentMode = EMode.MAIN_MENU;
        }
        
        private void OnHelpButtonClicked()
        {
           Debug.Log("Help Button Clicked, not implemented yet.");
        }
        
        private void OnEraserToggleValueChanged(bool value)
        {
            inputManager.OnToggleBrushEraser();
        }
        
        private void OnUndoToggleClicked()
        {
            inputManager.OnUndo();
        }
        
        private void OnRedoToggleClicked()
        {
            inputManager.OnRedo();
        }
    }
}
