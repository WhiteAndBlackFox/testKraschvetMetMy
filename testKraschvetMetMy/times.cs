using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    class times
    {
        public nomenclatures nomenclatures; // Ссылка на номенклатуру
        public int iTime;  // Время обработки

        public times(nomenclatures nomenclatures, int iTime)
        {
            this.nomenclatures = nomenclatures;
            this.iTime = iTime;
        }

        public times(){}

    }
}
