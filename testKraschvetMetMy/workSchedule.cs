using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    class workSchedule
    {
        public parties parties;
        public machineTools machineTools;
        public int timeStart;

        public workSchedule(parties parties, machineTools machineTools, int timeStart)
        {
            this.parties = parties;
            this.machineTools = machineTools;
            this.timeStart = timeStart;
        }

        public workSchedule(){}

    }
}
