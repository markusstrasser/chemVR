using System.Collections.Generic;
using System;
using UnityEngine;
using AtomConfig;
using Swag;
using VRTK;
using System.Linq;

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
    List<Transform> removeDuplicates(List<Transform> trans)
    {
        List<int> ids = new List<int> { };
        List<Transform> uniques = new List<Transform> { };
        foreach(Transform t in trans)
        {
            if (!ids.Contains(t.GetInstanceID()))
            {
                uniques.Add(t);
                ids.Add(t.GetInstanceID());
            }
        }
        return uniques;
    }



    public void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        grabbed = false;
        Transform other = closestToMe();
        if (isLegitBond(other))
        {
            mergeElements(other);
            //to.Add(other);
            //List<Transform> unique = removeDuplicates(to);
          
            Debug.Log("TOOOO" +  to + "  to  " + to.Count);

            Debug.Log(transform.name + "  bonded with  " + other.name);
          //  swagger.DrawBonds(transform, to);
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

    void mergeElements(Transform B)
    {
        Debug.Log("atom " + B);
        Debug.Log("childcount " +  B.parent.childCount);
        if (!isSame(transform, B) && CanConnect(B.GetComponent<Atom>()))
        {
            int len = B.parent.childCount;
            if (len <= 0)
            {
                return;
            }
            for (int i = 0; i < len; i++)
            {
                //GameObject c = Instantiate<GameObject>(B.parent.GetChild(i).gameObject);
                B.parent.GetChild(i).parent = transform.parent;

            }
            //**** ---- BUG: Atom doesn't move ...before Molecule dies....Pulse does...)
            B.parent.gameObject.SetActive(false);
            //Destroy(B.parent.gameObject);
        }
        //give me all B's children

        //destroy B. Now we're one!
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
