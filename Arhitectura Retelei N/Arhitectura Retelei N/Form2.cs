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
    public delegate void ShowFrm();
    public partial class Form2 : Form
    {
        public event ShowFrm evtFrm;
        public Form2()
        {
            InitializeComponent();
            creareIntrari(Convert.ToInt32(numericUpDown1.Value));

        }
        public struct Hlayer {


            public Label l { get; set; }
            public NumericUpDown n;
            public int index;

        }

        public static List<Hlayer> listaHlayer = new List<Hlayer>();
        
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            creareIntrari(Convert.ToInt32(numericUpDown3.Value));
        }
     /*   public void numericUDsaveNr_ValueChanged(object sender, EventArgs e)
        { 
           
        }
     */
        public void creareIntrari(int nrIntrari)
        {
            listaHlayer.Clear();
            for (int i = 0; i < nrIntrari; ++i)
            {
                NumericUpDown n = new NumericUpDown();
                Label l = new Label();
                int index=i;

                n.Size = new System.Drawing.Size(60, 30);
                n.Top = i * 40;
                n.Left = 350;
                n.Name = "NumUD1-" + i.ToString();
                n.Maximum = 1000;
                n.Minimum = 1;
                panel1.Controls.Add(n);

                //n.ValueChanged += new EventHandler(this.numericUDsaveNr_ValueChanged);

               
                l.Size = new System.Drawing.Size(300, 30);
                l.Top = i * 40;
                l.Left = 3;
                //l.BackColor = Color.Transparent;
                l.Text = "Numar de neuroni pe Hidden layer" + i.ToString() + ":";
                l.Name = "Hlayer" + i.ToString();
                panel1.Controls.Add(l);



                Hlayer hl = new Hlayer();
                hl.l = l;
                hl.n = n;
                hl.index = index;
                listaHlayer.Add(hl);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.nrNeuroniInputLayer = Convert.ToInt32(numericUpDown1.Value);
            Form1.nrNeuroniOutputLayer = Convert.ToInt32(numericUpDown2.Value);
            Form1.nrHiddenLayer = Convert.ToInt32(numericUpDown3.Value);
            if (evtFrm != null)
            {
                evtFrm();
            }
            this.Close();

        }
    }
}
