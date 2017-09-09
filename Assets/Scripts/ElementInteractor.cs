using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_Pointer)), RequireComponent(typeof(VRTK_ControllerEvents))]
public class ElementInteractor : MonoBehaviour
{
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;

    private VRTK_Pointer pointer;
    private VRTK_ControllerEvents controllerEvents;

    void Start()
    {
        pointer = GetComponent<VRTK_Pointer>();
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        // only continue if hand controllers are initialized
        if (!pointer.customOrigin) { return; }

        // positioning
        Ray rayCast = new Ray(pointer.customOrigin.position, pointer.customOrigin.forward);

        RaycastHit hitResult;
        float maxDistance = 1000f;
        LayerMask filter = LayerMask.GetMask("Interactable", "UI");

        // setting active model to the interactable manager
        // TODO remove this reference and make each component talk to each other using the EventManager
        InteractableMenu interactableMenu = GameObject.FindObjectOfType<InteractableMenu>();
        
        if (interactableMenu == null)
        {
            print("InteractableMenu doesn't exists. Please add one.");
            return;
        }

        if (Physics.Raycast(rayCast, out hitResult, maxDistance, filter))
        {
            if (validObject(hitResult.transform.gameObject))
            {
                // position interactMenu between player hand and hit object
                Vector3 directionVector = (hitResult.point - pointer.customOrigin.position);

                interactableMenu.transform.position = hitResult.point - (directionVector.normalized / 2);
                interactableMenu.transform.LookAt(Camera.main.transform.position);
                interactableMenu.transform.Rotate(new Vector3(0f, 90f, 0f));
                interactableMenu.SetCurrentElement(hitResult.transform.gameObject);
            }
        }
        else
        {
            interactableMenu.transform.position = new Vector3(0, -10, 0);
            interactableMenu.SetCurrentElement(null);
        }
    }

    private bool validObject(GameObject gameObject)
    {
        return LayerMask.LayerToName(gameObject.layer) == "Interactable";
    }
}
