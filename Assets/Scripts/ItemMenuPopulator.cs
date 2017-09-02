using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenuPopulator : MonoBehaviour
{
    public GameObject buttonReference;
    public MenuManager menuManager;

    private GameObject[] items;
    private GameObject[] buttons;

    // Use this for initialization
    void Start()
    {
        // get all furnitures to populate
        Object[] objects = Resources.LoadAll("Models/Furniture/", typeof(GameObject));
        items = new GameObject[objects.Length];

        for (int index = 0; index < objects.Length; ++index)
        {
            GameObject item = (GameObject)objects[index];
            items.SetValue(item, index);
        }

        // populate the UI with all the furnitures

        // delete all children
        // starting it with a clean slate
        for(int index = 0; index < transform.childCount; ++index)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }

        // populating the panel with the buttons
        buttons = new GameObject[items.Length];

        for (int index = 0; index < items.Length; ++index)
        {
            GameObject newButton = GameObject.Instantiate(buttonReference);
            newButton.transform.SetParent(transform, false);
            newButton.name = items[index].name;
            newButton.SetActive(true);

            buttons[index] = newButton;

            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(delegate {
                Debug.Log("Menu item clicked: " + newButton.name);
                menuManager.MenuItemSelected(newButton.name);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
