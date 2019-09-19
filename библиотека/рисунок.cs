﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;



namespace библиотека
{
    public class рисунок
    {
        // параметры окна
        int высота_окна;
        int ширина_окна;
        // рисовательные 
        public Bitmap картинка;
        Graphics рисовалка;
        double ширина_бара;
        Font шрифт;
        Brush кисть_текстовая;
        int отступ_баров_от_края;
        Pen p_кисть_для_выделения;        
        // котировки
        котировки котировки_1;
        int смещение_первого_бара;

        public double Ширина_бара()
        {
            return ширина_бара;
        }


        public string time_tool_tip (int координата_Х, ref List<Quotes> котировки)
        {
            int i = Convert.ToInt32(Math.Floor((координата_Х / ширина_бара) + смещение_первого_бара));
            return (котировки[i].time.ToString());

        }
        public string minimum_tool_tip(int координата_Х, ref List<Quotes> котировки)
        {
            int i = Convert.ToInt32(Math.Floor((координата_Х / ширина_бара) + смещение_первого_бара));
            return (котировки[i].minimum.ToString());

        }

        public string maximum_tool_tip(int координата_Х, ref List<Quotes> котировки)
        {
            int i = Convert.ToInt32(Math.Floor((координата_Х / ширина_бара) + смещение_первого_бара));
            return (котировки[i].maximum.ToString());

        }

        public string open_tool_tip(int координата_Х, ref List<Quotes> котировки)
        {
            int i = Convert.ToInt32(Math.Floor((координата_Х / ширина_бара) + смещение_первого_бара));
            return (котировки[i].open.ToString());

        }

        public string close_tool_tip(int координата_Х, ref List<Quotes> котировки)
        {
            int i = Convert.ToInt32(Math.Floor((координата_Х / ширина_бара) + смещение_первого_бара));
            return (котировки[i].close.ToString());
        }
        public void отрисовка
        (
            int количество_баров,
            int смещение_первого_бара_,
            ref List<Quotes> котировки,
            Color цвет_котировок,
            int периоды,
            Color цвет_фона, 
            Color цвет_разделителей_периодов,
            bool показывать_выходные,
            Color цвет_выходных,
            int таймфрейм
            
        )
        {
            смещение_первого_бара = смещение_первого_бара_;
            if (смещение_первого_бара >= котировки.Count) return;
            if (смещение_первого_бара + количество_баров >= котировки.Count)
            {
                количество_баров = котировки.Count - смещение_первого_бара - 1;

            }
            float максимум = котировки_1.максимум(котировки, смещение_первого_бара, количество_баров);
            float минимум = котировки_1.минимум(котировки, смещение_первого_бара, количество_баров);
            ширина_бара = ширина_окна / Convert.ToDouble(количество_баров);
            float k_растяжения_по_вертикали = (высота_окна - отступ_баров_от_края * 2) / (максимум - минимум);

            int точка_один_Х; // палочка опен
            int точка_один_Y;
            int точка_два_Х;
            int точка_два_Y;

            int точка_три_Х; // тело свечи
            int точка_три_Y;
            int точка_четыре_Х;
            int точка_четыре_Y;

            int точка_пять_Х; // тело свечи
            int точка_пять_Y;
            int точка_шесть_Х;
            int точка_шесть_Y;
            int ширина_хвостика = Convert.ToInt32(Math.Floor(ширина_бара / 2));
            int y = 1;
            рисовалка.Clear(цвет_фона);
            Pen перо = new Pen(цвет_котировок);
            Pen перо_периоды = new Pen(цвет_разделителей_периодов, 1);
            for (int i = смещение_первого_бара; i < смещение_первого_бара + количество_баров; i++) //собираем данные для отрисовки
            {
                точка_один_Х = Convert.ToInt32(Math.Floor(y * ширина_бара - ширина_хвостика- ширина_бара/2));
                точка_один_Y = высота_окна - (int)Math.Ceiling((котировки[i].open - минимум) * k_растяжения_по_вертикали);
                точка_два_Х = Convert.ToInt32(Math.Floor(y * ширина_бара  - ширина_бара / 2)); ;
                точка_два_Y = точка_один_Y;
                точка_три_Х = точка_два_Х;
                точка_три_Y = высота_окна - (int)Math.Ceiling((котировки[i].maximum - минимум) * k_растяжения_по_вертикали);
                точка_четыре_Х = точка_три_Х;
                точка_четыре_Y = высота_окна - (int)Math.Ceiling((котировки[i].minimum - минимум) * k_растяжения_по_вертикали);
                точка_пять_Х = точка_четыре_Х;
                точка_пять_Y = высота_окна - (int)Math.Ceiling((котировки[i].close - минимум) * k_растяжения_по_вертикали);
                точка_шесть_Х = точка_пять_Х + ширина_хвостика;
                точка_шесть_Y = точка_пять_Y;

                // рисование вертикальных линий разделители периодов
                int минут_c_начала_суток = котировки[i].time.Minute + котировки[i].time.Hour * 60;
                double temp = Convert.ToDouble(минут_c_начала_суток) / периоды;
                double temp1 = temp - Convert.ToInt32(temp);
                if (temp1 == 0)
                    if (ширина_бара > 0.4)
                    {
                        рисовалка.DrawLine(перо_периоды, точка_два_Х, 0, точка_два_Х, высота_окна);
                        
                        if (периоды /таймфрейм*ширина_бара > 30)
                        if (периоды<1440)
                        {
                            string время = котировки[i].time.Hour.ToString() + ":" + котировки[i].time.Minute.ToString();
                            рисовалка.DrawString(время, шрифт, кисть_текстовая, точка_два_Х, высота_окна - 10);
                        }

                        if (периоды / таймфрейм * ширина_бара > 40)
                        if (периоды >= 1440)
                        {
                            string дата = котировки[i].time.ToShortDateString();
                                рисовалка.DrawString(дата, шрифт, кисть_текстовая, точка_два_Х, высота_окна - 10);
                        }


                    }


                 
                // рисуем выходные 
                Pen перо_выходных = new Pen(цвет_выходных,6);
                if (показывать_выходные)
                if (i > 0)
                if (ширина_бара > 0.1)
                {                    
                    if (котировки[i].time.DayOfWeek==DayOfWeek.Monday)
                    if (котировки[i-1].time.DayOfWeek == DayOfWeek.Friday)
                        {
                                рисовалка.DrawLine
                                (
                                    перо_выходных,
                                    Convert.ToInt32(точка_два_Х - ширина_бара * 0.5), 
                                    0,
                                   Convert.ToInt32(точка_два_Х - ширина_бара * 0.5),
                                    высота_окна
                                );                          
                            }

                }


                // рисование свечи
                рисовалка.DrawLine(перо, точка_один_Х, точка_один_Y - отступ_баров_от_края, точка_два_Х, точка_два_Y - отступ_баров_от_края);
                рисовалка.DrawLine(перо, точка_три_Х, точка_три_Y - отступ_баров_от_края, точка_четыре_Х, точка_четыре_Y - отступ_баров_от_края);
                рисовалка.DrawLine(перо, точка_пять_Х, точка_пять_Y - отступ_баров_от_края, точка_шесть_Х, точка_шесть_Y - отступ_баров_от_края);



                y++;
            }
            return;

        }

        public void f_рисование_прямоугольника(Point p_старт,Point p_финиш)
        {
            рисовалка.DrawLine(p_кисть_для_выделения, p_старт.X, p_старт.Y, p_старт.X, p_финиш.Y);
            рисовалка.DrawLine(p_кисть_для_выделения, p_финиш.X, p_старт.Y, p_финиш.X, p_финиш.Y);
            рисовалка.DrawLine(p_кисть_для_выделения, p_старт.X, p_финиш.Y, p_финиш.X, p_финиш.Y);
            рисовалка.DrawLine(p_кисть_для_выделения, p_старт.X, p_старт.Y, p_финиш.X, p_старт.Y);
        }

        public int смещение_по_координате(int координата_X)
        {
            int temp = Convert.ToInt32(координата_X / ширина_бара + смещение_первого_бара);
            return temp;
        }

       public рисунок( int ширина_окна_,int высота_окна_, Color цвет_котировок_)
        {

            ширина_окна = ширина_окна_;
            высота_окна = высота_окна_;
            картинка = new Bitmap(ширина_окна, высота_окна);
            рисовалка = Graphics.FromImage(картинка);
            шрифт = new System.Drawing.Font("Arial", (float)7);
            кисть_текстовая = new SolidBrush(Color.White);
            котировки_1 = new котировки();
            отступ_баров_от_края = 20;// по вертикали
            p_кисть_для_выделения = new Pen(Color.FromArgb(255, 182, 182, 182), 5f);
        }
    }
    public struct Point
    {
        public int X;
        public int Y;
        public bool активна;
    }

}

