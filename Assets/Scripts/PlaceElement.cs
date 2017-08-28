using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaceElement : MonoBehaviour
{
    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    public VRTK_CustomRaycast customRaycast;

    private GameObject grabbedObject = null;
    private float maximumPlacementLength = 500f;
    private float objectRotation = 0f;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponentInParent<VRTK_ControllerEvents>();
        }

        // object set is happening on MenuManager
        //controller.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
        //controller.SubscribeToButtonAliasEvent(activationButton, false, DoActivationButtonReleased);
    }

    //protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    //{
    //    grabbedObject = GetSelectedObject();
    //}

    //protected virtual void DoActivationButtonReleased(object sender, ControllerInteractionEventArgs e)
    //{
    //    grabbedObject = null;
    //}
    
    public void SetElementToBePlaced(GameObject gameObject)
    {
        grabbedObject = GameObject.Instantiate(gameObject);
    }

    void FixedUpdate()
    {
        if (grabbedObject != null)
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
