using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arhitectura_Retelei_N
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        public NumericUpDown numUDo;
        public NumericUpDown numUDg;
        public NumericUpDown numUDa;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            panel1.Controls.Clear();
         
            if (comboBox2.Text == "Treapta")
            {

                numUDo = new NumericUpDown();
                numUDo.Size = new System.Drawing.Size(65, 30);
                numUDo.Top = 30;
                numUDo.Left = 25;
                numUDo.Name = "NumUDo";
                numUDo.Maximum = 1000;
                numUDo.Minimum = -1000;
                numUDo.DecimalPlaces = 2;
                numUDo.Increment = 0.01m;
                panel1.Controls.Add(numUDo);
               // numUDo.ValueChanged += new EventHandler(this.numActBi_ValueChanged);

                printLabelO();

               
            }
            else if (comboBox2.Text == "Sigmoidala")
            {
                numUDo = new NumericUpDown();
                numUDo.Size = new System.Drawing.Size(65, 30);
                numUDo.Top = 30;
                numUDo.Left = 25;
                numUDo.Name = "NumUDo";
                numUDo.Maximum = 1000;
                numUDo.Minimum = -1000;
                numUDo.DecimalPlaces = 2;
                numUDo.Increment = 0.01m;
                panel1.Controls.Add(numUDo);
              //  numUDo.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelO();

                numUDg = new NumericUpDown();
                numUDg.Size = new System.Drawing.Size(65, 30);
                numUDg.Top = 30;
                numUDg.Left = 130;
                numUDg.Name = "NumUDg";
                numUDg.Maximum = 1000;
                numUDg.Minimum = -1000;
                numUDg.Value = 1;
                panel1.Controls.Add(numUDg);
               // numUDg.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelG();

                
            }
            else if (comboBox2.Text == "Signum")
            {

                numUDo = new NumericUpDown();
                numUDo.Size = new System.Drawing.Size(65, 30);
                numUDo.Top = 30;
                numUDo.Left = 25;
                numUDo.Name = "NumUDo";
                numUDo.Maximum = 1000;
                numUDo.Minimum = -1000;
                numUDo.DecimalPlaces = 2;
                numUDo.Increment = 0.01m;
                panel1.Controls.Add(numUDo);
               // numUDo.ValueChanged += new EventHandler(this.numActBi_ValueChanged);
                printLabelO();
              
            }
            else if (comboBox2.Text == "Tangenta H")
            {
                numUDo = new NumericUpDown();
                numUDo.Size = new System.Drawing.Size(65, 30);
                numUDo.Top = 30;
                numUDo.Left = 25;
                numUDo.Name = "NumUDo";
                numUDo.Maximum = 1000;
                numUDo.Minimum = -1000;
                numUDo.DecimalPlaces = 2;
                numUDo.Increment = 0.01m;
                panel1.Controls.Add(numUDo);
                //numUDo.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelO();

                numUDg = new NumericUpDown();
                numUDg.Size = new System.Drawing.Size(65, 30);
                numUDg.Top = 30;
                numUDg.Left = 130;
                numUDg.Name = "NumUDg";
                numUDg.Maximum = 1000;
                numUDg.Minimum = -1000;
                numUDg.Value = 1;
                panel1.Controls.Add(numUDg);
              //  numUDg.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelG();

             

            }
            else if (comboBox2.Text == "Rampa")
            {

                numUDo = new NumericUpDown();
                numUDo.Size = new System.Drawing.Size(65, 30);
                numUDo.Top = 30;
                numUDo.Left = 25;
                numUDo.Name = "NumUDo";
                numUDo.Maximum = 1000;
                numUDo.Minimum = -1000;
                numUDo.DecimalPlaces = 3;
                numUDo.Increment = 0.001m;
                panel1.Controls.Add(numUDo);
               // numUDo.ValueChanged += new EventHandler(this.numActRampa_ValueChanged);
                printLabelO();

                numUDa = new NumericUpDown();
                numUDa.Size = new System.Drawing.Size(65, 30);
                numUDa.Top = 30;
                numUDa.Left = 130;
                numUDa.Name = "NumUDa";
                numUDa.Maximum = 1000;
                numUDa.Minimum = 1;
                numUDa.Value = 1;
                panel1.Controls.Add(numUDa);
               // numUDa.ValueChanged += new EventHandler(this.numActRampa_ValueChanged);

                printLabelA();

              
            }
        }
             Label lO;
            Label lG;
            Label lA;
            public void printLabelO()
            {
                lO = new Label();
                lO.Size = new System.Drawing.Size(50, 30);
                lO.Top = 30;
                lO.Left = 4;
                lO.BackColor = Color.Transparent;
                lO.Text = "θ:";
                lO.Name = "lO";
                panel1.Controls.Add(lO);
            }
            public void printLabelG()
            {
                lG = new Label();
                lG.Size = new System.Drawing.Size(50, 30);
                lG.Top = 30;
                lG.Left = 105;
                lG.BackColor = Color.Transparent;
                lG.Text = "g:";
                lG.Name = "lG";
                panel1.Controls.Add(lG);
            }
            public void printLabelA()
            {
                lA = new Label();
                lA.Size = new System.Drawing.Size(50, 30);
                lA.Top = 30;
                lA.Left = 105;
                lA.BackColor = Color.Transparent;
                lA.Text = "a:";
                lA.Name = "lA";
                panel1.Controls.Add(lA);
            }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Hide();
           
        }
    }
}
