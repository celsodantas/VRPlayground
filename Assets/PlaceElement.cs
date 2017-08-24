using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaceElement : MonoBehaviour
{

    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    public VRTK_CustomRaycast customRaycast;

    bool isGrabbingElement = false;
    GameObject grabbedObject = null;
    float maximumPlacementLength = 500f;
    float objectRotation = 0f;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponentInParent<VRTK_ControllerEvents>();
        }

        controller.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
        controller.SubscribeToButtonAliasEvent(activationButton, false, DoActivationButtonReleased);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        isGrabbingElement = true;
        grabbedObject = InstantiateAndReturnObject();
    }

    protected virtual void DoActivationButtonReleased(object sender, ControllerInteractionEventArgs e)
    {
        isGrabbingElement = false;
        grabbedObject = null;
    }

    private GameObject InstantiateAndReturnObject()
    {
        Object[] objects = Resources.LoadAll("Models/Furniture/", typeof(GameObject));
        return GameObject.Instantiate((GameObject)objects[0]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrabbingElement)
        {
            // positioning
            Ray rayCast = new Ray(transform.position, transform.forward);

            LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;

            RaycastHit pointerCollidedWith;
            bool rayHit = VRTK_CustomRaycast.Raycast(customRaycast, rayCast, out pointerCollidedWith, layersToIgnore, maximumPlacementLength);

            if (rayHit)
            {
                grabbedObject.transform.position = pointerCollidedWith.point;

                // rotating
                objectRotation += controller.GetTouchpadAxis().x;
                grabbedObject.transform.rotation = Quaternion.Euler(new Vector3(0, objectRotation, 0));
            }
        }
    }
}
