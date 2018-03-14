using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Swag
{
public class SwagUtils : MonoBehaviour {
        public LineRenderer lRender;
        public int segments = 2;

        public void CreateLine(Transform a, Transform b)
        {
            LineRenderer line = GameObject.Instantiate(lRender as LineRenderer);
            line.transform.parent = a;
            line.positionCount = segments;
            // line.useWorldSpace = true;
            Vector3 deltaVec = b.position - a.position;
            Vector3 step = deltaVec / segments;

            for (int i = 0; i <= segments; i++)
            {
                line.SetPosition(i, a.position + (step * i));
            }
        }

        void sort()
        {
            GameObject[] eles = GameObject.FindGameObjectsWithTag("Electron");
            GameObject closest = eles.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
                           .FirstOrDefault();   //or use .FirstOrDefault();  if you need just toArray(

           // GameObject tmp = GameObject.Find("TextMeshTip");
            //tmp.GetComponent<TextMeshManager>().DisplayText(Interesting.GetComponent<Atom>().textMeshName);
            //HOW TO make <ATOM> general? So it works for <Molecule>, <Cell>

            //tmp.GetComponent<TextMeshManager>().DisplayText("yoyl");
        }


        //(GameObject.FindGameObjectWithTag("RayLocation").transform)
        //link to player (this)
    }
}

