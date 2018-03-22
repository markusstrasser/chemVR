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

    public void mergeElements(Transform A, Transform B)
    {
        Debug.Log("molec " + B);
        Debug.Log("childcount " + B.childCount);
        int len = B.childCount;
        if (len <= 0)
        {
            return;
        }
        for (int i = 0; i < len; i++)
        {
            //GameObject c = Instantiate<GameObject>(B.parent.GetChild(i).gameObject);
            if (B.GetChild(i).tag == "Atom")
            {
                B.GetChild(i).parent = A;
                //transform.parent.GetChild(0).parent = B.parent;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
