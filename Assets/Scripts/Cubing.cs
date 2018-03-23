using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Cubing : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //subscribe to the event.  NOTE: the "ObectGrabbed"  this is the procedure to invoke if this objectis grabbed.. 

        if (GetComponent<VRTK_InteractableObject>() == null)

        {

            Debug.LogError("Team3_Interactable_Object_Extension is required to be attached to an Object that has the VRTK_InteractableObject script attached to it");

            return;

        }
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
    }

    public void ObjectGrabbed(object sender, InteractableObjectEventArgs e)

    {

        Debug.Log("Im Grabbed"  + gameObject);

    }

    // Update is called once per frame
    void Update () {
       bool f =  GetComponent<VRTK_InteractableObject>().IsGrabbed();
        Debug.Log(f);
    }
}

