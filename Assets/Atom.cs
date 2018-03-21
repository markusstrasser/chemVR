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
    public Config state;
    public Electron electron;
    public List<Transform> to = new List<Transform>();
    public List<Transform> from = new List<Transform>();
    public SwagUtils swagger;
    GameObject myParent;
    public bool grabbed = false;
    public float maxDistance = 0.5f;
    public Molecule mol;
    public Molecule m;
       
    // public LineRenderer lRender;

    //call state => config
    //make state just a backup
    //config.valence vs valence
    //make PTable a Struct
    //use delegates for successful bonding
	//to ==> dictionary .containskey




    public void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        grabbed = true;
        Debug.Log("Im Grabbed   " + gameObject);
        //remove linerenders that point to me and that I have as children
        removeTies(from, gameObject);
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

    void removeTies(List<Transform> ties, GameObject me)
    {
        //loop through links
        //destroy all that link to this atom
        //remove the entry of the atom from the .from list
        int myId = me.GetInstanceID();
        foreach (Transform nodeAtom in ties)
        {
            Debug.Log("HEEEEEELLOO removeTies");
            Atom node = nodeAtom.GetComponent<Atom>();
            foreach (Transform linkTo in node.to)
            {
                bool isMe = linkTo.gameObject.GetInstanceID() == myId;
                Debug.Log(isMe + " isMe", node);
                if (isMe)
                {
                    node.to.Remove(linkTo);
                }
            }
            node.removeLineRender();
            node.tick();
        }
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
        transform.parent = m.transform; //2 hour BUG ---- the Atom without any reason switched back to original parent;
        unGrab();
        grabbed = false;

    }

    public void unGrab()
    {

        KillEmptyMolecules();
        Transform other = closestToMe();
        bonding(other);

        transform.GetComponentInParent<Molecule>().AddPair(this, other.GetComponent<Atom>());

       
       if (other == null)
        {
            Debug.Log("NOTHING TO BOND ----> >>> ");
            from.Clear();
            to.Clear();
        }
        tick();
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

    void bonding(Transform other)
    {
        if (isLegitBond(other))
        {
            Debug.Log("LEGIT BOND TRUE");
            mergeElements(other);
            to.Add(other);
            other.GetComponent<Atom>().from.Add(transform);
            other.GetComponent<Atom>().tick();
        }
    }

    public void tick()
    {
        to = removeDuplicates(to);
        from = removeDuplicates(from);

        swagger.DrawBonds(transform, to);
      

    }

    bool isLegitBond(Transform B)
    {
        return B != null && !isSame(transform, B) && CanConnect(B.GetComponent<Atom>());
    }

    // Use this for initialization
    void Start()
    {
        //get data from chemical table
        //Init(1);
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

        Dictionary<string,int> comp = SwagUtils.elementComposition(transform.parent.transform);
        string text = SwagUtils.MoleculeText(comp);
        Debug.Log(SwagUtils.elementComposition(transform.parent.transform) + "   Dict");
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
                if (B.parent.GetChild(i).tag == "Atom")
                {
                    B.parent.GetChild(i).parent = transform.parent;
                    //transform.parent.GetChild(0).parent = B.parent;
                }
            }
            
           //transform.parent.GetComponent<Molecule>().name
            //**** ---- BUG: Atom doesn't move ...before Molecule dies....Pulse does...)
           // B.parent.gameObject.SetActive(false);
            //Destroy(B.parent.gameObject);
        }
        //give me all B's children
      
        //destroy B. Now we're one!
    }

    public bool isFull()
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
        foreach (GameObject atom in GameObject.FindGameObjectsWithTag("Atom"))
        {
            
            //loop through siblings

            float dist = Vector3.Distance(transform.position, atom.transform.position);
           
            if (dist < closestDist && !isSame(transform, atom.transform) && atom.GetComponent<Atom>())
            {
                closest = atom.transform;
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
        if (Input.GetKeyDown("space"))
        {
            unGrab();
        }

    }
}
