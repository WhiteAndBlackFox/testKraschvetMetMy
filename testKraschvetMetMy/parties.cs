using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    // Партия
    class parties
    {
        public int idParties; // Ид
        public nomenclatures nomenclatures; // Номенклатура в партии

        public parties(int id, nomenclatures nomenclatures)
        {
            this.idParties = id;
            this.nomenclatures = nomenclatures;
        }

        public parties(){}

    }
}
