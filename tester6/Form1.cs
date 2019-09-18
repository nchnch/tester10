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


namespace tester6
{
    public partial class Form1 : Form 
    {
       

        DateTime старт;
        DateTime финиш;
        Color цвет_котировок;
        int периоды;
        int j_количество_баров;
        Color цвет_фона;
        Color цвет_разделителей_периодов;
        double сдвиг;
        public библиотека.котировки котировки_1 = new библиотека.котировки();
        public библиотека.рисунок рисунок_1;
        ToolTip tt;
        int j_масштаб;
        // для пеермотки мышкой
        bool мышь_вниз;
        Point j_точка1;
        Point j_точка2;
        int j_стартовое_смещение;//
        int j_базовое_количество_баров;// для масштаба стартового
        double j_шаг_масштаба;



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
                j_шаг_масштаба = 2;

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
            f_рисуем();
        }

        private void button2_Click(object sender, EventArgs e)// показать котировки
        {

            f_рисуем();
        }

        private void button4_Click(object sender, EventArgs e) // перемотка вперед
        {
            j_стартовое_смещение = Convert.ToInt32(j_стартовое_смещение + j_количество_баров * сдвиг);
            f_рисуем();
        }

        private void button3_Click(object sender, EventArgs e) // перемотка назад
        {

            j_стартовое_смещение = Convert.ToInt32(j_стартовое_смещение - j_количество_баров * сдвиг);
            f_рисуем();
        }

        private void button5_Click(object sender, EventArgs e)// цвет фона
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            цвет_фона = colorDialog1.Color;
            f_рисуем();
        }

        private void button6_Click(object sender, EventArgs e)// цвет баров
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            цвет_котировок = colorDialog1.Color;
            f_рисуем();
        }

        private void button7_Click(object sender, EventArgs e)//цвет разделителей

        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            цвет_разделителей_периодов = colorDialog1.Color;
            f_рисуем();
        }

        // функции #################################################################################



        void перемотка_вперед(int смещение)
        {
            j_стартовое_смещение = Convert.ToInt32(j_стартовое_смещение + j_количество_баров * сдвиг);
            f_рисуем();
        }

 


        void перемотка_мышкой(Point j_точка_1,Point j_точка_2)
        {
           // перемотка_назад(10);
            // if (j_точка_1.X>j_точка_2.X)// переметка в лево
            ////  {
            //      double temp =(j_точка_1.X - j_точка2.X) / рисунок_1.Ширина_бара();
            //      int смещение = Convert.ToInt32(temp);
            //      перемотка_назад(смещение);
            //  }

            if (j_точка_1.X < j_точка_2.X)// переметка в право
            {

            }
        }


        // выбор через формы и комбобоксы #########################################################
        
        public void dateTimePicker1_ValueChanged(object sender, EventArgs e)// время старт
        {
            старт = dateTimePicker1.Value;
            j_стартовое_смещение = котировки_1.смещение(старт);
            f_рисуем();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)// выбор сделан щелчек мыши последний
        {
            DateTime финиш = dateTimePicker2.Value;

            int  смещение_финиш= котировки_1.смещение(финиш);           
            j_количество_баров = смещение_финиш - j_стартовое_смещение;
            f_рисуем();

        }

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)// расделитель периодов
        {
            f_рисуем();
        }

        public void trackBar1_Scroll(object sender, EventArgs e)// масштаб
        {
            j_масштаб   = trackBar1.Value;
            int j_новое_количество_баров = Convert.ToInt32(j_базовое_количество_баров * Math.Pow(j_шаг_масштаба, j_масштаб));
            j_стартовое_смещение = j_стартовое_смещение - (j_новое_количество_баров - j_количество_баров) / 2;
            f_рисуем();
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
            j_количество_баров = Convert.ToInt32(textBox1.Text);
            f_рисуем();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DateTime g = DateTime.Parse("02.02.2005 17:00");
            int a = котировки_1.смещение(g);
            DateTime time = котировки_1.Q.ElementAt(a).time; 
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            мышь_вниз = true;
           j_точка1.X = Form1.MousePosition.X;
           j_точка1.Y = Form1.MousePosition.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (мышь_вниз)
            {
               j_точка2.X = Form1.MousePosition.X;
               j_точка2.Y = Form1.MousePosition.Y;
                перемотка_мышкой(j_точка1,j_точка2);
            }
            else
                мышь_вниз = false;
        }

        void f_рисуем()
        {
            string периоды_string = comboBox2.Text;
            if (периоды_string == "час") { периоды = 60; }
            if (периоды_string == "4 часа") { периоды = 240; }
            if (периоды_string == "сутки") { периоды = 1440; }

            // контроль коррекности 
            if (j_количество_баров> котировки_1.Q.Count)
            {
                j_количество_баров = котировки_1.Q.Count;
            }

            if (j_стартовое_смещение<0)
            {
                j_стартовое_смещение = 0;
            }

            if (j_стартовое_смещение+j_количество_баров> котировки_1.Q.Count-1)
            {
                j_стартовое_смещение = котировки_1.Q.Count - 1 - j_количество_баров;
            }

            // корреция идет на основе двух переменных смещения и количества свечей
            textBox1.Text = Convert.ToString(j_количество_баров);
            dateTimePicker1.Value = котировки_1.Q.ElementAt(j_стартовое_смещение).time;
            dateTimePicker2.Value = котировки_1.Q.ElementAt(j_стартовое_смещение+j_количество_баров).time;
            double temp = j_количество_баров / j_базовое_количество_баров;
            trackBar1.Value = Convert.ToInt32(Math.Log10(temp) / Math.Log10(j_шаг_масштаба));
            рисунок_1.отрисовка(j_количество_баров, j_стартовое_смещение, ref котировки_1.Q, цвет_котировок, периоды, цвет_фона,
                цвет_разделителей_периодов);
            pictureBox1.Image = рисунок_1.картинка;
        }


    }
    // новые функции
  
}
public struct Point
{
    public int X;
    public int Y;
}