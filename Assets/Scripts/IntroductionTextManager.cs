using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionTextManager : MonoBehaviour
{
    public GameObject[] textFields;
    public Button nextButton;
    public Button getStartedButton;
    private int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < textFields.Length; i++)
        {
            textFields[i].SetActive(i == currentIndex);
        }
        // Add listener to the Next button
        nextButton.onClick.AddListener(ActivateNextTextField);
    }

    void ActivateNextTextField()
    {
        textFields[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % textFields.Length;
        textFields[currentIndex].SetActive(true);
        // Turn on GetStartedButton on last Introduction Slide
        if (currentIndex == textFields.Length - 1)
        {
            nextButton.GameObject().SetActive(false);
            getStartedButton.GameObject().SetActive(true);
        }
    }
}

