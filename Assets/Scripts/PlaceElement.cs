using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaceElement : MonoBehaviour
{
    public VRTK_ControllerEvents.ButtonAlias activationButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    public VRTK_CustomRaycast customRaycast;

    private GameObject grabbedObject = null;
    private float maximumPlacementLength = 500f;
    private float objectRotation = 0f;
    private VRTK_Pointer pointer;
    private VRTK_ControllerEvents controllerEvents;

    void Start()
    {
        // getting pointer as we need it to use for reference for the raycast (to know where it's forward on the controller)
        pointer = GetComponent<VRTK_Pointer>();
        controllerEvents = GetComponentInParent<VRTK_ControllerEvents>();

        // object set is happening on MenuManager
        controllerEvents.SubscribeToButtonAliasEvent(activationButton, true, DoActivationButtonPressed);
        EventManager.StartListening(EventManager.Event.INTERACTABLE_MENU_GRAB_PRESSED, GrabElementEvent);
    }

    protected virtual void DoActivationButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (grabbedObject != null)
        {
            EnableCollisions(grabbedObject, true);
        }

        grabbedObject = null;
    }

    protected void GrabElementEvent(object sender)
    {
        var element = ((IGrabElementEvent)sender).GetElement();
        SetElementToBePlaced(element);
    }

    public void SetElementToBePlaced(GameObject gameObject)
    {
        grabbedObject = GameObject.Instantiate(gameObject);
        EnableCollisions(grabbedObject, false);
    }

    private void EnableCollisions(GameObject obj, bool enabled)
    {
        // Disabling collider so the object does keep zooming on the wand
        foreach (Collider col in obj.GetComponentsInChildren<Collider>())
        {
            col.enabled = enabled;
        }
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
                // Hit UI
                if (pointerCollidedWith.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    grabbedObject.transform.position = pointer.customOrigin.position + (pointer.customOrigin.forward.normalized / 3);
                    grabbedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                // Hit placeable location
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
