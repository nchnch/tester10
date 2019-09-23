using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;


namespace библиотека
{
    public class расчеты_для_рисунка
    {

        // int 
        int vi_количество_баров = 300;
        int vi_стартовое_смещение = 0;
        int vi_таймфрейм;
        int vi_ширина_русунка;
        int vi_высота_рисунка;
        int vi_периоды;


        // цвета
        Color vc_цвет_котировок;
        Color vc_цвет_фона;
        Color vc_цвет_выходных;
        Color vc_цвет_разделителей_периодов;

        // double

        // объекты формы

        Button o_кнопка_загрузка;
        OpenFileDialog o_файл_диалог;

        // объекты
        котировки o_котировки1;
        рисунок o_рисунок1;
        // прочее
        List<Quotes> l_Q;

        // дата и время
        DateTime vd_старт;




        double vd_сдвиг;
        public библиотека.котировки o_котировки_1 = new библиотека.котировки();
        public библиотека.рисунок o_рисунок_1;
        ToolTip o_подсказка;
        int vi_масштаб;
        // для пеермотки мышкой
        bool vb_мышь_вниз;

        int vi_базовое_количество_баров;// для масштаба стартового
        double vd_шаг_масштаба;
 
        // для выделения и перемотки
        bool vb_режим_выделения;
        библиотека.Point s_точка_старт;
        библиотека.Point s_точка_финиш;
        bool vb_показывать_выходные;

        public void fv_инизиализация_обектов_формы
        (
           ref Button o_загрузка,
           ref OpenFileDialog o_файл_диалог_,
           ref котировки o_котировки1_,
           Color vc_цвет_котировок,
           int vi_ширина_русунка_,
           int vi_высота_рисунка_

        )
        {
            vi_ширина_русунка = vi_ширина_русунка_;
            vi_высота_рисунка = vi_высота_рисунка_;
            o_кнопка_загрузка = o_загрузка;
            o_файл_диалог = o_файл_диалог_;
            o_котировки1 = o_котировки1_;
            o_кнопка_загрузка.Click += O_кнопка_загрузка_Click1;

        }

        private void O_кнопка_загрузка_Click1(object sender, EventArgs e)// кнопка загрузка нажата
        {
            o_файл_диалог.InitialDirectory = @"C:\Users\Владислав\Dropbox\С#\history\EURUSD\EURUSD240";
            o_файл_диалог.ShowDialog();
            string файл = o_файл_диалог.SafeFileName;
            string путь = o_файл_диалог.FileName;
            o_котировки1.загрузка_котировок(путь);
            vi_таймфрейм = o_котировки1.time_frame();
            l_Q = o_котировки1.Q_();
            o_рисунок1 = new рисунок(vi_ширина_русунка, vi_высота_рисунка, vc_цвет_котировок, l_Q);
        }







    }
}
