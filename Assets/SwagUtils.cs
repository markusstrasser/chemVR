using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using AtomConfig;
using System;

namespace Swag
{
public class SwagUtils : MonoBehaviour {
        public LineRenderer lRender;
        public int segments = 2;


        public void DrawBonds(Transform a, List<Transform> bs)
        {
            foreach(Transform b in bs)
            {      
                CreateLine(a, b);
            }
        }

        public void DrawAllBonds (Dictionary<KeyValuePair<Atom,Atom>, int> bondData)
        {
            foreach (KeyValuePair<Atom, Atom> kvp in bondData.Keys)
            {
                CreateLine(kvp.Key.transform, kvp.Value.transform);
            }
        }


        public string AtomText(int p, int e, int n)
        {
            ChemistryCalc chem = GameObject.Find("ChemistryCalc").GetComponent<ChemistryCalc>();
            string text = "<sub>" + p.ToString() + "</sub>" + PTable.config[p] + "<sup>" + ChargeNess(p, e) + "</sup>";

           // gameObject.tag = "Defined";

            return text;
        }

        Transform closestTo(Transform trans, string tag = "Atom", float maxDistance = 2f)
        {
            Transform closest = null;
            float closestDist = maxDistance;
            foreach (GameObject elem in GameObject.FindGameObjectsWithTag(tag))
            {

                //loop through siblings

                float dist = Vector3.Distance(transform.position, elem.transform.position);

                if (dist < closestDist && !isSame(transform, elem.transform) && elem.GetComponent<Atom>())
                {
                    closest = elem.transform;
                    closestDist = dist;
                }
            }
            Debug.Log(closestDist);
            return closest;
        }

        static bool isSame(Transform t1, Transform t2)
        {
            return (t1.GetInstanceID() == t2.GetInstanceID());
        }

        public static string MoleculeText(Dictionary<string,int> composition)
        {
            string text = "";
            foreach(KeyValuePair<string, int> entry in composition)
            {
                text += entry.Key;

                    if (entry.Value > 1) { //no H2O1
                    text += entry.Value.ToString();
                }
            }
            return text;
        }

        public static string MoleculeTextPlus(Dictionary<string, int> composition, int charge =0)
        {
            string text = "";
            foreach (KeyValuePair<string, int> entry in composition)
            {
                text += entry.Key;
                //9x<sup><#00ff00>3</color></sup>
                if (entry.Value > 1)
                { //no H2O1
                    text += "<sub>" + entry.Value.ToString() + "</sub>";
                }
            }
            if (charge != 0)
            {
                //TODO -1 => - ... 1 ==> + ...for single charges only
                text += "<sup>" + charge + "</sup>";
            }
       
            return text;
        }

        public static Dictionary<string, int> elementComposition(Transform father)
        {
            Dictionary<string, int> elements = new Dictionary<string, int>();
            foreach (Transform child in father)
            {
                if (child.GetComponent<Atom>())
                {
                     string symb = child.GetComponent<Atom>().config.symbol;
                     if (elements.ContainsKey(symb))
                {
                    elements[symb]++;
                }
                else
                {
                    elements[symb] = 1;
                }
                }         
            }
            return elements;
        }

        public string ChargeNess(int protons, int electrons)
        {
            int charge = protons - electrons;
            if (charge > 0)
            {
                return charge.ToString() + "+";
            }
            else if (charge < 0)
            {
                return charge.ToString() + "-";
            }
            else return "";
        }

        public void CreateLine(Transform a, Transform b)
        {
            LineRenderer line = GameObject.Instantiate(lRender as LineRenderer);
            line.transform.parent = a;
            line.positionCount = segments;
            // line.useWorldSpace = true;
            Vector3 deltaVec = b.position - a.position;
            Vector3 step = deltaVec / segments;

            for (int i = 0; i < segments; i++)
            {
                line.SetPosition(i, a.position + ((step) * i));
            }
            line.SetPosition(segments - 1, b.position); //last piece
            line.tag = "bond";
        }

        public void removeChildrenWithType()
        {

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

        internal string MoleculeText(IDictionary comp)
        {
            throw new NotImplementedException();
        }


        //(GameObject.FindGameObjectWithTag("RayLocation").transform)
        //link to player (this)
    }
}

