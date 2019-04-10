using System;
using System.Collections;

namespace testKraschvetMetMy
{
    class machineTools
    {
        private int id; // ИД
        private string name; // Название
        private ArrayList arrTime; // Массив связанных номенклатур с временем обработки
        public ArrayList arrWorks; // Массив работ
        private int currentLoad; // Текущий уровень загрузки считая от нулевого момента времени

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

        /* Возвращает истину, если машина может обрабатывать номенклатуру */
        public bool checkNomenclature(int id)
        {
            foreach (times t in arrTime)
            {
                if (t.nomenclatures.idNomenclatures == id)
                    return true;
            }
            return false;
        }

        /* Добавляет номенклатуру и время ее обработки в описание оборудования
        повторное вхождение номенклатуры - отбрасывается */
        public int addTime(times t)
        {
            if (!checkNomenclature(t.nomenclatures.idNomenclatures))
                arrTime.Add(t);
            return 0;
        }

        // Возвращает время, требуемое на обработку номенклатуры по её ID
        public int getTimeForProcessingById(int id)
        {
            foreach (times t in arrTime)
            {
                if (t.nomenclatures.idNomenclatures == id)
                    return t.iTime;
            }
            return -1;
        }

        // Записать партию в план обработки машины
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

    // Предикаты для сортировки массива оборудования
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
