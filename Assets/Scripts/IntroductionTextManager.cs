using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionTextManager : MonoBehaviour
{
    public GameObject[] textFields;  // Array to hold all text fields
    public Button nextButton;        // Reference to the Next button
    private int currentIndex = 0;    // Index to keep track of the current active text field

    void Start()
    {
        // Ensure the first text field is active and the rest are inactive
        for (int i = 0; i < textFields.Length; i++)
        {
            textFields[i].SetActive(i == currentIndex);
        }

        // Add listener to the Next button
        nextButton.onClick.AddListener(ActivateNextTextField);
    }

    void ActivateNextTextField()
    {
        // Deactivate the current text field
        textFields[currentIndex].SetActive(false);

        // Increment the index and wrap around if necessary
        currentIndex = (currentIndex + 1) % textFields.Length;

        // Activate the next text field
        textFields[currentIndex].SetActive(true);
    }

    /*
    private void Update()
    {
        // Check for mouse click on a 3D button
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("NextButton"))
                {
                    ActivateNextTextField();
                }
            }
        }
    }*/
}

