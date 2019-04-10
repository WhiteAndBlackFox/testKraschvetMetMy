using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testKraschvetMetMy
{
    class workSchedule
    {
        public parties parties; // Ссылка на партию
        public machineTools machineTools; // Ссылка на машину
        public int timeStart; // Время начала этапа

        public workSchedule(parties parties, machineTools machineTools, int timeStart)
        {
            this.parties = parties;
            this.machineTools = machineTools;
            this.timeStart = timeStart;
        }

        public workSchedule(){}

    }
}
