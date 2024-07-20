using System;
using UnityEngine;

namespace UI
{
    public class UiMode : MonoBehaviour
    {
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private GameObject MainMenu;
        [SerializeField] private GameObject CreationMenu;
        [SerializeField] private GameObject PresentationMenu;

        private void Update()
        {
            switch (inputManager.CurrentMode)
            {
                case EMode.MAIN_MENU:
                    MainMenu.SetActive(true);
                    CreationMenu.SetActive(false);
                    PresentationMenu.SetActive(false);
                    break;
                case EMode.CREATE:
                    MainMenu.SetActive(false);
                    CreationMenu.SetActive(true);
                    PresentationMenu.SetActive(false);
                    break;
                case EMode.PRESENT:
                    MainMenu.SetActive(false);
                    CreationMenu.SetActive(false);
                    PresentationMenu.SetActive(true);
                    break;
                case EMode.TUTORIAL:
                    MainMenu.SetActive(false);
                    CreationMenu.SetActive(true);
                    PresentationMenu.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}