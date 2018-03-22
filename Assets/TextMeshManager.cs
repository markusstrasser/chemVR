using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshManager : MonoBehaviour
{
    public Transform target;

    public TextMeshPro textMeshPro;
    // Use this for initialization
    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
        textMeshPro.text = "<u>9x<sup><#00ff00>3</color></sup></u>";
    }

    public void DisplayText(string text)
    {
        textMeshPro.text = text;
    }
    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
            transform.Rotate(0, 180, 0);
        }
    }
}

    