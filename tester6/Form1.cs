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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace tester6
{
    public partial class Form1 : Form
    {
        DateTime vd_старт;
        Color vc_цвет_котировок;
        int vi_периоды;
        int vi_количество_баров;
        Color vc_цвет_фона;
        Color vc_цвет_выходных;
        Color vc_цвет_разделителей_периодов;
        double vd_сдвиг;
        public библиотека.котировки o_котировки_1 = new библиотека.котировки();
        public библиотека.рисунок o_рисунок_1;
        ToolTip o_подсказка;
        int vi_масштаб;
        // для пеермотки мышкой
        bool vb_мышь_вниз;
        int vi_стартовое_смещение;//
        int vi_базовое_количество_баров;// для масштаба стартового
        double vd_шаг_масштаба;
        int vi_таймфрейм;
        // для выделения и перемотки
        bool vb_режим_выделения;
        библиотека.Point s_точка_старт;
        библиотека.Point s_точка_финиш;
        bool vb_показывать_выходные;
        List<Quotes> l_Q;
        библиотека.расчеты_для_рисунка o_расчеты1 = new библиотека.расчеты_для_рисунка();

        public Form1()
        {
            InitializeComponent();
            {
                o_подсказка = new ToolTip(components);
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm";
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd.MM.yyyy HH:mm";
                vc_цвет_котировок = Color.FromArgb(255, 122, 244, 0);
                vc_цвет_выходных = Color.FromArgb(160, 55, 55, 55); ;
                vc_цвет_фона = Color.Black;
                vc_цвет_разделителей_периодов = Color.FromArgb(255, 55, 55, 55);
                vi_периоды = 1440;
                vd_сдвиг = 0.2;
                vd_шаг_масштаба = 2;
                vi_базовое_количество_баров = 300;
                vi_масштаб = 10;
                vb_режим_выделения = false;
                checkBox2.Checked = true;
                vb_показывать_выходные = true;


                o_расчеты1.fv_инизиализация_обектов_формы
                    (
                        ref button1,
                        ref openFileDialog1,
                        ref o_котировки_1,
                        vc_цвет_котировок,
                        pictureBox1.Width,
                        pictureBox1.Height
                    );
                //o_расчеты1.fv_инизиализация_обектов_формы(ref button1, ref openFileDialog1);


            }
            colorDialog1.FullOpen = true;
        }
        // доступ


        // кнопки ###################################################################################

        private void button1_Click(object sender, EventArgs e) // кнопка загрузить
        {


            openFileDialog1.InitialDirectory = @"C:\Users\Владислав\Dropbox\С#\history\EURUSD\EURUSD240";
            openFileDialog1.ShowDialog();
            string файл = openFileDialog1.SafeFileName;
            string путь = openFileDialog1.FileName;
            o_котировки_1.загрузка_котировок(путь);
            vi_стартовое_смещение = 0;
            vi_количество_баров = 300;
            vi_таймфрейм = o_котировки_1.time_frame();
            l_Q = o_котировки_1.Q_();
            o_рисунок_1 = new библиотека.рисунок(pictureBox1.Width, pictureBox1.Height, vc_цвет_котировок, l_Q);

            f_рисуем();
        }

        private void button2_Click(object sender, EventArgs e)// показать котировки
        {

            f_рисуем();
        }

        private void button4_Click(object sender, EventArgs e) // перемотка вперед
        {
            vi_стартовое_смещение = Convert.ToInt32(vi_стартовое_смещение + vi_количество_баров * vd_сдвиг);
            f_рисуем();
        }

        private void button3_Click(object sender, EventArgs e) // перемотка назад
        {

            vi_стартовое_смещение = Convert.ToInt32(vi_стартовое_смещение - vi_количество_баров * vd_сдвиг);
            f_рисуем();
        }

        private void button5_Click(object sender, EventArgs e)// цвет фона
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            vc_цвет_фона = colorDialog1.Color;
            f_рисуем();
        }

        private void button6_Click(object sender, EventArgs e)// цвет баров
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            vc_цвет_котировок = colorDialog1.Color;
            f_рисуем();
        }

        private void button7_Click(object sender, EventArgs e)//цвет разделителей

        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            vc_цвет_разделителей_периодов = colorDialog1.Color;
            f_рисуем();
        }



        // функции #################################################################################
        void f_рисуем()
        {
            string vi_периоды_string = comboBox2.Text;
            if (vi_периоды_string == "час") { vi_периоды = 60; }
            if (vi_периоды_string == "4 часа") { vi_периоды = 240; }
            if (vi_периоды_string == "сутки") { vi_периоды = 1440; }

            // контроль коррекности 
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

            textBox1.Text = Convert.ToString(vi_количество_баров);
            dateTimePicker1.Value = l_Q.ElementAt(vi_стартовое_смещение).time;
            dateTimePicker2.Value = l_Q.ElementAt(vi_стартовое_смещение + vi_количество_баров).time;
            double temp = Convert.ToDouble(vi_количество_баров) / vi_базовое_количество_баров;
            double temp2 = Math.Log10(temp);
            double temp3 = Math.Log10(vd_шаг_масштаба);
            trackBar1.Value = Convert.ToInt32(temp2 / temp3) + 10;
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



        void перемотка_мышкой(библиотека.Point j_точка_1, библиотека.Point j_точка_2)
        {
            double temp = (j_точка_1.X - s_точка_финиш.X) / o_рисунок_1.Ширина_бара();
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
            f_рисуем();
        }

        // выбор через формы и комбобоксы #########################################################

        public void dateTimePicker1_ValueChanged(object sender, EventArgs e)// время старт
        {
            vd_старт = dateTimePicker1.Value;
            vi_стартовое_смещение = o_котировки_1.смещение(vd_старт);
            f_рисуем();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)// выбор сделан щелчек мыши последний
        {
            DateTime финиш = dateTimePicker2.Value;

            int смещение_финиш = o_котировки_1.смещение(финиш);
            vi_количество_баров = смещение_финиш - vi_стартовое_смещение;
            f_рисуем();

        }

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)// расделитель периодов
        {
            f_рисуем();
        }

        public void trackBar1_Scroll(object sender, EventArgs e)// масштаб
        {
            vi_масштаб = trackBar1.Value - 10;
            double temp = Math.Pow(vd_шаг_масштаба, vi_масштаб);
            int j_новое_количество_баров = Convert.ToInt32(vi_базовое_количество_баров * temp);
            vi_стартовое_смещение = vi_стартовое_смещение - (j_новое_количество_баров - vi_количество_баров) / 2;
            vi_количество_баров = j_новое_количество_баров;
            f_рисуем();
        }

        // движения мышки ########################################################################

        private void pictureBox1_DoubleClick(object sender, EventArgs e) //двойной щелчек мыши на баре подсказка
        {
            int координата_Х = Cursor.Position.X - 4; // 4 - смещение для определения более точного мышки есть расхождение между в определении форма не прилегает к краю на 4 пикселя
            string time = o_рисунок_1.time_tool_tip(координата_Х);
            string minimum = o_рисунок_1.minimum_tool_tip(координата_Х);
            string maximum = o_рисунок_1.maximum_tool_tip(координата_Х);
            string open = o_рисунок_1.open_tool_tip(координата_Х);
            string close = o_рисунок_1.close_tool_tip(координата_Х);
            int смещение = o_котировки_1.смещение(DateTime.Parse(time));


            string total_tool_tip =
                time + "\n" +
                open + " open" + "\n" +
                maximum + " максимум" + "\n" +
                minimum + " минимум" + "\n" +
                close + " close" + "\n" +
                смещение + " смещение";


            o_подсказка.AutoPopDelay = 90000;
            o_подсказка.InitialDelay = 50;
            o_подсказка.ReshowDelay = 50;
            // o_подсказка.AutomaticDelay = 50;
            o_подсказка.SetToolTip(pictureBox1, total_tool_tip);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e) // уход мышки с поля картинки
        {
            o_подсказка.Active = false;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty) return;
            if (Convert.ToInt32(textBox1.Text) == 0) return;
            vi_количество_баров = Convert.ToInt32(textBox1.Text);
            f_рисуем();
        }

        private void button8_Click(object sender, EventArgs e)// тест смещения по дате 
        {
            // DateTime g = DateTime.Parse("02.02.2005 17:00");
            //  int a = o_котировки_1.смещение(g);
            /// DateTime time = o_котировки_1.Q.ElementAt(a).time;
          // test();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (vb_режим_выделения)
                return;
            vb_мышь_вниз = true;
            s_точка_старт.X = Form1.MousePosition.X;
            s_точка_старт.Y = Form1.MousePosition.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (vb_режим_выделения)
                return;
            if (vb_мышь_вниз)
            {
                s_точка_финиш.X = Form1.MousePosition.X;
                s_точка_финиш.Y = Form1.MousePosition.Y;
                перемотка_мышкой(s_точка_старт, s_точка_финиш);
            }
            else
                vb_мышь_вниз = false;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                vb_режим_выделения = true;
            if (checkBox1.Checked == false)
                vb_режим_выделения = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (vb_режим_выделения == false)
                return;

            if (Form1.MouseButtons == MouseButtons.Left)// миссия стартанула
            {
                if (s_точка_старт.активна == false)
                {
                    // какие то операторы
                    s_точка_старт.активна = true;
                    s_точка_старт.X = Cursor.Position.X - 8;
                    s_точка_старт.Y = Cursor.Position.Y - 122;
                    return;
                }
                else
                {
                    //какие то операторы
                    s_точка_финиш.X = Cursor.Position.X - 8;
                    s_точка_финиш.Y = Cursor.Position.Y - 122;
                    int err = 1;
                    o_рисунок_1.f_рисование_прямоугольника(s_точка_старт, s_точка_финиш);
                    pictureBox1.Image = o_рисунок_1.картинка;
                }

            }

            if (Form1.MouseButtons != MouseButtons.Left)// миссия завершена либо не начиналась
                if (s_точка_старт.активна == true)
                {
                    s_точка_старт.активна = false;
                    vi_стартовое_смещение = o_рисунок_1.смещение_по_координате(s_точка_старт.X);
                    int temp = o_рисунок_1.смещение_по_координате(s_точка_финиш.X) - vi_стартовое_смещение;
                    vi_количество_баров = temp;
                    f_рисуем();
                }
        }

        private void checkBox2_MouseClick(object sender, MouseEventArgs e)
        {
            vb_показывать_выходные = checkBox2.Checked;
            f_рисуем();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            нормализация_котировок F2 = new нормализация_котировок();
            F2.Show();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }

}

