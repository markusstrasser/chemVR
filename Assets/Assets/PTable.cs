using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtomConfig
{
    public class Bond
    {
        //ionic bonds don't share Electrons. The higher EN Atom just takes it
        //covalent bonds both ++ a valence Electron
        public string type;
        public float ENTreshold = 1.8f;
        public Atom to;
        public Atom self;
        public Atom stronger;
        public Bond(Atom myself, Atom other)
        {
            type = BondType(myself, other);
            to = other;
            self = myself;
            ElectronTrading(type);
        }

        void ElectronTrading(string type)
        {
            if (type == "ionic")
            {
                if (self.state.EN > to.state.EN)
                {
                    self.state.valence++;
                    to.state.valence--;
                }
                else
                {
                    self.state.valence--;
                    to.state.valence++;
                }
            }
            else if (type == "covalent")
            {
                self.state.valence++;
                to.state.valence++;
            }
        }

        string BondType(Atom A, Atom B)
        {
            float deltaEN = Mathf.Abs(A.state.EN - B.state.EN);
            if (deltaEN > ENTreshold)
            {
                return "ionic";
            }
            return "covalent";
        }
    }

    public class Config {
        public string symbol;
        public string name;
        public int protons;
        public int valence;
        public int capacity;
        public float EN; //ElectroNegativity
        public Atom[] bonded;
    }
    public class PTable
    {
        public static int[] OrbitCapacity = new int[] { 2, 8, 18 };
        public static List<Config> config = new List<Config>(){
            
            new Config()
            {
                name = "noAtom"
            },
             new Config(){
                 name = "Hydrogen",
                 symbol = "H",
                 protons = 1,
                 valence = 6,
                 EN = 2.2f,
                 capacity = 1,
             },
             new Config(){
                 name = "Oxygen",
                 symbol = "O",
                 protons = 8,
                 valence = 6,
                 capacity = 2,
                 EN = 3.44f
             }
        };
    }
}



