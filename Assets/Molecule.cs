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
    public void AddPair(Atom A, Atom B)
    {
        KeyValuePair<Atom, Atom> bond = new KeyValuePair<Atom, Atom>(A, B);
        if (bonds.isValidBond(A, B))
        { 
            Debug.Log("VALID BONDING!! " + A + "asdsaa" + B);
            bonds.AddBond(bond);
            updateBonds();
            makeVisibleInEditor(bonds.data);

            Dictionary<string, int> composition = SwagUtils.elementComposition(transform);
            string molText = SwagUtils.MoleculeTextPlus(composition);
            transform.GetComponentInChildren<TextMeshManager>().DisplayText(molText);

            swagger.DrawAllBonds(bonds.data);
        }
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

    public void Init(List<int> atomnNumbers)
    {
        foreach (int num in atomnNumbers)
        {
            GameObject at = Instantiate<GameObject>(atom);
            at.transform.parent = transform;
            at.transform.localPosition = Vector3.zero;

            at.GetComponent<Atom>().Init(num);     
        }
        Debug.Log(size());
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
