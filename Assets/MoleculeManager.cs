using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    foreach (GameObject mol in GameObject.FindGameObjectsWithTag("Molecule"))
        {
            mol.GetComponent<Molecule>().Init(new List<int> { 1 });
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
