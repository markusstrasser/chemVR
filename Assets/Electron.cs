using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron : MonoBehaviour {

    public int valenzDist = 20;
    public int speed = 60;
    public float scale = 0.2f;
    private Vector3 center = new Vector3(0, 0, 0);
    Transform to;
	// Use this for initialization
	void Start () {
        //gameObject.tag = "Electron";
        transform.localScale = new Vector3(scale, scale, scale);
    }

   

    // Update is called once per frame
    void Update () {
        transform.RotateAround(transform.parent.transform.position, Vector3.forward, 60 * Random.Range(0.5f,3) * Time.deltaTime);
	}

    public void Arrange(int orbitNr, float orbitRadius, float partial)
    {
        float rad = Mathf.PI * 2 * partial;
        Debug.Log(rad);
        transform.localPosition = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0) * orbitRadius * (orbitNr+1);

    }

    public Transform AtomFather()
    {
        return transform.parent.parent;
    }
}
