using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_Pointer)), RequireComponent(typeof(VRTK_ControllerEvents))]
public class ElementInteractor : MonoBehaviour {
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;

    private VRTK_Pointer pointer;
    private VRTK_ControllerEvents controllerEvents;

    void Start ()
    {
        pointer = GetComponent<VRTK_Pointer>();
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        // only continue if hand controllers are initialized
        if (!pointer.customOrigin) { return;  }

        // positioning
        Ray rayCast = new Ray(pointer.customOrigin.position, pointer.customOrigin.forward);

        RaycastHit hitResult;
        int layerFilter = LayerMask.GetMask("Interactable");
        float maxDistance = 1000f;

        if (Physics.Raycast(rayCast, out hitResult, maxDistance, layerFilter))
        {
            Debug.Log("coisa");
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

    }
}
