using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swag;
using TMPro;
using AtomConfig;
using seeDict;
using UnityEditor;
using System.Linq;

[System.Serializable] //show Lists in inspector

public class Molecule : MonoBehaviour {
    public GameObject atom;
    public int charge = 0;
    SwagUtils swagger;
    public Bonds bonds;
    public List<Atom> Atom_1 = new List<Atom>();
    public List<Atom> Atom_2 = new List<Atom>();
    public List<int> strengthsEdit = new List<int> { 0 };
    public float counter = 0;
	// Use this for initialization
	void Start () {
        bonds = new AtomConfig.Bonds();
        //Init(new List<int>() { 1 });
        //--> done by Molecule Manager
        swagger = GameObject.Find("Swag").GetComponent<SwagUtils>();
    }

    private void Update()
    {
        counter += 0.05f;
        float change = Mathf.Sin(counter) * Time.deltaTime * (1 - 0.15f * transform.childCount);
            transform.position += new Vector3(0, change, 0);
    }



    void changeVisuals ()
    {
        List<Vector3> pos = new List<Vector3>();
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {

            if (child.GetComponent<Atom>() != null)
            {
                pos.Add(child.position);
            }
            children.Add(child);

        }

        //reduce
        Vector3 center = pos.Aggregate(Vector3.zero, (acc, p) => acc + p);
        Debug.Log("Center" + center + "fl " +  (float)pos.Count);
        center = center / (float)pos.Count;
        Debug.Log("Center _after " + center);


        //unchild
        foreach (Transform child in children)
        {
            child.parent = null;
        }
        transform.position = center;
        foreach (Transform child in children)
        {
            child.parent = transform;
        }

        Transform pulse = transform.FindChild("Pulse").transform;
        pulse.localPosition = Vector3.zero;
        pulse.localScale = Vector3.one * pos.Count / 1.5f;



    }
    public void AddPair(Atom A, Atom B)
    {
        KeyValuePair<Atom, Atom> bond = new KeyValuePair<Atom, Atom>(A, B);
        if (bonds.isValidBond(A, B))
        { 
            Debug.Log("VALID BONDING!! " + A + "asdsaa" + B);

            bonds.AddBond(bond);
            updateBonds();
            makeVisibleInEditor(bonds.data);
            //mergeElements

            GameObject.Find("MoleculeManager").GetComponent<MoleculeManager>().mergeElements(A.transform.parent, B.transform.parent);


            changeText();
            changeVisuals();

            swagger.DrawAllBonds(bonds.data);
        }
    }
    int calcCharge()
    {
        int ch = 0;
        foreach (Transform child in transform)
        {
            Atom a = child.GetComponent<Atom>();
            if (a!= null)
            {
                //this makes 
                ch += (a.config.capacity - a.config.valence - a.shared);
            }
        }
        return ch;

    }

    public void Init(List<int> atomnNumbers)
    {
        foreach (int num in atomnNumbers)
        {
            GameObject at = Instantiate<GameObject>(atom);
            at.GetComponent<Atom>().Init(num);

            at.transform.parent = transform;
            at.transform.localPosition = Vector3.zero;


        }
        StartCoroutine(WaitForAtomForSomeReason());
    } 

    void Later(){
        changeText();
    }
IEnumerator WaitForAtomForSomeReason()
{
    yield return new WaitForSeconds(1f);
    Later();
}

int countAtomChildren()
    {
        int c = 0;
        foreach (Transform child in transform)
        {
            Atom a = child.GetComponent<Atom>();
            if (a != null)
            {
                c++;
            }
        }
        return c;
    }

void changeText()
    {
        int ch = calcCharge();
        if (countAtomChildren() == 1)
        {
            //don't display on single atoms
            ch= 0;
        }

        Dictionary<string, int> composition = SwagUtils.elementComposition(transform);
        string molText = SwagUtils.MoleculeTextPlus(composition, ch);
        if (molText== null)
        {
            molText = "X";
        }

        transform.GetComponentInChildren<TextMeshManager>().DisplayText(molText);
    }

    void makeVisibleInEditor(Dictionary<KeyValuePair<Atom,Atom>, int> bondData)
    {
        Atom_1.Clear();
        Atom_2.Clear();
        strengthsEdit.Clear();
        foreach (KeyValuePair<Atom, Atom> kvp in bonds.data.Keys)
        {
            Atom_1.Add(kvp.Key);
        }
        foreach (KeyValuePair<Atom, Atom> kvp in bonds.data.Keys)
        {
            Atom_2.Add(kvp.Value);
        }
        foreach (int strength in bonds.data.Values)
        {
            strengthsEdit.Add(strength);
        }
    }

    void updateBonds()
    {
        //reset all Atoms' electron state
       

        //TODO !!! assume the most likely new molecule without the Atom who left H2O => HO ? 
        //OR ! ==> disperse as a whole into single atoms again


        //recalc electron trading and Bondtype
        foreach (KeyValuePair<Atom, Atom> kvp in bonds.data.Keys) {
            string bondType = bonds.BondType(kvp.Key, kvp.Value);
            for (int i = 0; i < bonds.data[kvp]; i++)
                //if there is multiple bonds
            {
                bonds.ElectronTrading(kvp.Key, kvp.Value, bondType);
            }
        }
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Atom>())
            {
                child.GetComponent<Atom>().updateElectrons();
            }
        }
    }
    
    public int size () //in JS that's 1 line...
    {
        int counter = 0;
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Atom>())
            {
                counter++;
            }
        }
        return counter;
    }

}
