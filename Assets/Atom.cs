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

    public void Bond(Transform other)
    {
        if (!isSame(transform, other) && CanConnect(other.GetComponent<Atom>()))
        {
            to.Add(other);
            Debug.Log(transform.name + "  bonded with  " + other.name);

            swagger.CreateLine(transform, other);
            mergeElements(other);
        }
        else
        {
            //no bonding
            Debug.LogError("Tried to bond two Atoms wrongly");
        }

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
        Destroy(B.gameObject);
    }

    void Bond(Atom B)
    {
        //give all children
        //delete B
        if (!isBonded() && !B.isBonded())
        {
            myParent = new GameObject("Molec");
            myParent.AddComponent<Molecule>();
            transform.parent = myParent.transform;
            B.transform.parent = myParent.transform;
        }
        if (isBonded() && !B.isBonded())
        {
            B.transform.parent = transform.parent;
        }
        if (!isBonded() && B.isBonded())
        {
            transform.parent = B.transform.parent;
        }
        //if both are bonded ... make A ingests Molecule Badsaasdsasdsa
        //molecule loops through kids => if ATom => new Parent is the other molecule ... then Destroy mole
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

    Transform closest()
    {
        return transform;
    }

    Transform closestToMe()
    {
        Transform closest = null;
        float closestDist = maxDistance;
        foreach (Transform child in transform.parent)
        {
            //loop through siblings

            float dist = Vector3.Distance(transform.position, child.position);

            if (dist < maxDistance && dist < closestDist && !isSame(transform, child))
            {
                closest = child;
                closestDist = dist;
            }
        }
        Debug.Log(closestDist);
        return closest;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
