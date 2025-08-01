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
            
            DisableButtons(inputManager.CurrentMode == EMode.TUTORIAL);

            if (inputManager.CurrentMode != EMode.CREATE)
            {
                brushMenuToggle.isOn = false;
                colorMenuToggle.isOn = false;
            }
        }
        
        private void DisableButtons(bool isTutorialMode)
        {
            modeButton.interactable = !isTutorialMode;
            homeButton.interactable = !isTutorialMode;
            brushMenuToggle.interactable = !isTutorialMode;
            helpButton.interactable = !isTutorialMode;
            undoToggle.interactable = !isTutorialMode;
            redoToggle.interactable = !isTutorialMode;
            eraserToggle.interactable = !isTutorialMode;
            colorMenuToggle.interactable = !isTutorialMode;
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
            brushMenuToggle.isOn = false;
            colorMenuToggle.isOn = false;
            inputManager.OnMainMenu();
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
