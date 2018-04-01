using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshManager : MonoBehaviour
{
    public Transform target;

    TextMeshPro textMeshPro;
    // Use this for initialization
    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
        textMeshPro.text = "<u>9x<sup><#00ff00>3</color></sup></u>";
    }

    public void DisplayText(string text)
    {
        target = GameObject.FindGameObjectWithTag("POW").transform;
    
        textMeshPro.text = text;
        transform.localPosition = new Vector3(0, 0, 0);
    }
    private void Update()
    {


        if (target != null)
        {

            // transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.LookAt(target);
            transform.Rotate(0, 180, 0);


        }
    }
}

    