using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistryCalc : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public string[] AtomShortCuts = new string[]
    {
        "NoProton", "H", "He", "Li", "Be", "B", "C", "N", "O", "F", "Ne"
    };

    public string[] AtomNames = new string[]
    {
        "Nope", "Hydrogen", "Helium", "Lithium", "Berylium", "Boron", "Carbon", "Nitrogen", "Oxygen", "Fluorine", "Neon"
    };

    public string ChargeNess(int protons, int electrons)
    {
        int charge = protons - electrons;
        if (charge > 0)
        {
            return charge.ToString() + "+";
        }
        else if (charge < 0)
        {
            return  charge.ToString() + "-";
        }
        else return "";
    }

	// Update is called once per frame
	void Update () {
		
	}
}
