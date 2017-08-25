using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObject : MonoBehaviour {

    public GameObject attach;
    public Vector3 rotationAdjustment;

    private GameObject objectWrapper = new GameObject();

	// Use this for initialization
	void Start () {
        attach.transform.SetParent(transform);
        attach.transform.position = new Vector3();
        attach.transform.rotation = Quaternion.Euler(rotationAdjustment);
    }

    // Update is called once per frame
    void Update () {
        objectWrapper.transform.position = transform.position;
    }
}
