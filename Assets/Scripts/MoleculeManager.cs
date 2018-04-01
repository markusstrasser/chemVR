using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour {
    private List<int> atomNumbers = new List<int>() { 1, 1, 8, 5, 6, 7, 1, 8};
    public Molecule molecule;
	// Use this for initialization
	void Start () {
        foreach (GameObject mol in GameObject.FindGameObjectsWithTag("Molecule"))
        {
            mol.GetComponent<Molecule>().Init(new List<int> { 1 });
        }
        foreach (int entry in atomNumbers)
        {
            Vector3 center = new Vector3(0, 1.5f, 0);
            Vector3 pos = RandomCircle(center, 5.0f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

            Molecule MOL = Instantiate(molecule, pos, rot);

            //MOL.transform.position = new Vector3(Random.Range(0f, 4f), Random.Range(0.3f, 1.5f), Random.Range(0f, 4f));
            MOL.Init(new List<int>() { entry });
            Debug.Log("mol " + entry);
        }
	}

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
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

        foreach (GameObject mol in GameObject.FindGameObjectsWithTag("Molecule"))
        {
            if (mol.GetComponentInChildren<Atom>() == null)
            {
                Destroy(mol);
            }
        }
        A.GetComponent<Molecule>().changeText();
        A.GetComponent<Molecule>().changeVisuals();


    }

    // Update is called once per frame
    void Update () {
		
	}
}
