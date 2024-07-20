using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        
        [SerializeField] private BaseInputManager inputManager;
        
        [SerializeField] private Button newButton;
        [SerializeField] private Button loadButton;
        
        // Start is called before the first frame update
        void Start()
        {
            newButton.onClick.AddListener(OnNewButtonClicked);
            loadButton.onClick.AddListener(OnLoadButtonClicked);
        }

        private void OnNewButtonClicked()
        {
            inputManager.CurrentMode = EMode.TUTORIAL;
        }
        
        private void OnLoadButtonClicked()
        {
            Debug.Log("Load Button Clicked, not implemented yet.");   
        }
    }
}
