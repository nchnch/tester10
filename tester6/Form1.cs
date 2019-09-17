﻿// сделать нарезку любых баров
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace tester6
{
    public partial class Form1 : Form 
    {
       

        DateTime старт;
        DateTime финиш;
        Color цвет_котировок;
        int периоды;
        int количество_баров;
        Color цвет_фона;
        Color цвет_разделителей_периодов;
        double сдвиг;
        public библиотека.котировки котировки_1 = new библиотека.котировки();
        public библиотека.рисунок рисунок_1;
        ToolTip tt;
        int масштаб;


        public Form1()
        {          
            InitializeComponent();
            {
                tt = new ToolTip(components);
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm";
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd.MM.yyyy HH:mm";
                цвет_котировок = Color.FromArgb(255, 122, 244, 0); 
                цвет_фона = Color.Black;
                цвет_разделителей_периодов = Color.FromArgb(255, 55, 55, 55);
                периоды = 1440;
                сдвиг = 0.2;

            }
            colorDialog1.FullOpen = true;
        }
        // кнопки ###################################################################################

        private void button1_Click(object sender, EventArgs e) // кнопка загрузить
        {
            openFileDialog1.InitialDirectory = @"C:\Users\Владислав\Dropbox\С#\history\EURUSD\EURUSD240";
            openFileDialog1.ShowDialog();
            string файл = openFileDialog1.SafeFileName;
            string путь = openFileDialog1.FileName ;
            bool успешная_загрузка = котировки_1.загрузка_котировок(путь);
            if (успешная_загрузка)
            {
                MessageBox.Show("котировки загружены");
            }
            else
            {
                MessageBox.Show("котировки не удалось загрузить");
            }
            сбор_данных();
            рисунок_1 = new библиотека.рисунок(pictureBox1.Width, pictureBox1.Height, цвет_котировок);
        }

        private void button2_Click(object sender, EventArgs e)// показать котировки
        {

            рисуем();
        }

        private void button4_Click(object sender, EventArgs e) // перемотка вперед
        {
            перемотка_вперед();
        }

        private void button3_Click(object sender, EventArgs e) // перемотка назад
        {
            сбор_данных();
            int смещение = Convert.ToInt32(котировки_1.смещение(старт) - сдвиг * количество_баров);
            if (смещение < 0) return;
            dateTimePicker1.Value = котировки_1.Q[смещение].time;
            рисунок_1.отрисовка(количество_баров, смещение, ref котировки_1.Q, цвет_котировок, периоды, цвет_фона,
                цвет_разделителей_периодов);
            pictureBox1.Image = рисунок_1.картинка;
        }

        private void button5_Click(object sender, EventArgs e)// цвет фона
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            цвет_фона = colorDialog1.Color;
            рисуем();
        }

        private void button6_Click(object sender, EventArgs e)// цвет баров
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            цвет_котировок = colorDialog1.Color;
            рисуем();
        }

        private void button7_Click(object sender, EventArgs e)//цвет разделителей

        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            цвет_разделителей_периодов = colorDialog1.Color;
            рисуем();
        }

        // функции #################################################################################

        private void рисуем()
        {
            сбор_данных();
            int смещение = котировки_1.смещение(старт);
            рисунок_1.отрисовка(количество_баров, смещение, ref котировки_1.Q, цвет_котировок, периоды,цвет_фона,
                цвет_разделителей_периодов);
            pictureBox1.Image = рисунок_1.картинка;
        }

        private void сбор_данных()
        {

            старт = dateTimePicker1.Value;
            финиш = dateTimePicker2.Value;
            масштаб = trackBar1.Value;
            string периоды_string = comboBox2.Text;

            if (периоды_string == "час") { периоды = 60; }
            if (периоды_string == "4 часа") { периоды = 240; }
            if (периоды_string == "сутки") { периоды = 1440; }
            // if (периоды_string == "неделя") { периоды = 10800; }
            // if (периоды_string == "месяц") { периоды = ; }
            // if (периоды_string == "год") { периоды = Convert.ToInt32(периоды_string); }

            количество_баров = Convert.ToInt32(textBox1.Text);
        }

        void перемотка_вперед()
        {
            сбор_данных();
            int смещение = Convert.ToInt32(котировки_1.смещение(старт));
            смещение = Convert.ToInt32(смещение + сдвиг * количество_баров);
            if (смещение >= котировки_1.Q.Count) return;
            dateTimePicker1.Value = котировки_1.Q[смещение].time;
            рисунок_1.отрисовка(количество_баров, смещение, ref котировки_1.Q, цвет_котировок, периоды, цвет_фона,
                цвет_разделителей_периодов);
            pictureBox1.Image = рисунок_1.картинка;
        }

        // выбор через формы и комбобоксы #########################################################
        
        public void dateTimePicker1_ValueChanged(object sender, EventArgs e)// время старт
        {
            старт = dateTimePicker1.Value;
            int смещение = котировки_1.смещение(старт) + количество_баров;
            if (смещение >= котировки_1.Q.Count) return;
            dateTimePicker2.Value = котировки_1.Q[смещение].time;
            финиш = dateTimePicker2.Value;
            рисуем();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)// выбор сделан щелчек мыши последний
        {
            финиш = dateTimePicker2.Value;
            int смещение_финиш = котировки_1.смещение(финиш);
            int смещение_старт = котировки_1.смещение(старт);
            количество_баров = смещение_финиш - смещение_старт;
            textBox1.Text = Convert.ToString(количество_баров);
            рисуем();
        }

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)// расделитель периодов
        {
            рисуем();
        }

        public void textBox1_TextChanged(object sender, EventArgs e)// количество баров
        {
            // при изменении поменять даты 
        }

        public void trackBar1_Scroll(object sender, EventArgs e)// масштаб
        {

            // получить изменения масштаба
            int temp  = trackBar1.Value - масштаб; 
            int количество_баров_новое = Convert.ToInt32(количество_баров * Math.Pow(1.4, temp));
            int расширение_по_датам = (количество_баров_новое - количество_баров) / 2;
            int смещение = котировки_1.смещение(старт) - расширение_по_датам;
            if (смещение < 0) смещение = 0;
            if (смещение >= котировки_1.Q.Count) смещение = котировки_1.Q.Count - 100;
            старт = котировки_1.Q[смещение].time;
            dateTimePicker1.Value = старт;
            int смещение_финиш = смещение + количество_баров_новое;
            if (смещение_финиш >= котировки_1.Q.Count) смещение_финиш = котировки_1.Q.Count - 1;
            финиш = котировки_1.Q[смещение_финиш].time;
            dateTimePicker2.Value = финиш;
            количество_баров = количество_баров_новое;
            textBox1.Text = Convert.ToString(количество_баров);
            рисуем();
        }

        // движения мышки ########################################################################

        private void pictureBox1_DoubleClick(object sender, EventArgs e) //двойной щелчек мыши на баре подсказка
        {
            int координата_Х = Cursor.Position.X - 4; // 4 - смещение для определения более точного мышки есть расхождение между в определении форма не прилегает к краю на 4 пикселя
            string time = рисунок_1.time_tool_tip(координата_Х, ref котировки_1.Q);
            string minimum = рисунок_1.minimum_tool_tip(координата_Х, ref котировки_1.Q);
            string maximum = рисунок_1.maximum_tool_tip(координата_Х, ref котировки_1.Q);
            string open = рисунок_1.open_tool_tip(координата_Х, ref котировки_1.Q);
            string close = рисунок_1.close_tool_tip(координата_Х, ref котировки_1.Q);
            int смещение = котировки_1.смещение(DateTime.Parse(time));


            string total_tool_tip =
                time + "\n" +
                open + " open" + "\n" +
                maximum + " максимум" + "\n" +
                minimum + " минимум" + "\n" +
                close + " close" + "\n"+
                смещение + " смещение";


            tt.AutoPopDelay = 90000;
            tt.InitialDelay = 50;
            tt.ReshowDelay = 50;
            // tt.AutomaticDelay = 50;
            tt.SetToolTip(pictureBox1, total_tool_tip);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e) // уход мышки с поля картинки
        {
            tt.Active = false;
        }

       

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty) return;
            if (Convert.ToInt32(textBox1.Text) == 0) return;
            количество_баров = Convert.ToInt32(textBox1.Text);
            int смещение = котировки_1.смещение(старт) + количество_баров;
            if (смещение >= котировки_1.Q.Count) return;
            dateTimePicker2.Value = котировки_1.Q[смещение].time;
            финиш = dateTimePicker2.Value;
            рисуем();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DateTime g = DateTime.Parse("02.02.2005 17:00");
            int a = котировки_1.смещение(g);
            DateTime time = котировки_1.Q.ElementAt(a).time; 
        }
    }
}