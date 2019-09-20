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
    public partial class нормализация_котировок : Form
    {
        public библиотека.котировки котировки_1 = new библиотека.котировки();
        public библиотека.котировки котировки_2 = new библиотека.котировки();
        int j_стартовое_смещение;//
        int j_количество_баров;
        int таймфрейм;

        public нормализация_котировок()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)// загрузка 1
        {
        
            openFileDialog1.InitialDirectory = @"C:\Users\Владислав\Dropbox\С#\history\EURUSD";
            openFileDialog1.ShowDialog();
            string файл = openFileDialog1.SafeFileName;
            string путь = openFileDialog1.FileName;
            bool успешная_загрузка = котировки_1.загрузка_котировок(путь);
            j_стартовое_смещение = 0;
            j_количество_баров = 300;
            таймфрейм = котировки_1.time_frame();
            котировки_1.создание_котировок_без_временных_пробелов(таймфрейм);
            int t = 0;


        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)// файл диалог
        {

        }
    }
    class Visual:Form1
    {

    }

}
