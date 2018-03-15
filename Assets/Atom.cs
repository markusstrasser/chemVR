using System.Collections.Generic;
using System;
using UnityEngine;
using AtomConfig;
using Swag;
using VRTK;

[System.Serializable] //show Lists in inspector
public class Atom : MonoBehaviour
{
    public Config state;
    public Electron electron;
    public List<Transform> to = new List<Transform>();
    public Bond bond;
    public SwagUtils swagger;
    GameObject myParent;
    public bool grabbed = false;
    public float maxDistance = 0.5f;
    // public LineRenderer lRender;



    public void ObjectGrabbed(object sender, InteractableObjectEventArgs e)

    {
        grabbed = true;
        Debug.Log("Im Grabbed   " + gameObject);

    }
    public void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        grabbed = false;
        Transform other = closestToMe();
        if (isLegitBond(other))
        {
            mergeElements(other);
            to.Add(other);
            Debug.Log(transform.name + "  bonded with  " + other.name);
            swagger.CreateLine(transform, other);
        }

        if (other == null)
        {
            Debug.Log("NOTHING TO BOND ----> >>> ");
        }
    }

    bool isLegitBond(Transform B)
    {
        return B != null && !isSame(transform, B) && CanConnect(B.GetComponent<Atom>());
    }

    // Use this for initialization
    void Start()
    {
        //get data from chemical table
        Init(1);
        swagger = GameObject.Find("Swag").GetComponent<SwagUtils>();


        for (int i = 0; i < state.capacity; i++)
        {
            Electron ele = Instantiate<Electron>(electron);
            ele.transform.parent = transform;
        }


       
        
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("Team3_Interactable_Object_Extension is required to be attached to an Object that has the VRTK_InteractableObject script attached to it");

            return;
        }
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUnGrabbed);

    }

    public void Init(int atomNumber)
    {
        state = AtomConfig.PTable.config[atomNumber];
    }

    bool isSame(Transform t1, Transform t2)
    {
        return (t1.GetInstanceID() == t2.GetInstanceID());
    }

    bool isBonded()
    {
        return transform.parent.childCount <= 1;
    }

    void mergeElements(Transform B)
    {
        if (!isSame(transform, B) && CanConnect(B.GetComponent<Atom>()))
        {
            foreach (Transform child in B.parent)
            {
                child.parent = transform.parent;
            }
        }
            //give me all B's children
       
        //destroy B. Now we're one!
        Destroy(B.parent);
    }

    bool isFull()
    {
        return (state.valence == state.capacity) || state.valence == 0;
    }
    bool CanConnect(Atom B)
    {
        if (B == null) { return false; };
        return !B.isFull() && !isFull();
    }

    Transform closestToMe()
    {
        Transform closest = null;
        float closestDist = maxDistance;
        foreach (GameObject other in GameObject.FindGameObjectsWithTag("Atom"))
        {
            
            //loop through siblings

            float dist = Vector3.Distance(transform.position, other.transform.position);
           
            if (dist < closestDist && !isSame(transform, other.transform) && other.GetComponent<Atom>())
            {
                closest = other.transform;
                closestDist = dist;
            }
        }
        Debug.Log(closestDist);
        return closest;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (grabbed && GetComponentInParent<Molecule>().size() <= 1)
            //only you as Atom? molecule == you. But if you're in a cluster only move yourself on dragging

            //***for later molecule connects ==> mergeAnimation and molecule space collapsing function functions
        {
            transform.parent.position = transform.position;
        }
    }
}
