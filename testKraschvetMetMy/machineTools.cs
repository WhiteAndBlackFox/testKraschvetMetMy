using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    class machineTools
    {
        private int id;
        private String name;
        private ArrayList arrTime;
        private ArrayList arrWorks;
        private int currentLoad;

        public machineTools(){}

        public machineTools(int id, string name)
        {
            this.id = id;
            this.name = name;
            arrTime = new ArrayList();
            arrWorks = new ArrayList();
            currentLoad = 0;
        }


    }
}
