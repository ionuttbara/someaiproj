using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronulArtificial
{
    public partial class Form1 : Form
    {
        public NumericUpDown[] listNumericUD1 = new NumericUpDown[1000];
        public NumericUpDown[] listNumericUD2 = new NumericUpDown[1000];
        public Label[] labelListIn = new Label[1000];
        public Label[] labelListW = new Label[1000];

        double[] INW = new double[1000];

        double sum =0;
        double pro =1;
        double maxim;
        double minim;
        double rez;
        public void resetareVector()
        {
            for (int i = 0; i < 1000; i++)
            {
                INW[i] = 0;
            }
        }
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex =0;
            comboBox2.SelectedIndex = 0;
            resetareVector();
            creareIntrari(Convert.ToInt32(numericUpDown1.Value));
            verificareC();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //modificari la evenimete de intrare
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            resetareVector();
            creareIntrari(Convert.ToInt32(numericUpDown1.Value));
            actiuneIntrare();
        }

        
        public void numericUD_ValueChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < Convert.ToInt32(numericUpDown1.Value); ++i)
            {
                INW[i] = Convert.ToDouble(listNumericUD1[i].Value) * Convert.ToDouble(listNumericUD2[i].Value);
            }
            actiuneIntrare();
        }
        public void creareIntrari(int nrIntrari)
        {
            for(int i= 0; i<nrIntrari;++i)
            {
                listNumericUD1[i] = new NumericUpDown();
                listNumericUD1[i].Size = new System.Drawing.Size(60, 30);
                listNumericUD1[i].Top = i * 30;
                listNumericUD1[i].Left = 45;
                listNumericUD1[i].Name = "NumUD1-" + i.ToString();
                listNumericUD1[i].Maximum = 1000;
                listNumericUD1[i].Minimum = -1000;
                listNumericUD1[i].DecimalPlaces = 2;
                listNumericUD1[i].Increment = 0.01m;
                panel1.Controls.Add(listNumericUD1[i]);

                listNumericUD1[i].ValueChanged += new EventHandler(this.numericUD_ValueChanged);

                labelListIn[i] = new Label();
                labelListIn[i].Size = new System.Drawing.Size(50, 30);
                labelListIn[i].Top = i * 30;
                labelListIn[i].Left = 3;
                labelListIn[i].BackColor = Color.Transparent;
                labelListIn[i].Text = "In" + i.ToString() + ":";
                labelListIn[i].Name = "LabeIN" + i.ToString();
                panel1.Controls.Add(labelListIn[i]);

                listNumericUD2[i] = new NumericUpDown();
                listNumericUD2[i].Size = new System.Drawing.Size(60, 30);
                listNumericUD2[i].Top = i * 30;
                listNumericUD2[i].Left = 155;
                listNumericUD2[i].Name = "NumUD2-" + i.ToString();
                listNumericUD2[i].Maximum = 1000;
                listNumericUD2[i].Minimum = -1000;
                listNumericUD2[i].DecimalPlaces = 2;
                listNumericUD2[i].Increment = 0.01m;
                panel1.Controls.Add(listNumericUD2[i]);

                listNumericUD2[i].ValueChanged += new EventHandler(this.numericUD_ValueChanged);

                labelListW[i] = new Label();
                labelListW[i].Size = new System.Drawing.Size(50, 30);
                labelListW[i].Top = i * 30;
                labelListW[i].Left = 110;
                labelListW[i].BackColor = Color.Transparent;
                labelListW[i].Text = "W" + i.ToString() + ":";
                labelListW[i].Name = "LabeW" + i.ToString();
                panel1.Controls.Add(labelListW[i]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            actiuneIntrare();
        }


        //functii Intrare
        public void actiuneIntrare()
        {
            if (comboBox1.Text == "Suma")
            {
                for (int i = 0; i < Convert.ToInt32(numericUpDown1.Value); ++i)
                {
                    sum += INW[i];
                }
                rez = sum;
                sum = 0;
            }
            else if (comboBox1.Text == "Produs")
            {
                for (int i = 0; i < Convert.ToInt32(numericUpDown1.Value); ++i)
                {
                    pro *= INW[i];
                }
                rez = pro;
                pro = 1;
            }
            else if (comboBox1.Text == "Maxim")
            {
                maxim = INW[0];
                maxim = INW.Max();
                rez = maxim;
            }
            else if (comboBox1.Text == "Minim")
            {
                minim = INW[0];
                for (int i=0; i< Convert.ToInt32(numericUpDown1.Value); ++i)
                {
                    if (minim > INW[i])
                        minim = INW[i];
                }
                rez = minim;
            }
           
            textBox1.Text = Convert.ToDecimal(rez).ToString();
            afisareRezAct();
            
        }




        //functii activare
       // double fact;
        double o=0;
        double g=1;
        double a=1;
        public int fTreapta(double x)
        {
            if (x >= o) return 1;
            else return 0;
           
        }
        public decimal fSigmoidala(double x)
        {
            return Convert.ToDecimal(1 / (1 + Math.Exp(-g * (x - o))));
        }
        public int fSignum(double x)
        {
            if (x >= o) return 1;
            else return -1;
        }
        public decimal fTanH(double x)
        {
            return Convert.ToDecimal(  ((Math.Exp(g * (x - o)) - Math.Exp(-g * (x - o)) )/(Math.Exp(g * (x - o)) + Math.Exp(-g * (x - o)))) );
        }
        public decimal fRampa(double x)
        {
            if (x-o > a) return 1;
            else if (x-o < -a) return -1;
            else return Convert.ToDecimal((x-o) / a);
        }
        /*
         Treapta
        Sigmoidala
        Signum
        Tangenta H
        Rampa
        */

        NumericUpDown numUDo;
        NumericUpDown numUDg;
        NumericUpDown numUDa;
        public void numActBi_ValueChanged(object sender, EventArgs e)
        {

            o = Convert.ToDouble(numUDo.Value);
            afisareRezAct();
        }
        public void numActRe_ValueChanged(object sender, EventArgs e)
        {

            o = Convert.ToDouble(numUDo.Value);
            g = Convert.ToDouble(numUDg.Value);
            afisareRezAct();
        }
        public void numActRampa_ValueChanged(object sender, EventArgs e)
        {

            o = Convert.ToDouble(numUDo.Value);
            a = Convert.ToDouble(numUDa.Value);
            if (a == 0) a = 0.01;
            afisareRezAct();
        }
        public void afisareRezAct()
        {
            if (comboBox2.Text == "Treapta")
            {
                textBox2.Text = fTreapta(rez).ToString();
            }
            else if (comboBox2.Text == "Sigmoidala")
            {
                textBox2.Text = fSigmoidala(rez).ToString();
            }
            else if (comboBox2.Text == "Signum")
            {
                textBox2.Text = fSignum(rez).ToString();

            }
            else if (comboBox2.Text == "Tangenta H")
            {
                textBox2.Text = fTanH(rez).ToString();
            }
            else if (comboBox2.Text == "Rampa")
            {
                textBox2.Text = fRampa(rez).ToString();
            }

            verificareC();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { 
            panel2.Controls.Clear();
            o = 0;
            g = 1;
            a = 1;
            if (comboBox2.Text=="Treapta")
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
                    panel2.Controls.Add(numUDo);
                 numUDo.ValueChanged += new EventHandler(this.numActBi_ValueChanged);
                
                printLabelO();

                afisareRezAct();
            }
            else if(comboBox2.Text == "Sigmoidala")
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
                panel2.Controls.Add(numUDo);
                numUDo.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelO();

                numUDg = new NumericUpDown();
                numUDg.Size = new System.Drawing.Size(65, 30);
                numUDg.Top = 30;
                numUDg.Left = 130;
                numUDg.Name = "NumUDg";
                numUDg.Maximum = 1000;
                numUDg.Minimum = -1000;
                numUDg.Value = 1;
                panel2.Controls.Add(numUDg);
                numUDg.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelG();

                afisareRezAct();
            }
            else if(comboBox2.Text == "Signum")
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
                panel2.Controls.Add(numUDo);
                numUDo.ValueChanged += new EventHandler(this.numActBi_ValueChanged);
                printLabelO();
                afisareRezAct();
            }
            else if(comboBox2.Text == "Tangenta H")
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
                panel2.Controls.Add(numUDo);
                numUDo.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelO();

                numUDg = new NumericUpDown();
                numUDg.Size = new System.Drawing.Size(65, 30);
                numUDg.Top = 30;
                numUDg.Left = 130;
                numUDg.Name = "NumUDg";
                numUDg.Maximum = 1000;
                numUDg.Minimum = -1000;
                numUDg.Value = 1;
                panel2.Controls.Add(numUDg);
                numUDg.ValueChanged += new EventHandler(this.numActRe_ValueChanged);

                printLabelG();

                afisareRezAct();

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
                panel2.Controls.Add(numUDo);
                numUDo.ValueChanged += new EventHandler(this.numActRampa_ValueChanged);
                printLabelO();

                numUDa = new NumericUpDown();
                numUDa.Size = new System.Drawing.Size(65, 30);
                numUDa.Top = 30;
                numUDa.Left = 130;
                numUDa.Name = "NumUDa";
                numUDa.Maximum = 1000;
                numUDa.Minimum = 1;
                numUDa.Value = 1;
                panel2.Controls.Add(numUDa);
                numUDa.ValueChanged += new EventHandler(this.numActRampa_ValueChanged);

                printLabelA();

                afisareRezAct();
            }
            

        }

        //label functie de activare
        Label lO;
        Label lG;
        Label lA;
        public void printLabelO()
        {
            lO = new Label();
            lO.Size = new System.Drawing.Size(50, 30);
            lO.Top =30;
            lO.Left = 4;
            lO.BackColor = Color.Transparent;
            lO.Text = "θ:";
            lO.Name = "lO";
            panel2.Controls.Add(lO);
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
            panel2.Controls.Add(lG);
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
            panel2.Controls.Add(lA);
        }
        //functie iesire
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            verificareC();
        }

        public void verificareC()
        {
            int aux;
            if(checkBox1.Checked){ 
            if (comboBox2.Text == "Sigmoidala")
            {

                if (rez >= o) aux = 1;
                else aux = 0;

                textBox3.Text = aux.ToString();


            }
            else if (comboBox2.Text == "Tangenta H")
            {

                if (rez >= o) aux = 1;
                else aux = -1;

                textBox3.Text = aux.ToString();

            }
            else if (comboBox2.Text == "Rampa")
            {

                if (rez >= o) aux = 1;
                else aux = -1;

                textBox3.Text = aux.ToString();


            }
            }
            else
            {
                textBox3.Text = textBox2.Text;
            }
            iesire();
        }

        public void iesire()
        {
            textBox4.Text = textBox3.Text;
        }
    }
}
