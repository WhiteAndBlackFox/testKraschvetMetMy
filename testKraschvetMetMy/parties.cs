using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    class parties
    {
        public int id;
        public nomenclatures nomenclature;

        public parties(int id, nomenclatures nomenclature)
        {
            this.id = id;
            this.nomenclature = nomenclature;
        }

        public parties(){}

    }
}
