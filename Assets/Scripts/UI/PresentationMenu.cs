using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class PresentationMenu : MonoBehaviour
   {
      [SerializeField] private BaseInputManager inputManager;
      
      [SerializeField] private GameObject toolbar;
      [SerializeField] private GameObject effectsMenu;
      
      [SerializeField] private Toggle effectsMenuToggle;
      [SerializeField] private Button modeButton;
      [SerializeField] private Button homeButton;
      [SerializeField] private Button helpButton;
      
      private bool ToolbarActive
      {
         set => toolbar.SetActive(value);
      }
      
      private bool EffectsMenuActive
      {
         set => effectsMenu.SetActive(value);
      }

      private void Start()
      {
         ToolbarActive = true;
         effectsMenuToggle.onValueChanged.AddListener(OnEffectsMenuToggleValueChanged);
         modeButton.onClick.AddListener(OnModeButtonClicked);
         homeButton.onClick.AddListener(OnHomeButtonClicked);
         helpButton.onClick.AddListener(OnHelpButtonClicked);
      }
      
      private void OnEffectsMenuToggleValueChanged(bool value)
      {
         EffectsMenuActive = value;
      }
      
      private void OnHomeButtonClicked()
      {
         inputManager.CurrentMode = EMode.MAIN_MENU;
      }
      
      private void OnHelpButtonClicked()
      {
         Debug.Log("Help Button Clicked, not implemented yet.");
      }
      
      private void OnModeButtonClicked()
      {
         inputManager.CurrentMode = EMode.CREATE;
      }
   }
}
