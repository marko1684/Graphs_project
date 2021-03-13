using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafovi_projekat
{
    class Ivica
    {
        public string NodeName1,NodeName2;
        public int weight;

        public Ivica(string _NodeName1,string _NodeName2,int _weight)
        {
            NodeName1 = _NodeName1;
            NodeName2 = _NodeName2;
            weight = _weight;
        }
    }
}
