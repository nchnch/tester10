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
        int vi_базовое_количество_баров=300;// для масштаба стартового


        // цвета
        Color vc_цвет_котировок=Color.FromArgb(255, 122, 244, 0);
        Color vc_цвет_фона= Color.Black;
        Color vc_цвет_выходных = Color.FromArgb(160, 55, 55, 55);
        Color vc_цвет_разделителей_периодов = Color.FromArgb(255, 55, 55, 55);

        // double
        double vd_сдвиг_при_перемотке=0.2;// при перемотке
        double vd_шаг_масштаба=1.6;// на сколько больше показыать свечей при измененее масштаба на 1

        // объекты формы

        Button o_кнопка_загрузка;
        OpenFileDialog o_файл_диалог;
        PictureBox pictureBox1;
        ComboBox o_comboBox_периоды;
        TextBox o_textBox_количество_баров;
        DateTimePicker o_dateTimePicker_старт;
        DateTimePicker o_dateTimePicker_финиш;
        TrackBar o_trackBar_масштаб;
        Button o_кнопка_перемотка_назад;
        Button o_кнопка_перемотка_вперед;
        ComboBox o_comboBox_цвета;
        ColorDialog o_colorDialog;
        CheckBox o_сheckBox_приближение;

        // bool 
        bool vb_режим_выделения;
        bool vb_мышь_вниз;      
        bool vb_показывать_выходные;

        // объекты , структуры , списки
        b_библиотека_общая.котировки o_котировки_1 = new b_библиотека_общая.котировки();
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
           ref TextBox o_textBox_бары_,
           ref DateTimePicker o_dateTimePicker_старт_,
           ref DateTimePicker o_dateTimePicker_финиш_,
           ref TrackBar o_trackBar_масштаб_,
           ref Button o_кнопка_перемотка_назад_,
           ref Button o_кнопка_перемотка_вперед_,
           ref ComboBox o_comboBox_цвета_,
           ref ColorDialog o_colorDialog_,
           ref CheckBox o_сheckBox_приближение_,
           ref CheckBox o_CheckBox_показывать_выходные,
           ref OpenFileDialog o_файл_диалог_,
           ref PictureBox pictureBox1_

        )
        {
            o_сheckBox_приближение = o_сheckBox_приближение_;


            o_colorDialog = o_colorDialog_;
            o_colorDialog.FullOpen = true;
            o_comboBox_цвета = o_comboBox_цвета_;
            List<string> vs_цвета = new List<string>() {"цвет баров","цвет фона","цвет выходных","цвет разделителей"};// задаем периоды
            o_comboBox_цвета.DataSource = vs_цвета;
            o_comboBox_цвета.SelectedIndexChanged += e_comboBox_цвета_SelectedIndexChanged;
            o_кнопка_перемотка_вперед = o_кнопка_перемотка_вперед_;
            o_кнопка_перемотка_вперед.Click += e_кнопка_перемотка_вперед_Click;

            o_кнопка_перемотка_назад = o_кнопка_перемотка_назад_;
            o_кнопка_перемотка_назад.Click += e_кнопка_перемотка_назад_Click;

            o_trackBar_масштаб = o_trackBar_масштаб_;
            o_trackBar_масштаб.Maximum = 20;
            o_trackBar_масштаб.Minimum = 0;
            o_trackBar_масштаб.Value = 0;
            o_trackBar_масштаб.Scroll += e_trackBar_масштаб_Scroll;

            o_dateTimePicker_финиш = o_dateTimePicker_финиш_;
            o_dateTimePicker_финиш.Format = DateTimePickerFormat.Custom;
            o_dateTimePicker_финиш.CustomFormat = "dd.MM.yyyy HH:mm";
            o_dateTimePicker_финиш.CloseUp += e_dateTimePicker_финиш_CloseUp;

            o_dateTimePicker_старт = o_dateTimePicker_старт_;
            o_dateTimePicker_старт.Format = DateTimePickerFormat.Custom;
            o_dateTimePicker_старт.CustomFormat = "dd.MM.yyyy HH:mm";
            o_dateTimePicker_старт.CloseUp += e_dateTimePicker_старт_CloseUp;

            o_textBox_количество_баров = o_textBox_бары_;
            o_textBox_количество_баров.LostFocus += e_textBox_количество_баров_LostFocus;

            o_comboBox_периоды = o_comboBox_периоды_;
            List<int> numbers = new List<int>() { 1440, 240, 60, 15};// задаем периоды 
            o_comboBox_периоды.DataSource = numbers;// присваиваем периоды
            o_comboBox_периоды.SelectedIndexChanged += e_comboBox_периоды_изменение;
            o_comboBox_периоды.LostFocus += e_comboBox_периоды_LostFocus;

            o_кнопка_загрузка = o_кнопка_загрузка_;
            o_файл_диалог = o_файл_диалог_;
            o_кнопка_загрузка.Click += e_кнопка_загрузка_клик;

            pictureBox1 = pictureBox1_;
            vi_ширина_рисунка = pictureBox1.Width;          
            vi_высота_рисунка = pictureBox1.Height;
        }

        private void e_comboBox_цвета_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (o_colorDialog.ShowDialog() == DialogResult.Cancel)
                return;

            if (o_comboBox_цвета.SelectedIndex==0)
                vc_цвет_котировок = o_colorDialog.Color;
            if (o_comboBox_цвета.SelectedIndex == 1)
                vc_цвет_фона = o_colorDialog.Color;
            if (o_comboBox_цвета.SelectedIndex == 2)
                vc_цвет_выходных = o_colorDialog.Color;
            if (o_comboBox_цвета.SelectedIndex == 3)
                vc_цвет_разделителей_периодов = o_colorDialog.Color;
            fv_рисуем();
        }

        private void e_кнопка_перемотка_вперед_Click(object sender, EventArgs e)
        {
            vi_стартовое_смещение = Convert.ToInt32(vi_стартовое_смещение + vi_количество_баров * vd_сдвиг_при_перемотке);
            fv_рисуем();
        }

        private void e_кнопка_перемотка_назад_Click(object sender, EventArgs e)
        {
            vi_стартовое_смещение = Convert.ToInt32(vi_стартовое_смещение - vi_количество_баров * vd_сдвиг_при_перемотке);
            fv_рисуем();
        }

        private void e_trackBar_масштаб_Scroll(object sender, EventArgs e)
        {
            vi_масштаб = o_trackBar_масштаб.Value - 10;
            double temp = Math.Pow(vd_шаг_масштаба, vi_масштаб);
            int j_новое_количество_баров = Convert.ToInt32(vi_базовое_количество_баров * temp);
            vi_стартовое_смещение = vi_стартовое_смещение - (j_новое_количество_баров - vi_количество_баров) / 2;
            vi_количество_баров = j_новое_количество_баров;
            fv_рисуем();
        }

        private void e_dateTimePicker_финиш_CloseUp(object sender, EventArgs e)
        {
            DateTime финиш = o_dateTimePicker_финиш.Value;
            int смещение_финиш = o_котировки_1.смещение(финиш);
            vi_количество_баров = смещение_финиш - vi_стартовое_смещение;
            fv_рисуем();
        }

        private void e_dateTimePicker_старт_CloseUp(object sender, EventArgs e)
        {
            vd_старт = o_dateTimePicker_старт.Value;
            vi_стартовое_смещение = o_котировки_1.смещение(vd_старт);
            fv_рисуем();
        }

        private void e_comboBox_периоды_LostFocus(object sender, EventArgs e)
        {
            vi_периоды = Convert.ToInt32(o_comboBox_периоды.Text);
            fv_рисуем();
        }

        private void e_textBox_количество_баров_LostFocus(object sender, EventArgs e)
        {
            if (o_textBox_количество_баров.Text == string.Empty) return;
            try
            {
                if (Convert.ToInt32(o_textBox_количество_баров.Text) == 0) return;
                vi_количество_баров = Convert.ToInt32(o_textBox_количество_баров.Text);
                fv_рисуем();
            }
            catch
            {
                return;
            }

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
            o_котировки_1.загрузка_котировок(путь);
            vi_таймфрейм = o_котировки_1.time_frame();
            l_Q = o_котировки_1.Q_();
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

            o_textBox_количество_баров.Text = Convert.ToString(vi_количество_баров);
            o_dateTimePicker_старт.Value = l_Q.ElementAt(vi_стартовое_смещение).time;
            o_dateTimePicker_финиш.Value = l_Q.ElementAt(vi_стартовое_смещение + vi_количество_баров).time;
            double temp = Convert.ToDouble(vi_количество_баров) / vi_базовое_количество_баров;
            double temp2 = Math.Log10(temp);
            double temp3 = Math.Log10(vd_шаг_масштаба);
            o_trackBar_масштаб.Value = Convert.ToInt32(temp2 / temp3) + 10;
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
