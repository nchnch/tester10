using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace b_формы
{
    public partial class Form1 : Form
    {
        b_формы.расчеты_для_рисунка расчеты1 = new b_формы.расчеты_для_рисунка();

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);// для прозрачности
            // toolTip1 = new ToolTip(components);
            расчеты1.fv_инизиализация_обектов_формы
             (
               ref button1,
               ref comboBox1,// периоды
               ref textBox1,// баров
               ref dateTimePicker1,
               ref dateTimePicker2,
               ref trackBar1,// масштаб
               ref button2,// назад
               ref button3,// вперед
               ref comboBox2,// цвета
               ref colorDialog1,
               ref checkBox1,// приближение
               ref checkBox2,// показывать выходные
               ref openFileDialog1,
               ref pictureBox1
             //  ref textBox3
             );

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
