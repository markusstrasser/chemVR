using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLine : MonoBehaviour {

    public int segments = 200;
    public float xradius;
    public float yradius;
    LineRenderer line;
    int electrons = 0;
    int orbitCapacity = 2;
    Color initialColor;
    bool increaseAlpha = true;
    public bool OrbitComplete = false;

    void Start()
    {
        initialColor = transform.GetComponent<LineRenderer>().material.GetColor("_TintColor");
    }
    private void Update()
    {
       if ( electrons / orbitCapacity != 1)
        {
            Color temp = transform.GetComponent<LineRenderer>().material.GetColor("_TintColor");
            if (temp.a > 0.2f)
            {
                increaseAlpha = false;
            }
            if (temp.a < 0.005f)
            {
                increaseAlpha = true;
            }

            if (increaseAlpha)
            {
                temp.a += 0.005f;
            }
            else
            {
                temp.a -= 0.005f;
            }
            transform.GetComponent<LineRenderer>().material.SetColor("_TintColor", temp);

            if (electrons >= orbitCapacity)
            {
                OrbitComplete = true;
            }

        }
    }
    public void SetElectronCount(int elec)
    {
        electrons = elec;
    }

    public void AddElectron()
    {
        electrons += 1;
     
    }

    public void SetOrbitCapacity(int capacity)
    {
        orbitCapacity = capacity;
    }


    public void CreatePoints(float rad)
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        Debug.Log(line);

        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * rad;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * rad;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += 360f / segments;
        }
    }
}
