using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaceElement : MonoBehaviour
{
    public VRTK_ControllerEvents controllerEvents;
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    public VRTK_CustomRaycast customRaycast;

    private GameObject grabbedObject = null;
    private float maximumPlacementLength = 500f;
    private float objectRotation = 0f;
    private VRTK_Pointer pointer;

    GameObject sample = null;

    void Start()
    {
        if (controllerEvents == null)
        {
            controllerEvents = GetComponentInParent<VRTK_ControllerEvents>();
        }

        // object set is happening on MenuManager
        controllerEvents.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
        //controller.SubscribeToButtonAliasEvent(activationButton, false, DoActivationButtonReleased);

        // getting pointer as we need it to use for reference for the raycast (to know where it's forward on the controller)
        pointer = GetComponent<VRTK_Pointer>();
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("Trigger pressed");
        grabbedObject = null;
    }

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
            Ray rayCast = new Ray(pointer.customOrigin.position, pointer.customOrigin.forward);

            RaycastHit pointerCollidedWith;
            LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;
            bool rayHit = VRTK_CustomRaycast.Raycast(customRaycast, rayCast, out pointerCollidedWith, layersToIgnore, maximumPlacementLength);

            if (rayHit)
            {
                if (pointerCollidedWith.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    grabbedObject.transform.position = pointer.customOrigin.position + (pointer.customOrigin.forward.normalized/3);
                    grabbedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                else
                {
                    grabbedObject.transform.position = pointerCollidedWith.point;
                    grabbedObject.transform.localScale = Vector3.one;
                }
            }
            else
            {
                grabbedObject.transform.position = pointer.customOrigin.position + (pointer.customOrigin.forward.normalized / 3);
                grabbedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); 
            }

            // rotating
            objectRotation += controllerEvents.GetTouchpadAxis().x;
            grabbedObject.transform.rotation = Quaternion.Euler(new Vector3(0, objectRotation, 0));
        }
    }
}
