using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// проверка нужна на корректность дат

namespace библиотека
{
    public class котировки
    {
       // public Quotes[] quotes = new Quotes[1];// создаем массив объектов класса равный количеству котировок
        public DateTime latest_time;
        public DateTime earliest_time;
        public Time[] time_massiv = new Time[1];// создаем массив ссылок на quites где одной минуте соответсвует одно данное.
        public List<Quotes> Q = new List<Quotes>() { };// объявили список
        public List<Time> T = new List<Time>() { };// объявили список
        public int таймфрейм;// потом подавать в функцию его
        
        public bool загрузка_котировок(string path)
        {
            
            int.TryParse(string.Join("", path.Where(c => char.IsDigit(c))), out таймфрейм);// парсим тайм фрейм
            string textFromFile = String.Empty; // строка для получения данных их файла 
            var quotss = File.ReadAllLines(path);// читаем все строки в массив
            int y=0;
            for (int i = 0; i < quotss.Length; i++)
            {
                Quotes q = new Quotes();
                string[] temp = quotss[i].Split(new char[] { ',' });// создаем массив их элементов разделенных запятой
                string data = temp[0] + "  " + temp[1];
                q.time = DateTime.Parse(data);
                temp[2] = temp[2].Replace(".", ",");
                temp[3] = temp[3].Replace(".", ",");
                temp[4] = temp[4].Replace(".", ",");
                temp[5] = temp[5].Replace(".", ",");
                q.open = float.Parse(temp[2]);
                q.maximum = float.Parse(temp[3]);
                q.minimum = float.Parse(temp[4]);
                q.close = float.Parse(temp[5]);
                string[] temp4 = temp[6].Split(new char[] { '\r' });
                q.volume = int.Parse(temp4[0]);
                Q.Add(q);
                if (y == 1000000)
                {
                    y = 0;
                }
                y++;              
            }           

            latest_time = Q.ElementAt(Q.Count() - 1).time;
            earliest_time = Q.ElementAt(0).time;
            создние_временного_массива(таймфрейм, earliest_time, latest_time);
            return true;
        }

 
        public float максимум(List<Quotes> котировки, int старт, int количество)
        {
            float максимум = 0;
            for (int i = 0; i < количество; i++)
            {
                if (котировки[старт + i].maximum > максимум)
                {
                    максимум = котировки[старт + i].maximum;
                }
            }
            return максимум;
        }
        public float минимум(List<Quotes> котировки, int старт, int количество)
        {
            float минимум = 100000000000000;
            for (int i = 0; i < количество; i++)
            {
                if (котировки[старт + i].minimum < минимум)
                {
                    минимум = котировки[старт + i].minimum;
                }
            }
            return минимум;
        }



        void создние_временного_массива(int step_min, DateTime старт, DateTime финиш)
        {
            int величина_массива = разница_в_минутах(старт, latest_time)/step_min;
            DateTime time = старт;
            DateTime temp;
            Time значение = new Time();
            int i = 1;
            while (time<=latest_time)
            {
                temp = Q.ElementAt(i).time;// отладка
                if (time<Q.ElementAt(i).time)// 
                {
                    значение.time = time;
                    значение.смещение = i-1;
                    T.Add(значение);
                    time = time.AddMinutes(step_min);
                    continue;
                }
                i++;
                значение.time = time;
                значение.смещение = i - 1;
                T.Add(значение);
                time = time.AddMinutes(step_min);               
            }


        }

        public int разница_в_минутах(DateTime time_start, DateTime time_finish)
        {
            return Convert.ToInt32((time_finish.Ticks - time_start.Ticks) / 10000000 / 60);
        }


        public int смещение (DateTime time)
        {
            int N = разница_в_минутах(earliest_time, time)/таймфрейм;
            if (N > T.Count - 1)
                return Q.Count - 1;
            int temp = T.ElementAt(N).смещение;
            return temp;
        }  




        public int смещение2 (DateTime time ) // метод постепенного приближения
        {
            bool точное_или_более_позднее=true;
            int количество_баров = Q.Count();
            int смещение_минимум = 0;
            int смещение_максимум = количество_баров - 1;
            int пробное_смещение = (смещение_максимум - смещение_минимум) / 2 + смещение_минимум;
            DateTime пробное_время;
            DateTime пробное_время_прошлое;
            DateTime пробное_время_след;
            while (пробное_смещение != смещение_минимум)
            {                                           
                пробное_время = Q.ElementAt(пробное_смещение).time;
                пробное_время_прошлое = Q.ElementAt(пробное_смещение-1).time;// отладка
                пробное_время_след = Q.ElementAt(пробное_смещение + 1).time;// отладка

                if (пробное_время>time)
                {
                    смещение_максимум = пробное_смещение;
                    пробное_смещение = (смещение_максимум - смещение_минимум) / 2 + смещение_минимум;
                    continue;
                }

                if (пробное_время < time)
                {
                    смещение_минимум = пробное_смещение;
                    пробное_смещение = (смещение_максимум - смещение_минимум) / 2 + смещение_минимум;
                    continue;
                }
                
                if (пробное_время == time)
                {
                    return пробное_смещение;
                }
            }

            if (точное_или_более_позднее) return пробное_смещение + 1;
            return пробное_смещение;
        }



        // тут загружаются котировки и все хранится что с этим связанно
        //  public int смещение_бара(DateTime время, bool точное, bool более_позднее, bool более_раннее)
        //   {

        //  }

        //  public DateTime время_по_смещению(int смещение)
        //  {

        //  }

        //  public int количество_котировок()
        // {

        //  }


    }
}

public struct Quotes
{
    public float open;
    public float maximum;
    public float minimum;
    public float close;
    public int volume;
    public DateTime time;

}

public struct Time
{
    public int смещение;
    public DateTime time;
}