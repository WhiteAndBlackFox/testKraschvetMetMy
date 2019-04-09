using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    class parties
    {
        public int idParties;
        public nomenclatures nomenclatures;

        public parties(int id, nomenclatures nomenclatures)
        {
            this.idParties = id;
            this.nomenclatures = nomenclatures;
        }

        public parties(){}

    }
}
