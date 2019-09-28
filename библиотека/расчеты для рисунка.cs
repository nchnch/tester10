// сделать нарезку любых баров
// ренко ( псевдо ... по своей системе сделать по размеру на основе открытий) 
// свечей
// любой период
// сравнение нескольких источников 
// фильтрация всех разрывов, гепов , больших свечей, странных котировок
// модификация истории
// задом на перед 
// изменение местами кусков 
// изменение размера баров
//
//---------
// тестер 
// настройки для оценки и фильтрации сета в пакете 
// средняя приыльность месячная ,недельная, квартальная, годовая , 
// скорость восстановления
// и прочие характеристики
// визуализация сделок 
// график баланса 
// как в мт и в привязке к свечам 

// тестирование 
// привязка к базе данных результатов тестов
// качественное сложение тестов 
// синтетика
// создание котировок 
// изучение
// оценка синтетики на предмет тренда и флета 

// написать все тактики что были на очереди + умное управление лотом
// протестировать на разных инстурменах и синтетике.

// создние рабочик пакетов 

// управление торговлей пакетами ...
// общий лот ... контроль ...
// торговля по фикс протоколу и через МТ

// переоптимизация пакетов  с какой то частотой ,формирование торговых пакетов на полном автомате

// генетика 
// разметка истории
// изменения пораметров тактики с помощью генетики, профит,стоп 
// классификация раннка по кластерам 
// 

// умная оптимиация сетов
// грубо сначала потом более детально

//----------------------------------------

// количество баров и дату связать воедино что бы изменение одного параметра касалось другого.
// все прокрутки на основе количества баров 
// масштаб с данными остальными 
// перемотка кнопка и мышкой тоже связать 
// форма сначала делается что бы все связанно было 
// а потом отображение уже 
// в форме две функции сбор в коммуникаторе расчет отображение и в форме отображение этих настроек

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;



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
        int vi_масштаб_максимум = 40;
        int vi_масштаб_минимум = 0;
        int vi_позиция_X1;// для перемотки мышкой
        int vi_позиция_X2;


        // цвета
        Color vc_цвет_котировок=Color.FromArgb(255, 122, 244, 0);
        Color vc_цвет_фона= Color.Black;
        Color vc_цвет_выходных = Color.FromArgb(160, 55, 55, 55);
        Color vc_цвет_разделителей_периодов = Color.FromArgb(255, 55, 55, 55);
        Color vc_цвет_заливки = Color.FromArgb(255, 138, 138, 138);

        // double
        double vd_сдвиг_при_перемотке=0.2;// при перемотке
        double vd_шаг_масштаба=1.6;// на сколько больше показыать свечей при измененее масштаба на 1

        // объекты формы

        Button o_кнопка_загрузка;
        OpenFileDialog o_файл_диалог;
        PictureBox o_pictureBox;
        PictureBox o_pictureBox_приближение=new PictureBox();
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
        CheckBox o_CheckBox_показывать_выходные;
        ToolTip o_toolTip;
        SolidBrush o_кисть_для_заливки; 
        



        // bool 
        bool vb_режим_приближения=false;
        bool vb_мышь_была_внизу;      
        bool vb_показывать_выходные=true;

        // объекты , структуры , списки
        b_библиотека_общая.котировки o_котировки_1 = new b_библиотека_общая.котировки();
        рисунок o_рисунок_1;
        Point s_точка_старт;
        List<Quotes> l_Q;
        //ToolTip o_подсказка;
        

        // дата и время
        DateTime vd_старт;

        // рисование приближения

        Pen o_pen = new Pen(Brushes.Blue, 0.8f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };// перо
        Rectangle o_rectangle; // глобальный прямоугольник
        Brush o_кисть = Brushes.Azure;



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
           ref CheckBox o_CheckBox_показывать_выходные_,
           ref OpenFileDialog o_файл_диалог_,
           ref PictureBox pictureBox_


        )
        {
            o_кисть_для_заливки = new SolidBrush(vc_цвет_заливки);

            o_pictureBox = pictureBox_;
           // o_pictureBox.MouseMove += e_pictureBox_MouseMove;

            o_toolTip = new ToolTip();
            o_pictureBox.DoubleClick += e_pictureBox_DoubleClick;
            o_pictureBox.MouseLeave += e_pictureBox_MouseLeave;
            

            o_CheckBox_показывать_выходные = o_CheckBox_показывать_выходные_;
           o_CheckBox_показывать_выходные.Checked=true;
            o_CheckBox_показывать_выходные.CheckedChanged += e_CheckBox_показывать_выходные_CheckedChanged;

            o_сheckBox_приближение = o_сheckBox_приближение_;
            o_сheckBox_приближение.Checked=false;
            o_сheckBox_приближение.CheckedChanged += e_сheckBox_приближение_CheckedChanged;

            o_colorDialog = o_colorDialog_;
            o_colorDialog.FullOpen = true;
            o_comboBox_цвета = o_comboBox_цвета_;
            List<string> vs_цвета = new List<string>() {"цвет баров","цвет фона","цвет выходных","цвет разделителей","цвет заливки"};// задаем периоды
            o_comboBox_цвета.DataSource = vs_цвета;
            o_comboBox_цвета.SelectedIndexChanged += e_comboBox_цвета_SelectedIndexChanged;
            o_кнопка_перемотка_вперед = o_кнопка_перемотка_вперед_;
            o_кнопка_перемотка_вперед.Click += e_кнопка_перемотка_вперед_Click;

            o_кнопка_перемотка_назад = o_кнопка_перемотка_назад_;
            o_кнопка_перемотка_назад.Click += e_кнопка_перемотка_назад_Click;

            o_trackBar_масштаб = o_trackBar_масштаб_;
            o_trackBar_масштаб.Maximum = vi_масштаб_максимум;
            o_trackBar_масштаб.Minimum = vi_масштаб_минимум;
            o_trackBar_масштаб.Value = vi_масштаб_максимум/2;
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

            
            vi_ширина_рисунка = o_pictureBox.Width;          
            vi_высота_рисунка = o_pictureBox.Height;
            o_pictureBox.MouseDown += e_pictureBox_MouseDown;
            o_pictureBox.MouseUp += e_pictureBox_MouseUp;


            o_rectangle = new Rectangle();
            
            o_pictureBox.MouseDown += o_pictureBox_MouseDown;

        }

        private void o_pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (vb_режим_приближения == false)
                return;
            var relativePoint = o_pictureBox.PointToClient(Cursor.Position);
            o_rectangle.X = relativePoint.X;
            o_rectangle.Y = relativePoint.Y;
            s_точка_старт.активна = true;
            o_pictureBox.MouseMove += e_pictureBox_MouseMove;
        }

        private void e_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)// миссия стартанула
            {
                var relativePoint = o_pictureBox.PointToClient(Cursor.Position);
                o_rectangle.Width = relativePoint.X - o_rectangle.X;
                o_rectangle.Height = relativePoint.Y - o_rectangle.Y;
                o_рисунок_1.fv_прямоугольник(o_rectangle, o_кисть_для_заливки);
                o_pictureBox.Image = o_рисунок_1.картинка;
                return;
            }

            if (s_точка_старт.активна == true)
            {
                s_точка_старт.активна = false;
                vi_стартовое_смещение = o_рисунок_1.смещение_по_координате(o_rectangle.X);
                int temp = o_рисунок_1.смещение_по_координате(o_rectangle.X+ o_rectangle.Width) - vi_стартовое_смещение;
                vi_количество_баров = temp;
                fv_рисуем();
                o_pictureBox.MouseMove -= e_pictureBox_MouseMove;
                // vb_режим_приближения = false;
                // o_сheckBox_приближение.Checked = false;
            }
        }


        private void e_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            o_toolTip.Active = false;
        }

        private void e_pictureBox_DoubleClick(object sender, EventArgs e)// подсказка
        {
           // int координата_Х = Cursor.Position.X - 4; // 4 - смещение для определения более точного мышки есть расхождение между в определении форма не прилегает к краю на 4 пикселя
            var relativePoint = o_pictureBox.PointToClient(Cursor.Position);
            string time = o_рисунок_1.time_tool_tip(relativePoint.X);
            string minimum = o_рисунок_1.minimum_tool_tip(relativePoint.X);
            string maximum = o_рисунок_1.maximum_tool_tip(relativePoint.X);
            string open = o_рисунок_1.open_tool_tip(relativePoint.X);
            string close = o_рисунок_1.close_tool_tip(relativePoint.X);
            string prise = o_рисунок_1.prise_tool_tip(relativePoint.Y);
            int смещение = o_котировки_1.смещение(DateTime.Parse(time));


            string total_tool_tip =
                time + "\n" +
                open + " open" + "\n" +
                maximum + " максимум" + "\n" +
                minimum + " минимум" + "\n" +
                close + " close" + "\n" +
                смещение + " бар по счету" + "\n" +
                prise + " цена_в_этом_месте";


            o_toolTip.AutoPopDelay = 90000;
            o_toolTip.InitialDelay = 50;
            o_toolTip.ReshowDelay = 50;
            // o_подсказка.AutomaticDelay = 50;
            o_toolTip.Active = true;
            o_toolTip.SetToolTip(o_pictureBox, total_tool_tip);
          
        }

        private void e_CheckBox_показывать_выходные_CheckedChanged(object sender, EventArgs e)
        {
            vb_показывать_выходные = o_CheckBox_показывать_выходные.Checked;
            fv_рисуем();
        }

        private void e_сheckBox_приближение_CheckedChanged(object sender, EventArgs e)
        {
            if (o_сheckBox_приближение.Checked)
                vb_режим_приближения = true;
            else
                vb_режим_приближения = false;
        }

        private void e_pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (vb_режим_приближения)
                return;
            vb_мышь_была_внизу = true;
            vi_позиция_X1 = PictureBox.MousePosition.X;

        }

        private void e_pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (vb_режим_приближения)
                return;
            if (vb_мышь_была_внизу == false)
                return;
            vi_позиция_X2 = PictureBox.MousePosition.X;               
            fv_перемотка_мышкой(vi_позиция_X1, vi_позиция_X2);
        }

        void fv_перемотка_мышкой(int vi_позиция_X1, int vi_позиция_X2)
        {
            double temp = (vi_позиция_X1 - vi_позиция_X2) / o_рисунок_1.Ширина_бара();
            if (temp < 1)
                if (temp > -1)
                    return;
            if (double.IsInfinity(temp))
                return;
            if (double.IsInfinity(-temp))
                return;
            if (temp == double.NaN)
                return;
            int смещение = Convert.ToInt32(temp);
            vi_стартовое_смещение = vi_стартовое_смещение + смещение;
            vb_мышь_была_внизу = false;
            fv_рисуем();
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
            if (o_comboBox_цвета.SelectedIndex == 4)            
                 o_кисть_для_заливки.Color = o_colorDialog.Color;
            fv_рисуем();
        }

        private void e_кнопка_перемотка_вперед_Click(object sender, EventArgs e)
        {
            int temp = Convert.ToInt32(vi_стартовое_смещение + vi_количество_баров * vd_сдвиг_при_перемотке);
            if (temp == vi_стартовое_смещение)
                vi_стартовое_смещение++;
            else
            vi_стартовое_смещение = temp;
            fv_рисуем();
        }

        private void e_кнопка_перемотка_назад_Click(object sender, EventArgs e)
        {
            int temp = Convert.ToInt32(vi_стартовое_смещение - vi_количество_баров * vd_сдвиг_при_перемотке);
            if (temp == vi_стартовое_смещение)
                vi_стартовое_смещение--;
            else
                vi_стартовое_смещение = temp;

            fv_рисуем();
        }

        private void e_trackBar_масштаб_Scroll(object sender, EventArgs e)
        {
            vi_масштаб = o_trackBar_масштаб.Value - vi_масштаб_максимум/2;
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
            int temp4 = Convert.ToInt32(temp2 / temp3) + vi_масштаб_максимум/2;
            if (temp4 > vi_масштаб_максимум)
                temp4 = vi_масштаб_максимум;
            o_trackBar_масштаб.Value = temp4;
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

            o_pictureBox.Image = o_рисунок_1.картинка;
           
        }







    }
}
