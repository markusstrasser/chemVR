using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron : MonoBehaviour {

    float speed = 100f;
    public float scale = 0.2f;
    [Range(0,2)]
    public float orbitRad = 1f;
	// Use this for initialization
	void Start () {
        //gameObject.tag = "Electron";
        float partial = (1 + transform.GetSiblingIndex())/transform.parent.childCount;
        transform.localScale = new Vector3(scale, scale, scale);
        Arrange(transform.parent.GetComponent<Atom>().config.valenceOrbit, orbitRad, partial);
       
    }

   

    // Update is called once per frame
    void Update () {
        transform.RotateAround(transform.parent.transform.position, Vector3.forward, speed * Time.deltaTime);
	}

    public void Arrange(int orbitNr, float orbitRadius, float partial)
    {
        float rad = Mathf.PI * 2 * partial;
        Debug.Log(rad);
        transform.localPosition = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0) * orbitRadius;

    }
}
