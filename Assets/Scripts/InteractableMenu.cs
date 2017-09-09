using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IGrabElementEvent
{
    GameObject GetElement();
}

public class InteractableMenu : MonoBehaviour, IGrabElementEvent
{
    public Button deleteButton;
    public Button grabButton;

    private GameObject currentElement;

    public void Start()
    {
        DeactivateUI();
        deleteButton.onClick.AddListener(DeleteButtonPressed);
        grabButton.onClick.AddListener(GrabButtonPressed);
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
        grabButton.interactable = true;
    }

    private void DeactivateUI()
    {
        deleteButton.interactable = false;
        grabButton.interactable = false;
    }

    private void DeleteButtonPressed()
    {
        Destroy(currentElement);
        ResetMenu();
    }

    private void GrabButtonPressed()
    {
        EventManager.TriggerEvent(EventManager.Event.INTERACTABLE_MENU_GRAB_PRESSED, this);
        ResetMenu();
    }

    public GameObject GetElement()
    {
        return currentElement;
    }

    public void ResetMenu()
    {
        // the destroy shouldn't happen here. TODO: move it to a listener for better separation of concerns.
        Destroy(currentElement);

        SetCurrentElement(null);
        transform.position = new Vector3(0, -10, 0);
    }
}
