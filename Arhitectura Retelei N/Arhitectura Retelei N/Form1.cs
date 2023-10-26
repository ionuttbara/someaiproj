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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
          
            
        }

        public static int nrNeuroniInputLayer;
        public static int nrNeuroniOutputLayer;
        public static int nrHiddenLayer;

        //activare button creare 
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.evtFrm += new ShowFrm(f_evtFrm);
           f.Show();
        }
        void f_evtFrm()
        {
            button2.Enabled = true;
        }
    
        
        public int maxNeuroni()
        {
            int nrNMax = nrNeuroniInputLayer;
            if (nrNMax < nrNeuroniOutputLayer)
            {
                nrNMax = nrNeuroniOutputLayer;
            }

            foreach (Form2.Hlayer i in Form2.listaHlayer)
            {
                if (nrNMax < Convert.ToInt32(i.n.Value))
                {
                    nrNMax = Convert.ToInt32(i.n.Value);
                }
            }
            
            return nrNMax;
        }

        List<Button> listB;
        private void button2_Click(object sender, EventArgs e)
        {
            listB = new List<Button>();

            panel1.Controls.Clear();
            panel2.Controls.Clear();
            tableLayoutPanel1.Controls.Clear();
            

            Button outL = new Button();
            outL.Text = "Output Layer";
            outL.Height = 40;
            outL.Width = 160;
            outL.Left = 25;
            outL.Top = 25;
            panel2.Controls.Add(outL);
            outL.Click += new EventHandler(this.out_Click);

            for (int i = 0; i < nrHiddenLayer; ++i)
            {
                Button b = new Button();
                b.Text = "Hidden Layer" + i.ToString();
                b.Height = 40;
                b.Width = 160;
                b.Top = 25;
                b.Left = i  * 170;
                b.Name = i.ToString();
                panel1.Controls.Add(b);
                listB.Add(b);
                b.Click += new EventHandler(this.functi_Click);
            }


            initializareLayer();
            generareListaLayerEditor();
            initializareTariiTemp();
            setareTabelPanel();

            
           
            for (int i=0;i<nrNeuroniInputLayer;i++)
            { NeuronIn n = inputL.listaNeuroni[i];
                n.c = addNeuronButtonIn(0,i,"IL");
                inputL.listaNeuroni[i]=n;
            }
           
            for (int i = 0; i < nrHiddenLayer; i++)
            {

                
                for (int j = 0; j < Form2.listaHlayer[i].n.Value; j++)
                {
                    Neuron n = listHiddenLayer[i].listaNeuroni[j];
                    n.c=addNeuronButton(i+1, j,"HL"+i.ToString()+"-");
                    listHiddenLayer[i].listaNeuroni[j] = n;
                }
              
            }
            for (int i = 0; i < nrNeuroniOutputLayer; i++)
            {
                Neuron n = outputL.listaNeuroni[i];
                n.c = addNeuronButton(nrHiddenLayer + 1, i, "OL");
                outputL.listaNeuroni[i] = n;
            
               
            }

            
            salvareInput();
            updateListaInputH();
            refreshValFunc();
            initializarePanouTarii();
            updateTarii();
            outputTextBoxShow();
            setareEventButoane();
            updateTextOut();

            tableLayoutPanel1.Paint += new PaintEventHandler(this.tableLayoutPanel1_Paint);
            tableLayoutPanel1.Invalidate();
        }

        
        public void refreshValFunc()
        {

            for (int i = 0; i <nrHiddenLayer;i++)
            {
                for(int j=0;j<Form2.listaHlayer[i].n.Value;j++)
                {
                    Neuron n = listHiddenLayer[i].listaNeuroni[j];
                    n.gin = n.functieIntrare();
                    n.activare = n.functieActivare();
                    n.output = n.checkBoxFunction();
                    listHiddenLayer[i].listaNeuroni[j] = n;
                }
            }
            for(int i=0;i<nrNeuroniOutputLayer;i++)
            {
                Neuron n = outputL.listaNeuroni[i];
                n.gin = n.functieIntrare();
                n.activare = n.functieActivare();
                n.output = n.checkBoxFunction();
                outputL.listaNeuroni[i] = n;

            }
         }

        public void setareTabelPanel()
        {

            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.RowCount = maxNeuroni() + 1;
            tableLayoutPanel1.RowStyles.Clear();
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
            }

            tableLayoutPanel1.ColumnCount = 2 + nrHiddenLayer;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 215));
            }

        }
        

        //  List<Button> blist= new List<Button>();


        //adaugare butaone tarii
        public UserControl1 addNeuronButton(int col,int row,string name)
        {
 
            UserControl1 c = new UserControl1();
            c.Anchor = AnchorStyles.Left;
            c.button2.Text = name + row.ToString();
          //  c.button2.Height = 80;
           // c.button2.Width = 80;
            c.button1.Text = "W";
            if (name == "OL")
            {
                c.Name = col.ToString();
                c.button2.Name = row.ToString();
                c.button2.Click += new EventHandler(this.buttonAfisareValori_ClickOL);

                c.button1.Name = row.ToString();
                c.button1.Click += new EventHandler(this.buttonTariiAfisareOL);
            }
            else
            {
                c.Name = col.ToString();
                c.button2.Name =(col-1).ToString()+"-"+ row.ToString();
                c.button2.Click += new EventHandler(this.buttonAfisareValori_ClickHL);

                c.button1.Name = (col - 1).ToString() + "-" + row.ToString();
                c.button1.Click += new EventHandler(this.buttonTariiAfisareHL);
            }


            tableLayoutPanel1.Controls.Add(c, col, row);
            return c;

        }

        //functii event butaone tarii 
        public void buttonTariiAfisareOL(object sender, EventArgs e)
        {
             refreshValFunc();
            updateListaInputH();
            updateTarii();
            updateTextOut();
            Button b = (Button)sender;

            string s = b.Name;
            Neuron n = outputL.listaNeuroni[Convert.ToInt32(s)];
            n.f5.Show();
            n.f5.button1.Click += new EventHandler(this.buttonForm3_Click);
            for (int i = 0; i <n.f5.t.Count; i++)
            {
                n.f5.t[i].Text = n.input[i].ToString();
                

              }
            
            //refreshValFunc();
        }

        public void buttonTariiAfisareHL(object sender, EventArgs e)
        {
            refreshValFunc();
            updateListaInputH();
            updateTarii();
            updateTextOut();
            Button b = (Button)sender;

            string s = b.Name;
            string first = b.Name.Split('-').First();
            string last = b.Name.Split('-').Last();
            int fr = Convert.ToInt32(first);
            int la = Convert.ToInt32(last);
            Layer l = listHiddenLayer[fr];
            Neuron n = l.listaNeuroni[la];
            
            
            for (int i = 0; i < n.f5.t.Count; i++)
            {
                n.f5.t[i].Text = n.input[i].ToString();


            }
            n.f5.Show();
            //  updateListaInputH();
            //  refreshValFunc();
        }



        private void buttonAfisareValori_ClickOL(object sender, EventArgs e)
        {
            refreshValFunc();
            updateListaInputH();
            updateTarii();
            updateTextOut();
            Button c = (Button)sender;
            string s = c.Name;
            Neuron n = outputL.listaNeuroni[Convert.ToInt32(s)];
            n.f4 = new Form4();
            n.f4.Show();
            n.f4.textBox1.Text =n.gin.ToString();
            n.f4.textBox2.Text = n.activare.ToString();
            n.f4.textBox3.Text = n.output.ToString();
            //   MessageBox.Show(n.gin.ToString());
           
          // initializareTariiTemp();
          //  refreshValFunc();
        }
        private void buttonAfisareValori_ClickHL(object sender, EventArgs e)
        {
            refreshValFunc();
            updateListaInputH();
            updateTarii();
            updateTextOut();
            Button c = (Button)sender;
            string first= c.Name.Split('-').First();
            string last= c.Name.Split('-').Last();
            int fr = Convert.ToInt32(first);
            int la = Convert.ToInt32(last);
            Layer l = listHiddenLayer[fr];
            Neuron n = l.listaNeuroni[la];
            n.f4.Show();
            n.f4.textBox1.Text = n.gin.ToString();
            n.f4.textBox2.Text = n.activare.ToString();
            n.f4.textBox3.Text = n.output.ToString();

           
           // initializareTariiTemp();
           // refreshValFunc();
        }
        
        // // // // 

        //adaugare button input
        public UserControl2 addNeuronButtonIn(int col, int row, string name)
        {

            UserControl2 c = new UserControl2();
            c.button2.Text = name + row.ToString();
            c.Anchor = AnchorStyles.Left;
            c.numericUpDown1.ValueChanged += new EventHandler(this.updateInputNumU);
            tableLayoutPanel1.Controls.Add(c, col, row);
            return c;
        }

        public void updateInputNumU(object sender, EventArgs e)
        {
            refreshValFunc();
            updateListaInputH();
            updateTarii();
            updateTextOut();
        }



        public List<decimal> inputiS;
        public void salvareInput()
        {
            inputiS = new List<decimal>();
           
            foreach(NeuronIn k in inputL.listaNeuroni)
            {
                inputiS.Add(k.c.numericUpDown1.Value);
            }

        }

        public List<decimal> calculOutpuHidden(int indice)
        {
            List<decimal> outs=new List<decimal>();
            
            foreach (Neuron k in listHiddenLayer[indice].listaNeuroni)
            {
                
                outs.Add(k.output);

            }
           

            return outs;

        }
        public List<decimal> initializareTarii(int indice)
        {
            List<decimal> outs = new List<decimal>();
            int i = 0;
            Layer l = listHiddenLayer[indice];
            List<Neuron> ln =l.listaNeuroni;
            for (i=0;i<ln.Count;i++)
            {
                decimal z = 0;
                outs.Add(z);

            }
            
            return outs;

        }
        public void initializareTariiTemp()
        {

            for (int j = 0; j < Form2.listaHlayer[0].n.Value; j++)
            {
                Neuron n1 = listHiddenLayer[0].listaNeuroni[j];
                n1.tari = new List<decimal>();
                for(int k=0;k<nrNeuroniInputLayer; k++)
                {
                    n1.tari.Add(0);
                }
                listHiddenLayer[0].listaNeuroni[j] = n1;
            }

            for (int i = 1; i < nrHiddenLayer; i++)
            {
                for (int j = 0; j < Form2.listaHlayer[i].n.Value; j++)
                {Layer l = listHiddenLayer[i];
                    Neuron n =l.listaNeuroni[j];
                    n.tari = initializareTarii(i-1);
                    listHiddenLayer[i].listaNeuroni[j] = n;
                }
            }
            for (int i = 0; i < nrNeuroniOutputLayer; i++)
            {
                Neuron n = outputL.listaNeuroni[i];
                n.tari = initializareTarii(nrHiddenLayer-1);
                outputL.listaNeuroni[i] = n;

            }
        }
        
        /// <summary>
        /// /////////////
        /// </summary>
        public  void updateListaInputH()
        {
            salvareInput();
           for (int i=0;i<nrHiddenLayer;i++)
            {
               
                if (i==0)
                {
                    Layer l = listHiddenLayer[i];
                    
                    for (int k=0;k<l.listaNeuroni.Count;k++)
                    {
                        Neuron n = l.listaNeuroni[k];
                        n.input = new List<decimal>();
                        n.input=inputiS;
                        l.listaNeuroni[k] = n;
                    }

                }
                else
                {
                    Layer l = listHiddenLayer[i];
                    for (int k = 0; k < l.listaNeuroni.Count; k++)
                    {
                        Neuron n = l.listaNeuroni[k];
                        n.input = new List<decimal>();
                        n.input = calculOutpuHidden(i-1);
                        l.listaNeuroni[k] = n;
                    }

                }
                


            }
            for (int k = 0; k < nrNeuroniOutputLayer; k++)
            {
                Neuron n = outputL.listaNeuroni[k];
                n.input = new List<decimal>();
                n.input = calculOutpuHidden(nrHiddenLayer-1);
                outputL.listaNeuroni[k] = n;
            }


        }


        /// <summary>
        /// //////////////////
        /// </summary>

        List<Layer> listHiddenLayer = new List<Layer>();
       public LayerIn inputL = new LayerIn();
        Layer outputL = new Layer();
        public void initializareLayer()
        {
            inputL.listaNeuroni = new List<NeuronIn>();
            outputL.listaNeuroni = new List<Neuron>();

            for(int i=0;i<nrNeuroniInputLayer;++i)
            {
                NeuronIn ne = new NeuronIn();
                
                inputL.listaNeuroni.Add(ne);
                
                inputL.nrNeuroni=nrNeuroniInputLayer;
            }
            for (int i = 0; i < nrNeuroniOutputLayer; ++i)
            {
                Neuron ne = new Neuron();
                ne.f = new Form3();
                ne.f4 = new Form4();
                ne.tari = new List<decimal>();
                ne.input = new List<decimal>();
                ne.nume = "O" + i.ToString();
                ne.f.comboBox1.SelectedIndex = 0;
                ne.f.comboBox2.SelectedIndex = 0;
                outputL.listaNeuroni.Add(ne);
                outputL.numeL = "OL";
                outputL.functieIntrare = "Suma";
               // outputL.f.
                outputL.funtieActivare = "Treapta";
                outputL.functieIesire = false;
                outputL.f = ne.f;
                
                outputL.nrNeuroni = nrNeuroniOutputLayer;
            }

            for(int i=0;i<nrHiddenLayer;++i)
            { Layer l = new Layer();
                l.f = new Form3();
                l.f.comboBox1.SelectedIndex = 0;
                l.f.comboBox2.SelectedIndex = 0;
                l.functieIntrare = "Suma";
                l.funtieActivare = "Treapta";
                l.functieIesire = false;
                l.nrNeuroni = Convert.ToInt32(Form2.listaHlayer[i].n.Value);
                l.listaNeuroni = new List<Neuron>();
                l.numeL = "HL" + i.ToString();
                listHiddenLayer.Add(l);
                for(int j=0;j<Convert.ToInt32(Form2.listaHlayer[i].n.Value);++j)
                {
                    Neuron ne = new Neuron();
                    ne.f = new Form3();
                   // ne.f.button1.Click += new EventHandler(this.buttonForm3_Click);
                    ne.f4 = new Form4();
                    ne.tari = new List<decimal>();
                    ne.input = new List<decimal>();
                    ne.nume = "H" + i.ToString() + "-" + j.ToString();
                    l.f = ne.f;
                    ne.f.comboBox1.SelectedIndex = 0;
                    ne.f.comboBox2.SelectedIndex = 0;
                    listHiddenLayer[i].listaNeuroni.Add(ne);

                }

            }

          
            // initializareTariiTemp();

        }
        //
        public void setareEventButoane()
        {
            for (int i = 0; i < nrNeuroniOutputLayer; ++i)
            {
                Neuron ne = outputL.listaNeuroni[i];
                ne.f.button1.Click += new EventHandler(this.buttonForm3_Click);
                outputL.listaNeuroni[i] = ne;
            }
            for (int i = 0; i < nrHiddenLayer; ++i)
            {
               
                for (int j = 0; j < Convert.ToInt32(Form2.listaHlayer[i].n.Value); ++j)
                {
                    Neuron ne = listHiddenLayer[i].listaNeuroni[j];
                    ne.f.button1.Click += new EventHandler(this.buttonForm3_Click);
                    listHiddenLayer[i].listaNeuroni[j] = ne;

                }

            }
        }
        public void buttonForm3_Click(object sender, EventArgs e)
        {
            refreshValFunc();
            updateListaInputH();
            updateTarii();
            updateTextOut();
         //   MessageBox.Show("da");


        }


        Form3 outF3;
        private void out_Click(object sender, EventArgs e)
        {
            refreshValFunc();
            updateTextOut();

            outputL.f.Show();

            refreshValFunc();
            updateTextOut();
            // refreshValFunc();

        }


      // public  List<Form3> listaF ;
        public void generareListaLayerEditor()
        {
            outF3 = new Form3();
            outputL.f = outF3;
            for(int i=0;i<nrNeuroniOutputLayer;++i)
            {
                Neuron n = outputL.listaNeuroni[i];
                    n.f= outF3;
                outputL.listaNeuroni[i] = n;
            }
           // listaF = new List<Form3>();
            for (int i = 0; i < nrHiddenLayer; ++i)
            {
                Form3 f3 = new Form3();
                Layer l = listHiddenLayer[i];
                l.f = f3;
                for(int j=0;j<Form2.listaHlayer[i].n.Value;++j)
                {
                    Neuron n = l.listaNeuroni[j];
                   n.f=f3;
                    n.input = new List<decimal>();
                    n.tari = new List<decimal>();
                    l.listaNeuroni[j] = n;
                }
                
                listHiddenLayer[i] = l;
               // listaF.Add(f3);
            }
            initializareTariiTemp();
        }


        private void functi_Click(object sender, EventArgs e)
        { 

            Button n = (Button)sender;
            listHiddenLayer[Convert.ToInt32(n.Name)].f.Show();
            refreshValFunc();
            updateTextOut();
            //listaF[Convert.ToInt32(n.Name)].Show();
        }


        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        
        public void initializarePanouTarii()
        {  
            for(int i=0;i<nrHiddenLayer;++i)
            {
                int z = Convert.ToInt32(Form2.listaHlayer[i].n.Value);
                for (int j=0;j<z;j++)
                {
                    
                        Form5 f5 = new Form5();
                    f5.button1.Click += new EventHandler(this.buttonF5_Click);
                    int y = Convert.ToInt32(listHiddenLayer[i].listaNeuroni[j].input.Count);
                        for (int k=0;k<y;k++)
                        {
                            Label l = new Label();
                            l.Text = "Input" + k.ToString() + ":";
                            l.Size = new System.Drawing.Size(80,30);
                            l.Top = k * 35;
                            l.Left = 10;
                            f5.panel1.Controls.Add(l);

                        }
                    List<TextBox> texA = new List<TextBox>();
                    for (int k = 0; k < listHiddenLayer[i].listaNeuroni[j].tari.Count; k++)
                        {
                            TextBox t = new TextBox();
                            t.TextAlign = HorizontalAlignment.Center;
                            t.Size = new System.Drawing.Size(150, 40);
                            t.Top = k * 35;
                            t.Left = 90;
                            f5.panel1.Controls.Add(t);
                        texA.Add(t);
                        }


                    for (int k = 0; k < listHiddenLayer[i].listaNeuroni[j].input.Count; k++)
                    {
                        Label l = new Label();
                        l.Text = "W" + k.ToString() + ":";
                        l.Size = new System.Drawing.Size(30, 30);
                        l.Top = k * 35;
                        l.Left = 10;
                        f5.panel2.Controls.Add(l);

                    }
                    List<NumericUpDown> numUDA = new List<NumericUpDown>();
                    for (int k = 0; k < listHiddenLayer[i].listaNeuroni[j].tari.Count; k++)
                    {
                        NumericUpDown t = new NumericUpDown();
                        t.DecimalPlaces = 2;
                        t.Increment = 0.01m;
                        t.Size = new System.Drawing.Size(60, 30);
                        t.Top = k * 35;
                        t.Left = 50;
                        f5.panel2.Controls.Add(t);
                        numUDA.Add(t);

                    }
                    Neuron n = listHiddenLayer[i].listaNeuroni[j];
                    n.f5 = f5;
                    n.f5.t = texA;
                    n.f5.n = numUDA;
                    listHiddenLayer[i].listaNeuroni[j] = n;

                }
            }

            for (int i = 0; i < nrNeuroniOutputLayer; ++i)
            {
                Form5 f5 = new Form5();
                f5.button1.Click += new EventHandler(this.buttonF5_Click);

                for (int k = 0; k < outputL.listaNeuroni[i].input.Count; k++)
                {
                    Label l = new Label();
                    l.Text = "Input" + k.ToString() + ":";
                    l.Size = new System.Drawing.Size(80, 30);
                    l.Top = k * 35;
                    l.Left = 10;
                    f5.panel1.Controls.Add(l);

                }
                List<TextBox> texA = new List<TextBox>();
                for (int k = 0; k < outputL.listaNeuroni[i].tari.Count; k++)
                {
                    TextBox t = new TextBox();
                    t.TextAlign = HorizontalAlignment.Center;
                    t.Size = new System.Drawing.Size(150, 40);
                    t.Top = k * 35;
                    t.Left = 90;
                    f5.panel1.Controls.Add(t);
                    texA.Add(t);
                }
                for (int k = 0; k < outputL.listaNeuroni[i].input.Count; k++)
                {
                    Label l = new Label();
                    l.Text = "W" + k.ToString() + ":";
                    l.Size = new System.Drawing.Size(30, 30);
                    l.Top = k * 35;
                    l.Left = 10;
                    f5.panel2.Controls.Add(l);

                }
                List<NumericUpDown> numUDA = new List<NumericUpDown>();
                for (int k = 0; k < outputL.listaNeuroni[i].tari.Count; k++)
                {
                    NumericUpDown t = new NumericUpDown();
                    t.DecimalPlaces = 2;
                    t.Increment = 0.01m;
                    t.Size = new System.Drawing.Size(60, 30);
                    t.Top = k * 35;
                    t.Left = 80;
                    f5.panel2.Controls.Add(t);
                    numUDA.Add(t);
                }


                Neuron n = outputL.listaNeuroni[i];
                n.f5 = f5;
                n.f5.t = texA;
                n.f5.n = numUDA;
                outputL.listaNeuroni[i] = n;
            }
        }

        private void buttonF5_Click(object sender, EventArgs e)
        {
            refreshValFunc();
            updateListaInputH();
            updateTarii();
        }





        public void updateTarii()
        {
            for (int i = 0; i < nrHiddenLayer; ++i)
            {
                int z = Convert.ToInt32(Form2.listaHlayer[i].n.Value);
                for (int j = 0; j < z; j++)
                {

                    Neuron n = listHiddenLayer[i].listaNeuroni[j];

                    for (int k = 0; k < n.tari.Count; k++)
                    {
                        n.tari[k] = n.f5.n[k].Value;
                    }
                    listHiddenLayer[i].listaNeuroni[j] = n;

                }
            }

            for (int i = 0; i < nrNeuroniOutputLayer; ++i)
            {

                Neuron n = outputL.listaNeuroni[i];
                for (int k = 0; k < outputL.listaNeuroni[i].tari.Count; k++)
                {
                    n.tari[k] = Convert.ToDecimal(n.f5.n[k].Value);
                }

                outputL.listaNeuroni[i] = n;
            }
        }





        public void outputTextBoxShow()
        {
            for(int i=0;i<nrNeuroniOutputLayer;i++)
            {
                TextBox t = new TextBox();
                t.Location = new Point(90, 50);
               //t.Show();
               
                 t.TextAlign = HorizontalAlignment.Center;
                t.Size = new System.Drawing.Size(110, 40);
                outputL.listaNeuroni[i].c.t = t;
                outputL.listaNeuroni[i].c.Size = new System.Drawing.Size(210, 85);
                outputL.listaNeuroni[i].c.t.Show();
                outputL.listaNeuroni[i].c.Controls.Add(t);
            }
        }

        public void updateTextOut()
        {
            for (int i = 0; i < nrNeuroniOutputLayer; i++)
            {
                
                outputL.listaNeuroni[i].c.t.Text = outputL.listaNeuroni[i].output.ToString();
            }
        }
        //desen


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            int x1, x2, y1, y2;
            Control a, b;
            Pen pen = new Pen(Color.Purple, 2);
            Graphics p = tableLayoutPanel1.CreateGraphics();
            for (int i = 0; i < this.tableLayoutPanel1.ColumnCount; i++)
            {
                for ( int j = 0; j <= this.tableLayoutPanel1.RowCount; j++)
                {
                    a = tableLayoutPanel1.GetControlFromPosition(i, j);

                    for (int k = 0; k <= this.tableLayoutPanel1.RowCount; k++)
                    {
                        b = tableLayoutPanel1.GetControlFromPosition(i + 1, k);
                        if (a != null && b != null)
                        {
                            
                            x1 = a.Location.X+20;
                            y1 = a.Location.Y+50 ;
                            x2 = b.Location.X +20;
                            y2 = b.Location.Y +50;
                            p.DrawLine(pen, x1, y1, x2, y2);
                        }
                    }
                }
            }
           

        }

        //desen


    }//iesire form 1;




    /// <summary>
    /// neuroni + layer 
    /// </summary>
    public struct Neuron{
        public List<decimal> input;
        public List<decimal> tari;
        public decimal output;
        public decimal gin;
        public decimal activare;
        public string nume;
        public UserControl1 c { get; set; }
        public Form3 f;
        public Form4 f4;
        public Form5 f5;
        public List<TextBox> tex;
        public List<NumericUpDown> numUD;
        
        public decimal checkBoxFunction()
        {//
            decimal teta=f.numUDo.Value;
             activare = functieActivare();
            

            if (f.checkBox1.Checked)
            {
                if (f.comboBox2.Text == "Sigmoidala")
                {

                    if (activare >= f.numUDo.Value) output = 1;
                    else output = 0;

                }
                else if (f.comboBox2.Text == "Tangenta H")
                {

                    if (activare >= f.numUDo.Value) output = 1;
                    else output = -1;



                }
                else if (f.comboBox2.Text == "Rampa")
                {

                    if (activare >= f.numUDo.Value) output = 1;
                    else output = -1;

                }
            }
            else
            {
                output = functieActivare();
            }
            return output;
        }//
        public decimal functieActivare()
        {
             gin = functieIntrare();

            if (f.comboBox2.Text == "Treapta")
            {
                activare = fTreapta(gin);
            }
            else if (f.comboBox2.Text == "Sigmoidala")
            {
                activare = fSigmoidala(gin);
            }
            else if (f.comboBox2.Text == "Signum")
            {
                activare = fSignum(gin);

            }
            else if (f.comboBox2.Text == "Tangenta H")
            {
                activare = fTanH(gin);
            }
            else if (f.comboBox2.Text == "Rampa")
            {
                activare = fRampa(gin);
            }
            return activare;
        }
        public int fTreapta(decimal x)
        {
            if (x >= f.numUDo.Value) return 1;
            else return 0;

        }
        public decimal fSigmoidala(decimal x)
        {
            return Convert.ToDecimal(1 / (1 + Math.Exp(Convert.ToDouble(-f.numUDg.Value * (x - f.numUDo.Value)))));
        }
        public int fSignum(decimal x)
        {
            if (x >= f.numUDo.Value) return 1;
            else return -1;
        }
        public decimal fTanH(decimal x)
        {
            return Convert.ToDecimal(((Math.Exp(Convert.ToDouble(f.numUDg.Value * (x - f.numUDo.Value))) - Math.Exp(Convert.ToDouble(-f.numUDg.Value * (x - f.numUDo.Value)))) / (Math.Exp(Convert.ToDouble(f.numUDg.Value * (x - f.numUDo.Value))) + Math.Exp(Convert.ToDouble(-f.numUDg.Value * (x - f.numUDo.Value))))));
        }
        public decimal fRampa(decimal x)
        {
            if (x - f.numUDo.Value > f.numUDa.Value) return 1;
            else if (x - f.numUDo.Value < -f.numUDa.Value) return -1;
            else return Convert.ToDecimal((x - f.numUDo.Value) / f.numUDa.Value);
        }

        //activare
        //intrare
        public decimal functieIntrare()
        {
            

            if (f.comboBox1.Text == "Suma")
            {
                decimal sum = 0;
                for (int i = 0; i < input.Count; ++i)
                {
                    sum += input[i]*tari[i];
                }
                gin = sum;
                 return sum;
               
            }
            else if (f.comboBox1.Text == "Produs")
            {
                decimal pro = 1;
                for (int i = 0; i < input.Count; ++i)
                {
                    pro *= input[i]*tari[i];
                }
                gin = pro;
                return pro;
                
            }
            else if (f.comboBox1.Text == "Maxim")
            {
                decimal maxim = input[0] * tari[0];
                for (int i = 0; i < input.Count; ++i)
                {
                    if (maxim < input[i] * tari[i])
                        maxim = input[i] * tari[i];
                }
                gin = maxim;
                return maxim;
            }
            else if (f.comboBox1.Text == "Minim")
            {
                decimal minim = input[0] * tari[0];
                for (int i = 0; i < input.Count; ++i)
                {
                    if (minim > input[i]*tari[i])
                        minim = input[i] * tari[i];
                }
                gin = minim;
                return minim;
            }
            else
            { //in caz ca nu e selectat nimic

                decimal sum = 0;
                for (int i = 0; i < input.Count; ++i)
                {
                    sum += input[i] * tari[i];
                }
                gin = sum;
                return sum;
            }
            
        }

    }
    
    public struct NeuronIn
    {
        //public decimal[] input;
        //public decimal[] tari;
        //public decimal output;
        // public decimal gin;
        //public decimal activare;
        public UserControl2 c { get; set; }

    }
    public struct Layer
    {
        public List<Neuron> listaNeuroni;
        public string functieIntrare;
        public string funtieActivare;
        public bool functieIesire;
        public int nrNeuroni;
        public Form3 f;
        public string numeL;




    }
    public struct LayerIn
    {
        public List<NeuronIn> listaNeuroni;
      //  public string functieIntrare;
        //public string funtieActivare;
        //public bool functieIesire;
        public int nrNeuroni;


    }
    

}
