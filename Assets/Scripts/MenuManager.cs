using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public PlaceElement placeElementManager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private GameObject FindElement(string elementName)
    {
        Object[] objects = Resources.LoadAll("Models/Furniture/", typeof(GameObject));

        foreach (Object o in objects)
        {
            if (o.name == elementName)
            {
                return (GameObject) o;
            }
        }

        return null;
    }

    public void MenuItemSelected(string buttonName)
    {
        Debug.Log("Item selected: "+ buttonName);

        GameObject elementToPlaceInScenario = FindElement(buttonName);

        if (elementToPlaceInScenario != null)
        {
            placeElementManager.SetElementToBePlaced(elementToPlaceInScenario);
        }
        else
        {
            Debug.Log("Item to be places not found in the assets folder: '"+ buttonName +"'");
        }
    }
}
