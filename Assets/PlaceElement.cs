using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaceElement : MonoBehaviour {


    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    public VRTK_CustomRaycast customRaycast;

    // Use this for initialization
    void Start () {
        if (controller == null)
        {
            controller = GetComponentInParent<VRTK_ControllerEvents>();
        }

        controller.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        Ray rayCast = new Ray(transform.position, transform.forward);
        
        LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;
        float maximumLength = 500f;

        RaycastHit pointerCollidedWith;
        bool rayHit = VRTK_CustomRaycast.Raycast(customRaycast, rayCast, out pointerCollidedWith, layersToIgnore, maximumLength);
     
        if (rayHit)
        {
            Object[] objects = Resources.LoadAll("Models/Furniture/", typeof(GameObject));
            print(objects);

            foreach (var x in objects)
                print(x.name);

            GameObject newObject = GameObject.Instantiate( (GameObject) objects[0]);
            newObject.transform.position = pointerCollidedWith.point;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
