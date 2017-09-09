using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerEventBroadcast : MonoBehaviour {
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    
    private VRTK_ControllerEvents controllerEvents;

    void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        controllerEvents.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        // TODO send event
    }

    // Update is called once per frame
    void Update () {
		
	}
}
