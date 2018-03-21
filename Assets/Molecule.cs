using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swag;
using TMPro;
using AtomConfig;

[SerializeField]
public class Molecule : MonoBehaviour {
    public GameObject atom;
    SwagUtils swagger;
    public Bonds bonds;

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
        }
        Debug.Log("BONDS:" + bonds.data.ContainsKey(bond));
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

    public void updateTextMesh()
    {

    }

    string Naming(List <Atom> components)  
    {
        return ".reduce";
    }
	// Update is called once per frame
	void Update () {
		
	}
}
