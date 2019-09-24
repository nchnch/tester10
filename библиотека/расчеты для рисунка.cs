using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace b_библиотека_форм
{
    public class расчеты_для_рисунка
    {

        // int 
        int vi_количество_баров = 300;
        int vi_стартовое_смещение = 0;
        int vi_таймфрейм;
        int vi_ширина_рисунка;
        int vi_высота_рисунка;
        int vi_периоды=1440;// как часто показывать разделители в минутах
        int vi_масштаб;// текущий масштаб показа
        int vi_базовое_количество_баров;// для масштаба стартового


        // цвета
        Color vc_цвет_котировок=Color.FromArgb(255, 122, 244, 0);
        Color vc_цвет_фона= Color.Black;
        Color vc_цвет_выходных = Color.FromArgb(160, 55, 55, 55);
        Color vc_цвет_разделителей_периодов = Color.FromArgb(255, 55, 55, 55);

        // double
        double vd_сдвиг_при_перемотке;// при перемотке
        double vd_шаг_масштаба;// на сколько больше показыать свечей при измененее масштаба на 1

        // объекты формы

        Button o_кнопка_загрузка;
        OpenFileDialog o_файл_диалог;
        PictureBox pictureBox1;
        ComboBox o_comboBox_периоды;

        // bool 
        bool vb_режим_выделения;
        bool vb_мышь_вниз;      
        bool vb_показывать_выходные;

        // объекты , структуры , списки
        b_библиотека_общая.котировки o_котировки1 = new b_библиотека_общая.котировки();
        рисунок o_рисунок_1;
        Point s_точка_старт;
        Point s_точка_финиш;
        List<Quotes> l_Q;
        ToolTip o_подсказка;
        

        // дата и время
        DateTime vd_старт;
       

        public void fv_инизиализация_обектов_формы
        (
           ref Button o_кнопка_загрузка_,
           ref ComboBox o_comboBox_периоды_,
           ref TextBox o_textBox_бары,
           ref DateTimePicker o_dateTimePicker_старт,
           ref DateTimePicker o_dateTimePicker_финиш,
           ref TrackBar o_trackBar_масштаб,
           ref Button o_кнопка_перемотка_назад,
           ref Button o_кнопка_перемотка_вперед,
           ref ComboBox o_comboBox_цвета,
           ref CheckBox o_CheckBox_приближение,
           ref CheckBox o_CheckBox_показывать_выходные,
           ref OpenFileDialog o_файл_диалог_,
           ref PictureBox pictureBox1_

        )
        {
            o_comboBox_периоды = o_comboBox_периоды_;
            List<int> numbers = new List<int>() { 1440, 240, 60, 15};// задаем периоды 
            o_comboBox_периоды.DataSource = numbers;// присваиваем периоды
            o_comboBox_периоды.SelectedIndexChanged += e_comboBox_периоды_изменение;
            o_comboBox_периоды.MouseLeave += e_comboBox_периоды_MouseLeave;
            o_кнопка_загрузка = o_кнопка_загрузка_;
            o_файл_диалог = o_файл_диалог_;
            o_кнопка_загрузка.Click += e_кнопка_загрузка_клик;
            pictureBox1 = pictureBox1_;
            vi_ширина_рисунка = pictureBox1.Width;          
            vi_высота_рисунка = pictureBox1.Height;
        }

        private void e_comboBox_периоды_MouseLeave(object sender, EventArgs e)
        {
            vi_периоды = Convert.ToInt32(o_comboBox_периоды.Text);
            fv_рисуем();
        }

        private void e_comboBox_периоды_изменение(object sender, EventArgs e)
        {
            vi_периоды = Convert.ToInt32(o_comboBox_периоды.SelectedItem);
            fv_рисуем();

        }

        // события


        private void e_кнопка_загрузка_клик(object sender, EventArgs e)// кнопка загрузка нажата
        {
            o_файл_диалог.ShowDialog();
            string путь = o_файл_диалог.FileName;
            o_котировки1.загрузка_котировок(путь);
            vi_таймфрейм = o_котировки1.time_frame();
            l_Q = o_котировки1.Q_();
            o_рисунок_1 = new рисунок(vi_ширина_рисунка, vi_высота_рисунка, vc_цвет_котировок, l_Q);
            fv_рисуем();
        }

        void fv_рисуем()
        {
            if (vi_количество_баров <= 0)
            {
                vi_количество_баров = 1;
            }

            if (vi_количество_баров >= l_Q.Count)
            {
                vi_количество_баров = l_Q.Count - 1;
            }

            if (vi_стартовое_смещение < 0)
            {
                vi_стартовое_смещение = 0;
            }

            if (vi_стартовое_смещение + vi_количество_баров > l_Q.Count - 1)
            {
                vi_стартовое_смещение = l_Q.Count - 1 - vi_количество_баров;
            }

            if (vi_стартовое_смещение < 0)
            {
                vi_стартовое_смещение = 0;
            }

            // корреция идет на основе двух переменных смещения и количества свечей

           // textBox1.Text = Convert.ToString(vi_количество_баров);
           // dateTimePicker1.Value = l_Q.ElementAt(vi_стартовое_смещение).time;
          //  dateTimePicker2.Value = l_Q.ElementAt(vi_стартовое_смещение + vi_количество_баров).time;
          //  double temp = Convert.ToDouble(vi_количество_баров) / vi_базовое_количество_баров;
          //  double temp2 = Math.Log10(temp);
          //  double temp3 = Math.Log10(vd_шаг_масштаба);
          //  trackBar1.Value = Convert.ToInt32(temp2 / temp3) + 10;
            o_рисунок_1.отрисовка
            (
                vi_количество_баров,
                vi_стартовое_смещение,
                vc_цвет_котировок,
                vi_периоды,
                vc_цвет_фона,
                vc_цвет_разделителей_периодов,
                vb_показывать_выходные,
                vc_цвет_выходных,
                vi_таймфрейм
            );

            pictureBox1.Image = o_рисунок_1.картинка;
        }







    }
}
