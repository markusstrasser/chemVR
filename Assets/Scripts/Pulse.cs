using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {
    public Rigidbody rb;
    Vector3 direction;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void OnTriggerStay(Collider other)
    {
        direction = (transform.position - other.transform.position).normalized;
        rb.MovePosition(transform.position +  direction * 3f);
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
