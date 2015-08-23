using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StockVentas
{
    public partial class reloj : Form
    {

        System.Threading.Thread newThread;

        public reloj()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString(); 
        }

        private void reloj_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newThread = new System.Threading.Thread(AMethod);
            newThread.Start();
        }

        void AMethod()
        {
            int x;
            for (x = 1; x < 20; x++)
            { 

            }
        }
    }
}
