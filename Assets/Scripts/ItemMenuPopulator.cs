using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuPopulator : MonoBehaviour {

	public GameObject itemPrefabModel;

	// Use this for initialization
	void Start () {
		GameObject newElement = GameObject.Instantiate(itemPrefabModel);
		newElement.transform.SetParent (transform, false);

		GameObject newElement2 = GameObject.Instantiate(itemPrefabModel);
		newElement2.transform.SetParent (transform, false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
