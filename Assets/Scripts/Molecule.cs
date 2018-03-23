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
    SwagUtils swagger;
    public Bonds bonds;
    public List<Atom> Atom_1 = new List<Atom>();
    public List<Atom> Atom_2 = new List<Atom>();
    public List<int> strengthsEdit = new List<int> { 0 };

	// Use this for initialization
	void Start () {
        bonds = new AtomConfig.Bonds();
        //Init(new List<int>() { 1 });
        //--> done by Molecule Manager
        swagger = GameObject.Find("Swag").GetComponent<SwagUtils>();
    }

    void changeVisuals ()
    {
        List<Vector3> pos = new List<Vector3>();
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {

            if (transform.GetComponent<Atom>())
            {
                pos.Add(child.position);
            }
            children.Add(child);

        }

        //reduce
        Vector3 center = pos.Aggregate(Vector3.zero, (acc, p) => acc + p);
        Debug.Log("Center" + center + "fl " +  (float)pos.Count);
        Debug.Log((new Vector3(0, 0, 0) == Vector3.zero) + "vectorzero");
        //center = center / (float)pos.Count;
        Debug.Log("Center _after " + center);
 

        //unchild
        foreach(Transform child in children)
        {
            child.parent = null;
        }
        transform.position = center;
        foreach (Transform child in children)
        {
            child.parent = transform;
        }

        transform.FindChild("Pulse").localScale *= 2;


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
            changeVisuals();

            //mergeElements

            GameObject.Find("MoleculeManager").GetComponent<MoleculeManager>().mergeElements(A.transform.parent, B.transform.parent);
            changeText();

            swagger.DrawAllBonds(bonds.data);
        }
    }

    public void Init(List<int> atomnNumbers)
    {
        foreach (int num in atomnNumbers)
        {
            GameObject at = Instantiate<GameObject>(atom);
            at.transform.parent = transform;
            at.transform.localPosition = Vector3.zero;

            at.GetComponent<Atom>().Init(num);
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


void changeText()
    {
        Dictionary<string, int> composition = SwagUtils.elementComposition(transform);
        string molText = SwagUtils.MoleculeTextPlus(composition);
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
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Atom>())
            {
                child.GetComponent<Atom>().ResetState();
            }
        }

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
