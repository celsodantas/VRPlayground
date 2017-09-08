using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableManager : MonoBehaviour
{
    public Button deleteButton;
    public Button grabButton;

    private GameObject currentElement;

    public void Start()
    {
        DeactivateUI();
        deleteButton.onClick.AddListener(DeleteButtonPressed);
    }

    public void SetCurrentElement(GameObject element)
    {
        currentElement = element;

        if (element != null)
        {
            Invoke("ActivateUI", 1);
        }
        else
        {
            DeactivateUI();
        }
    }

    private void ActivateUI()
    {
        deleteButton.interactable = true;
    }

    private void DeactivateUI()
    {
        deleteButton.interactable = false;
    }

    private void DeleteButtonPressed()
    {
        Destroy(currentElement);
    }
}
