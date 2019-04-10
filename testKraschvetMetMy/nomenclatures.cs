using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    // Номенклатура
    class nomenclatures
    {
        public int idNomenclatures; // Ид
        public String nameNomenclatures; // Название

        public nomenclatures(int id, string name)
        {
            this.idNomenclatures = id;
            this.nameNomenclatures = name;
        }

        public nomenclatures(){}
    }
}
