using System.Collections.Generic;
using System;
using UnityEngine;
using AtomConfig;
using Swag;
using VRTK;
using System.Linq;
using System.Collections;

[System.Serializable] //show Lists in inspector
public class Atom : MonoBehaviour
{
    public Config config;
    public int shared = 0;
    public int lost = 0;
    public Electron electron;
    public SwagUtils swagger;
    GameObject myParent;
    public float maxDistance = 0.5f;
    public Molecule mol;
    public Molecule m;
    public bool grabbed = false;

    //make PTable a Struct
    //use delegates for successful bonding

    // Use this for initialization
    void Start()
    {
        //get data from chemical table
        //Init(1);
        swagger = GameObject.Find("Swag").GetComponent<SwagUtils>();

        for (int i = 0; i < config.capacity; i++)
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
        config = AtomConfig.PTable.config[atomNumber];
    }


    public void ResetState()
    {
        shared = 0;
        lost = 0;
    }
    public void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        grabbed = true;
        Debug.Log("Im Grabbed   " + gameObject);
        //remove linerenders that point to me and that I have as children
        removeLineRender();

        m = Instantiate<Molecule>(mol);
        m.name = "yar" + UnityEngine.Random.Range(0f, 100f).ToString();
        transform.parent = m.transform;
    }

    public void removeLineRender()
    {
        foreach (Transform child in transform)
        {
			//TODO GETcomponentsInChildren
			if (child.tag == "bond")
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        transform.parent = m.transform; //2 hour BUG ---- the Atom without any reason switched back to original parent;
        unGrab();
        grabbed = false;
    }

    public void unGrab()
    {
        KillEmptyMolecules();
        Transform other = closestToMe();
        transform.GetComponentInParent<Molecule>().AddPair(this, other.GetComponent<Atom>());
    }

    void KillEmptyMolecules ()
    {
        foreach (GameObject mol in GameObject.FindGameObjectsWithTag("Molecule"))
        {
            int count = 0;
            foreach (Transform child in mol.transform)
            {
                if (child.tag == "Atom")
                {
                    count++;
                }
            }
            if (count == 0)
            {
                Destroy(mol);
            }
        }
    }

 
    bool isSameTransform(Transform t1, Transform t2)
    {
        return (t1.GetInstanceID() == t2.GetInstanceID());
    }

    public bool isFull()
    {
        int valence = config.valence + shared - lost;
        if (valence > config.capacity)
        {
            Debug.Log("ahh VALENCE > Config.CApacity...what's going on?!");
        }
        return (valence == config.capacity) || valence == 0;
    }


    Transform closestToMe()
    {
        Transform closest = null;
        float closestDist = maxDistance;
        foreach (GameObject atom in GameObject.FindGameObjectsWithTag("Atom"))
        {
            float dist = Vector3.Distance(transform.position, atom.transform.position);
            if (dist < closestDist && !isSameTransform(transform, atom.transform) && atom.GetComponent<Atom>())
            {
                closest = atom.transform;
                closestDist = dist;
            }
        }
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
        if (Input.GetKeyDown("space"))
        {
            unGrab();
        }

    }
}
