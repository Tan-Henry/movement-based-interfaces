using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionTextManager : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    [SerializeField] private GameObject tutorialCanvas;
    public GameObject[] textFields;
    public Button nextButton;
    public Button getStartedButton;
    private int currentIndex = 0;

    void Update()
    {
        if (inputManager.CurrentMode == EMode.CREATE && inputManager.IsTutorialMode)
        { tutorialCanvas.gameObject.SetActive(true); }
        else
        { tutorialCanvas.gameObject.SetActive(false); }
    }
    
    void Start()
    {
        // Ensure all text fields are initially inactive
        foreach (GameObject textField in textFields)
        {
            textField.SetActive(false);
        }

        // Set the first text field active
        if (textFields.Length > 0)
        {
            textFields[0].SetActive(true);
        }

        // Ensure GetStartedButton is initially inactive
        getStartedButton.gameObject.SetActive(false);

        // Ensure the tutorialCanvas is initially inactive
        tutorialCanvas.gameObject.SetActive(false);

        // Add listeners to nextButton
        nextButton.onClick.AddListener(ActivateNextTextField);
        getStartedButton.onClick.AddListener(TurnOffTutorial);
    }

    
    void ActivateNextTextField()
    {
        // if (currentIndex != 0)
        // {
        textFields[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % textFields.Length;
        textFields[currentIndex].SetActive(true);
        // Turn on GetStartedButton on last Introduction Slide
        if (currentIndex == textFields.Length - 1)
        {
            nextButton.GameObject().SetActive(false);
            getStartedButton.GameObject().SetActive(true);
        }
        // }
    }

    void TurnOffTutorial()
    {
        inputManager.IsTutorialMode = false;
    }

    /*void OnButtonClicked()
    {
        inputManager.IsTutorialMode = true;
        currentIndex = 0;
    }*/
}