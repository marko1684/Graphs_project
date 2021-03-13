using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafovi_projekat
{
    class Cvor
    {
        public string name;
        public int _x1, _y1;

        public Cvor(int x1,int y1,string Name)
        {
            name = Name;
            _x1 = x1;
            _y1 = y1;
        }
    }
}
