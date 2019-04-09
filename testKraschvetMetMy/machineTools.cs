using System;
using System.Collections;

namespace testKraschvetMetMy
{
    class machineTools
    {
        private int id;
        private string name;
        private ArrayList arrTime;
        public ArrayList arrWorks;
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

        public int getID()
        {
            return id;
        }

        public void setID(int id)
        {
            this.id = id;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public int getCurrentLoad()
        {
            return currentLoad;
        }

        public bool checkNomenclature(int id)
        {
            foreach (times t in arrTime)
            {
                if (t.nomenclatures.idNomenclatures == id)
                    return true;
            }
            return false;
        }

        public int addTime(times t)
        {
            if (!checkNomenclature(t.nomenclatures.idNomenclatures))
                arrTime.Add(t);
            return 0;
        }

        public int getTimeForProcessingById(int id)
        {
            foreach (times t in arrTime)
            {
                if (t.nomenclatures.idNomenclatures == id)
                    return t.iTime;
            }
            return -1;
        }

        public int AssignJob(parties p)
        {
            int iTime = getTimeForProcessingById(p.nomenclatures.idNomenclatures);
            if (iTime != -1)
            {
                currentLoad += iTime;
                arrWorks.Add(p);
                return 0;
            }
            return -1;
        }

    }

    public class machineToolComparerCurrentLoad : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            int cX = ((machineTools)x).getCurrentLoad();
            int cY = ((machineTools)y).getCurrentLoad();
            return ((cX < cY) ? (-1) : ((cX > cY) ? (1) : (0)));
        }
    }

    public class machineToolComparerID : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            int cX = ((machineTools)x).getID();
            int cY = ((machineTools)y).getID();
            return ((cX < cY) ? (-1) : ((cX > cY) ? (1) : (0)));
        }
    }
}
