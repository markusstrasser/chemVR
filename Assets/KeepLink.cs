using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class KeepLink : MonoBehaviour {
    public LineRenderer line;
    int segments = 100;
   public Transform myTo;
    public Transform myFrom;
	// Use this for initialization
	void Start () {
	}
    public void Generate(Transform from, Transform to)
    {
        myTo = to;
        myFrom = from;
        CreateLine(from, to);

    }
    public void Cancel()
    {
        Destroy(gameObject);
    }

    public void CreateLine(Transform a, Transform b)
    {
        //some messed up tinkering here
        Debug.Log(line);
        Debug.Log("line");

        line.SetVertexCount(segments);
        line.useWorldSpace = true;
        Debug.Log(line);


        Vector3 deltaVec = a.position - b.position;

        Vector3 step = deltaVec / segments;

        for (int i = 0; i < segments; i++)
        {
            line.SetPosition(i, a.position - (step * i));
        }
    }

    // Update is called once per frame
    void Update() {

        CreateLine(myFrom, myTo);
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //find electron
            GameObject[] eles = GameObject.FindGameObjectsWithTag("Electron");
            GameObject closest = eles.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
                           .FirstOrDefault();   //or use .FirstOrDefault();  if you need just toArray(

              //(GameObject.FindGameObjectWithTag("RayLocation").transform)
            //link to player (this)
        }

    }
}
