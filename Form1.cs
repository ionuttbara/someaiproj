using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace ProjectAI3
{
	public partial class Form1 : Form
	{
		string c;
		DataTable dt = new DataTable();
		DataTable valoriRamase = new DataTable();
		DataTable dtn = new DataTable(); // normal values
		normal[] x = new normal[13];
		//selectare date de Antrenament
		DataRow[] listaDt = new DataRow[1000]; // randuri de antrenament
		DataTable dataAntrenament = new DataTable();
		List<int> listRand = new List<int>();
		// locatie chart ptr animare
		public Form1()
		{
			InitializeComponent();
			numericUpDown4.Value = 1;
			listaEepoca = new List<decimal>();
			EepocaCurenta = 1;
			for (int i = 0; i < 12; ++i)
			{
				x[i] = new normal();
				switch (i)
				{
					case 0:
						x[i].setNorm(0, 10);
						break;
					case 1:
						x[i].setNorm(0, 1);
						break;
					case 2:
						x[i].setNorm(0,1);
						break;
					case 3:
						x[i].setNorm(0, 20);
						break;
					case 4:
						x[i].setNorm(0, 1);
						break;
					case 5:
						x[i].setNorm(0, 70);
						break;
					case 6:
						x[i].setNorm(0, 225);
						break;
					case 7:
						x[i].setNorm(0, 1);
						break;
					case 8:
						x[i].setNorm(0, 5);
						break;
					case 9:
						x[i].setNorm(0, 1);
						break;
					case 10:
						x[i].setNorm(0, 35);
						break;
					case 11:
						x[i].setNorm(0, 10);
						break;
				}
			}
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Title = "Select a data file";
			openFileDialog1.Filter = "CSV Files (*.csv)|*.csv";
			openFileDialog1.InitialDirectory = @"E:\Projects\Development\Visual Studio\ProjectAI3\ProjectAI3\datasFolder";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				c = openFileDialog1.FileName;
				getDataCSV(c);
				valoriRamase = dtn.Copy();
			}
		}

		// metoda care citește date din CSV și apoi le normalizează
		public void getDataCSV(string path)
		{

			string[] lines = System.IO.File.ReadAllLines(path);
			if (lines.Length > 0)
			{
				string firstLine = lines[0];
				string[] headerLabels = firstLine.Split(';');
				foreach (string headerWord in headerLabels)
				{
					dt.Columns.Add(new DataColumn(headerWord));
					dtn.Columns.Add(new DataColumn(headerWord));
				dataAntrenament.Columns.Add(new DataColumn(headerWord));
				}
				for (int r = 1; r < lines.Length; r++)
				{
					string[] dataWords = lines[r].Split(';');
					DataRow dr = dt.NewRow();
					DataRow drn = dtn.NewRow();
					int columnIndex = 0;
					foreach (string headerWord in headerLabels)
					{
						dr[headerWord] = Convert.ToDecimal(dataWords[columnIndex]);
						decimal dataA = Convert.ToDecimal(dataWords[columnIndex]);
						drn[headerWord] = x[columnIndex].m * dataA + x[columnIndex].n;
						columnIndex++;

					}
					dt.Rows.Add(dr);
					dtn.Rows.Add(drn);
				}
			}
			if (dt.Rows.Count > 0)
			{
				dataGridView1.DataSource = dt;
				dataGridView2.DataSource = dtn;
			}
		}
		public void selectTDatat()
		{
			var rand = new Random();

			for (int i = 0; i < 1000; i++)
			{

				int rd = rand.Next(4500);
				Console.WriteLine(rd);
				listaDt[i] = dtn.Rows[rd];
				dataGridView2.Rows[rd].DefaultCellStyle.BackColor = Color.FromArgb(254, 0, 250);
				listRand.Add(rd);
			}
			listRand.Sort();
			foreach (DataRow row in listaDt)
			{
				dataAntrenament.ImportRow(row);
			}
			dataGridView3.DataSource = dataAntrenament;
			for (int i = 999; i >= 0; i--)
			{
				valoriRamase.Rows[listRand[i]].Delete();
			}
		}

		bool colortab = true;


		//normalizare
		struct normal
		{
			public decimal m;
			public decimal n;
			public decimal a, b;

			public void calculM()
			{
				m = 1 / (b - a);
			}
			public void calculN()
			{
				n = (-a) / (b - a);
			}
			public void setNorm(decimal a, decimal b)
			{
				this.a = a;
				this.b = b;
				calculM();
				calculN();
			}
		}
		// initalizarea retea
		public Layer outputL = new Layer();
		public Layer inputL = new Layer();
		public List<Layer> listHidden = new List<Layer>();
		public void initializaerReteaN()
		{//in
			var rand = new Random();
			inputL.listaNeuroni = new List<Neuron>();
			inputL.nrNeuroni = dataAntrenament.Columns.Count - 1;
			for (int i = 0; i < dataAntrenament.Columns.Count - 1; i++)
			{
				Neuron n = new Neuron();
				inputL.listaNeuroni.Add(n);
			}

			listHidden = new List<Layer>();
			//hidden
			for (int i = 0; i < listaNrNeuroniHiddenLAyer.Count; i++)
			{
				Layer l = new Layer();
				l.nrNeuroni = Convert.ToInt32(listaNrNeuroniHiddenLAyer[i]);
				l.listaNeuroni = new List<Neuron>();
				listHidden.Add(l);


				for (int j = 0; j < listaNrNeuroniHiddenLAyer[i]; j++)
				{
					Neuron n = new Neuron();
					n.tari = new List<decimal>();
					n.input = new List<decimal>();
					if (i == 0)
					{
						for (int k = 0; k < inputL.nrNeuroni; k++)
						{
							if (rand.Next() % 2 == 0)
							{
								n.tari.Add(Convert.ToDecimal(rand.NextDouble()));
								n.input.Add(0);
							}
							else
							{
								n.tari.Add(-Convert.ToDecimal(rand.NextDouble()));
								n.input.Add(0);
							}
						}
					}
					else
					{
						for (int k = 0; k < listaNrNeuroniHiddenLAyer[i - 1]; k++)
						{
							n.tari.Add(Convert.ToDecimal(rand.NextDouble()));
							n.input.Add(0);
						}
					}

					listHidden[i].listaNeuroni.Add(n);
				}

			}

			//out
			outputL.listaNeuroni = new List<Neuron>();
			outputL.nrNeuroni = 1;
			Neuron n1 = new Neuron();
			n1.tari = new List<decimal>();
			n1.input = new List<decimal>();
			for (int k = 0; k < listaNrNeuroniHiddenLAyer[listaNrNeuroniHiddenLAyer.Count - 1]; k++)
			{
				n1.tari.Add(Convert.ToDecimal(rand.NextDouble()));
				n1.input.Add(0);
			}
			outputL.listaNeuroni.Add(n1);

		}
		public void numericUDsaveNr_ValueChanged(object sender, EventArgs e)
		{
			listaNrNeuroniHiddenLAyer = new List<decimal>();
			for (int i = 0; i < numericUpDown4.Value; i++)
			{
				listaNrNeuroniHiddenLAyer.Add(listnNmericUpDowns[i].Value);
				initializaerReteaN();
			}
		}
		// selectie culoare date
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
				if (colortab)
				{
					selectTDatat();
					colortab = false;
				}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			zedGraphControl1.GraphPane.CurveList.Clear();
			zedGraphControl1.GraphPane.AddCurve("Eroare Epoca", new PointPairList(), Color.YellowGreen, SymbolType.None);
			zedGraphControl1.GraphPane.AddCurve("Eroare Admisa", new PointPairList(), Color.Green, SymbolType.None);
			zedGraphControl1.GraphPane.Title.Text = "Rezultate Antrenament";
			zedGraphControl1.GraphPane.XAxis.Title.Text = "Număr Epoci";
			zedGraphControl1.GraphPane.YAxis.Title.Text = "Erori admise";
			invatare();
	//		invatare();
			testare();
			zedGraphControl1.AxisChange();
			zedGraphControl1.Invalidate();
		}

		public bool stop = true;
		public decimal EepocaCurenta;
		public List<decimal> listaEpas;
		public List<decimal> listaEepoca;
		public void invatare()
		{
			/*if (EepocaCurenta < numericUpDown2.Value == true && EepocaCurenta < numericUpDown2.Value * 2 == true && EepocaCurenta != 0)
			{
			   tbArrray.AppendText("Eroare finala: " + EepocaCurenta + Environment.NewLine);
			   goto adk;
			}*/
			decimal nrEpoci = numericUpDown3.Value;
			//Console.WriteLine("Nr epoci :" + nrEpoci);
			listaEepoca = new List<decimal>();
			decimal Q = 0;
			decimal auxEpas;
			decimal auxTy;
			if (EepocaCurenta == 0) EepocaCurenta = 1;
			if (EepocaCurenta < numericUpDown2.Value == false && EepocaCurenta < numericUpDown2.Value * 2 == false)
				//{

				for (int i = 0; i < nrEpoci; i++)
				{
					//tbArrray.Clear(); 
					//tbArrray.AppendText("Epoca " + i + Environment.NewLine);
					listaEpas = new List<decimal>();
					DataRow[] dr = dataAntrenament.Select();
					Q = 0;
					if (EepocaCurenta < numericUpDown2.Value == false && EepocaCurenta < numericUpDown2.Value * 2 == false)
					//if (EepocaCurenta < numericUpDown2.Value == false )
					{
						//for (int j = 0; j < 3; j++) 
						for (int j = 0; j < 1000; j++)
						// foreach (DataRow row in dr)
						{
							Q++;
							//Console.WriteLine("Q =" + Q);
							string[] elemente = new string[12];

							elemente[0] = dr[j].Field<string>("fixed_acidity");
							elemente[1] = dr[j].Field<string>("volatile_acidity");
							elemente[2] = dr[j].Field<string>("citric_acid");
							elemente[3] = dr[j].Field<string>("residual_sugar");
							elemente[4] = dr[j].Field<string>("chlorides");
							elemente[5] = dr[j].Field<string>("free_sulfur_dioxide");
							elemente[6] = dr[j].Field<string>("total_sulfur_dioxide");
							elemente[7] = dr[j].Field<string>("density");
							elemente[8] = dr[j].Field<string>("pH");
							elemente[9] = dr[j].Field<string>("sulphates");
							elemente[10] = dr[j].Field<string>("alcohol");
							elemente[11] = dr[j].Field<string>("quality");

							//incarc valorile in neuroni si calculez output retea
							for (int l = 0; l < inputL.listaNeuroni.Count; l++)
							{
								//Console.WriteLine("Lista Neuroni input layer :" + inputL.listaNeuroni.Count);
								Neuron n = inputL.listaNeuroni[l];
								//Console.WriteLine("Lista Neuroni input layer :" + inputL.listaNeuroni[l]);

								n.output = Convert.ToDecimal(elemente[l]);
								//Console.WriteLine("Output neuron :" + n.output);
								inputL.listaNeuroni[l] = n;
							}
							Neuron nt = outputL.listaNeuroni[0];
							//Console.WriteLine("Neuron output :" + nt);
							nt.output = calculaOutputRetea();//neuron temporar
															 //Console.WriteLine("Neuron output-output retea :" + nt.output);
							outputL.listaNeuroni[0] = nt;
							//-------------------


							//calcul eroare pas
							decimal x = Convert.ToDecimal(elemente[11]);
							//Console.WriteLine("Pret target :" + x);
							decimal y = outputL.listaNeuroni[0].output;
							//Console.WriteLine("Output retea" + y);
							auxEpas = x - y;
							auxTy = (auxEpas * auxEpas) / 2;
							//Console.WriteLine("Eroare((x-y)^2)/2):" + auxTy);
							listaEpas.Add(auxTy);
							//Console.WriteLine("Adaugare Epas " + j +":"+ auxTy);
							//-----

							Neuron no = outputL.listaNeuroni[0];

							no.eroareNeuron = outputL.listaNeuroni[0].calculEroareOut(Convert.ToDecimal(elemente[11]));//rezultat-tinta * derivataAct
																													   //Console.WriteLine("Calcul eroare neuron OL: " + no.eroareNeuron);
							outputL.listaNeuroni[0] = no;
							outputL.listaNeuroni[0].recalculTariiEroare(numericUpDown1.Value);
							//Console.WriteLine("Tarii recalculate dintre OL si HL");



							for (int k = listHidden.Count - 1; k >= 0; k--)
							{
								if (k == listHidden.Count - 1)
								{
									//Console.WriteLine("Ultimul HL");
									for (int p = 0; p < listHidden[k].listaNeuroni.Count; p++)
									{
										Neuron n = listHidden[k].listaNeuroni[p];
										//Console.WriteLine("Neuron " + p);
										n.eroareNeuron = 0;
										for (int u = 0; u < outputL.listaNeuroni.Count; u++)
										{
											n.eroareNeuron += outputL.listaNeuroni[u].eroareNeuron * outputL.listaNeuroni[u].tari[p];

										}
										n.eroareNeuron *= n.activare * (1 - n.activare);
										//Console.WriteLine("Eroare neuron:" + n.eroareNeuron);

										n.tari = listHidden[k].listaNeuroni[p].recalculTariiEroare(numericUpDown1.Value);
										//Console.WriteLine("Lista tarii neuron:" + n.tari);
										listHidden[k].listaNeuroni[p] = n;
									}


								}
								else
								{
									for (int p = 0; p < listHidden[k].listaNeuroni.Count; p++)
									{
										//Console.WriteLine("HL:" + k);
										Neuron n = listHidden[k].listaNeuroni[p];
										//Console.WriteLine("Neuron:" + p);
										n.eroareNeuron = 0;
										for (int u = 0; u < listHidden[k + 1].listaNeuroni.Count; u++)
										{
											n.eroareNeuron += listHidden[k + 1].listaNeuroni[u].eroareNeuron * listHidden[k + 1].listaNeuroni[u].tari[p];

										}
										n.eroareNeuron *= n.activare * (1 - n.activare);
										//Console.WriteLine("Eroare neuron:" + n.eroareNeuron);
										n.tari = listHidden[k].listaNeuroni[p].recalculTariiEroare(numericUpDown1.Value);
										//Console.WriteLine("Lista tarii neuron:" + n.tari);
										listHidden[k].listaNeuroni[p] = n;
									}

								}
							}
						}
						EepocaCurenta = calculEepoca(Q);
						//Console.WriteLine("Eroare epoca curenta:" + EepocaCurenta);
						textBox1.Text = EepocaCurenta.ToString();
						zedGraphControl1.GraphPane.CurveList[0].AddPoint(i, (double)EepocaCurenta);
						zedGraphControl1.GraphPane.CurveList[1].AddPoint(i, (double)numericUpDown2.Value);

						//  chart1.Invalidate();
						//chart1.Update();
						listaEepoca.Add(EepocaCurenta);
						//Console.WriteLine("Eroare epoca curenta adaugata:" + EepocaCurenta);
					}

				}
			if (EepocaCurenta < numericUpDown2.Value == false && EepocaCurenta < numericUpDown2.Value * 2 == false)
			{
				testare();
				tbArrray.AppendText("ListaEepoca :" + Environment.NewLine);
				for (int i = 0; i < listaEepoca.Count; i++)
				{//for (int i = 0; i < nrEpoci; i++)
					tbArrray.AppendText(listaEepoca[i] + Environment.NewLine);
					//Console.WriteLine("Lista Eroare epoca " + i +":"+ listaEepoca[i]);
				}
			}
			else
			{
				tbArrray.AppendText("ListaEepoca :" + EepocaCurenta + Environment.NewLine);
				//Console.WriteLine("Eroare epoca =" + EepocaCurenta );
			}

		}
		public decimal calculEepoca(decimal Q)
		{
			decimal Eepoca = 0;
			for (int i = 1; i < Q; i++)
			{
				Eepoca += listaEpas[i];
			}
			return Eepoca / Q;

		}

		private void button2_Click(object sender, EventArgs e)
		{
		stop = true;
		}

		private void tabPage5_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}
		//initializare layer
		List<decimal> listaNrNeuroniHiddenLAyer;
		List<NumericUpDown> listnNmericUpDowns;
		private void numericUpDown4_ValueChanged(object sender, EventArgs e)
		{
			creareIntrari(Convert.ToInt32(numericUpDown4.Value));
			numericUDsaveNr_ValueChanged(sender, e);
			button1.Enabled = true;
		}
		public void testare()
		{
			dataGridView3.DataSource = valoriRamase;
			DataRow[] r = valoriRamase.Select();
			decimal p = 0;
			for (int j = 0; j < valoriRamase.Rows.Count; j++)
			{
				string[] elemente = new string[12];
				elemente[0] = r[j].Field<string>("fixed_acidity");
				elemente[1] = r[j].Field<string>("volatile_acidity");
				elemente[2] = r[j].Field<string>("citric_acid");
				elemente[3] = r[j].Field<string>("residual_sugar");
				elemente[4] = r[j].Field<string>("chlorides");
				elemente[5] = r[j].Field<string>("free_sulfur_dioxide");
				elemente[6] = r[j].Field<string>("total_sulfur_dioxide");
				elemente[7] = r[j].Field<string>("density");
				elemente[8] = r[j].Field<string>("pH");
				elemente[9] = r[j].Field<string>("sulphates");
				elemente[10] = r[j].Field<string>("alcohol");
				elemente[11] = r[j].Field<string>("quality");
				for (int l = 0; l < inputL.listaNeuroni.Count; l++)
				{
					Neuron n = inputL.listaNeuroni[l];

					n.output = Convert.ToDecimal(elemente[l]);
					inputL.listaNeuroni[l] = n;
				}
				calculaOutputRetea();
				//   outputL.listaNeuroni[0].output;
				if (j % 100 == 0)
				{
					chart2.Series["Valoare Calculata"].Points.AddXY(j, outputL.listaNeuroni[0].output);
					chart2.Series["Valoare Initiala"].Points.AddXY(j, Convert.ToDecimal(elemente[11]));
				}
				if (outputL.listaNeuroni[0].output - Convert.ToDecimal(elemente[11]) >= 0 && outputL.listaNeuroni[0].output - Convert.ToDecimal(elemente[11]) < numericUpDown2.Value * 2)
				{
					p++;
				}

			}

			decimal c = (p * 100) / valoriRamase.Rows.Count;

			label6.Text = c.ToString() + "%";

		}
		//calcul rezultat retea
		public decimal calculaOutputRetea()
		{
			for (int i = 0; i < listaNrNeuroniHiddenLAyer.Count; i++)
			{
				if (i == 0)
				{

					inputL.setOutputLActual();
					for (int k = 0; k < listaNrNeuroniHiddenLAyer[i]; k++)
					{
						Neuron n = listHidden[i].listaNeuroni[k];
						n.input = inputL.listaOutpuLayerActual;
						n.outputN();
						listHidden[i].listaNeuroni[k] = n;
					}
					Layer l1 = listHidden[i];
					l1.listaOutpuLayerActual = listHidden[i].setOutputLActual();
					listHidden[i] = l1;

				}
				else
				{

					for (int k = 0; k < listaNrNeuroniHiddenLAyer[i]; k++)
					{
						Neuron n = listHidden[i].listaNeuroni[k];
						Layer l2 = listHidden[i - 1];
						n.input = new List<decimal>();
						n.input = l2.listaOutpuLayerActual;
						n.outputN();
						listHidden[i].listaNeuroni[k] = n;
					}
					Layer l1 = listHidden[i];
					l1.listaOutpuLayerActual = listHidden[i].setOutputLActual();
					listHidden[i] = l1;

				}

			}

			for (int k = 0; k < outputL.listaNeuroni.Count; k++)
			{
				Neuron n = outputL.listaNeuroni[k];
				Layer l3 = listHidden[listHidden.Count - 1];
				n.input = new List<decimal>();
				n.input = l3.listaOutpuLayerActual;
				n.outputN();
				outputL.listaNeuroni[k] = n;
			}
			Layer l = outputL;
			l.listaOutpuLayerActual = outputL.setOutputLActual();
			outputL = l;

			return outputL.listaNeuroni[0].output;
		}
		public void creareIntrari(int nrIntrari)
		{
			panel1.Controls.Clear();
			listnNmericUpDowns = new List<NumericUpDown>();
			for (int i = 0; i < nrIntrari; ++i)
			{
				NumericUpDown n = new NumericUpDown();
				System.Windows.Forms.Label l = new System.Windows.Forms.Label();
				int index = i;

				n.Size = new System.Drawing.Size(50, 50);
				n.Top = i * 65;
				n.Left = 220;
				n.Name = "NumUD1-" + i.ToString();
				n.Maximum = 1000;
				n.Minimum = 1;
				panel1.Controls.Add(n);

				n.ValueChanged += new EventHandler(this.numericUDsaveNr_ValueChanged);

				listnNmericUpDowns.Add(n);

				l.Size = new System.Drawing.Size(200, 50);
				l.Top = i * 65;
				l.Left = 3;
				l.BackColor = Color.Transparent;
				l.Text = "Numar de neuroni pe Hidden layer" + i.ToString() + ":";
				l.Name = "Hlayer" + i.ToString();
				panel1.Controls.Add(l);
			}
		}
		// neuron
		public struct Neuron
		{
			public List<decimal> input;
			public List<decimal> tari;
			public decimal output;
			public decimal gin;
			public decimal activare;
			public string nume;
			public decimal eroareNeuron;
			public decimal dTarii;
			public void outputN()//setare output
			{
				output = functieActivare();
			}
			public decimal functieActivare()//functie activare
			{
				gin = functieIntrare();
				activare = fSigmoidala(gin);
				return activare;
			}

			public decimal fSigmoidala(decimal x)
			{
				decimal g = 1;
				decimal teta = 0;
				return Convert.ToDecimal(1 / (1 + Math.Exp(Convert.ToDouble(-g * (x - teta)))));
			}

			public decimal functieIntrare()//functie intrare
			{
				decimal sum = 0;
				for (int i = 0; i < input.Count; ++i)
				{
					sum += input[i] * tari[i];
				}
				gin = sum;
				return sum;
			}

			public decimal calculEroareOut(decimal tinta)
			{
				return (output - tinta) * activare * (1 - activare);
			}

			public List<decimal> recalculTariiEroare(decimal pas)
			{

				dTarii = -pas * activare * eroareNeuron;
				for (int i = 0; i < tari.Count; i++)
					tari[i] = tari[i] + dTarii;

				return tari;
			}
		}

		private void label18_Click(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{
			string[] elemente = new string[11];
			elemente[0] = textBox2.Text;
			elemente[1] = textBox3.Text;
			elemente[2] = textBox4.Text;
			elemente[3] = textBox5.Text;
			elemente[4] = textBox6.Text;
			elemente[5] = textBox12.Text;
			elemente[6] = textBox11.Text;
			elemente[7] = textBox10.Text;
			elemente[8] = textBox9.Text;
			elemente[9] = textBox8.Text;
			elemente[10] = textBox7.Text;
			decimal aux;
			decimal[] v = new decimal[11];
			for (int i = 0; i < 11; i++)
			{
				aux = Convert.ToDecimal(elemente[i]);

				v[i] = x[i].m * aux + x[i].n;
			}
			for (int l = 0; l < inputL.listaNeuroni.Count; l++)
			{
				Neuron n = inputL.listaNeuroni[l];

				n.output = v[l];
				inputL.listaNeuroni[l] = n;
			}
			Neuron nt = outputL.listaNeuroni[0];
			nt.output = calculaOutputRetea();//neuron temporar
			outputL.listaNeuroni[0] = nt;

			decimal y;
			y = (outputL.listaNeuroni[0].output - x[11].n) / x[11].m;
			int za = Convert.ToInt32(y);
			MessageBox.Show(za.ToString(), "Rezultat");
		}
		public struct Layer
		{
			public List<Neuron> listaNeuroni { get; set; }
			public int nrNeuroni;
			public string numeL;
			public List<decimal> listOutpuLayerAnterior;
			public List<decimal> listaOutpuLayerActual;
			public List<decimal> setOutputLActual()
			{
				listaOutpuLayerActual = new List<decimal>();
				for (int i = 0; i < listaNeuroni.Count; i++)
				{
					listaOutpuLayerActual.Add(listaNeuroni[i].output);
				}
				return listaOutpuLayerActual;
			}
		}
	}
}
