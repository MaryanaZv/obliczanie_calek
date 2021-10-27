using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;
using System.IO;

namespace projekt3_Zvarych54558
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.75F);
            this.Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.7F);
        }

        private void lblKolorLinii_Click(object sender, EventArgs e)
        {
            
        }

        private void lblKolorTla_Click(object sender, EventArgs e)
        {

        }

        private void btnKolorLiniiWykresu_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGrafikWizual_Click(object sender, EventArgs e)
        {
            float mzX, mzY;
            chart1.Visible = true;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                mzX = float.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                mzY = float.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                chart1.Series[0].Points.AddXY(mzX, mzY);
            }

            chart1.ChartAreas[0].AxisX.Title = "Wartości zmiennej X";
            chart1.ChartAreas[0].AxisY.Title = "Wartości funkcji F(X)";
            chart1.Series[0].IsVisibleInLegend = true;
            chart1.Legends.FindByName("Legend1").Docking = Docking.Bottom;
            chart1.Series[0].Name = "Wykres zmian wartości szeregu w podanym przedziale";
            chart1.Series[0].ChartType = SeriesChartType.Line;

            btnGrafikWizual.Enabled = false;
        }

        private void txtWartoscX_TextChanged(object sender, EventArgs e)
        {

        }

        static float mzSumaSzereguPotęgowego(float mzX, float mzEps)
        {
            float mzW, mzS;
            int mzN = 1;

            mzS = 0.0F;
            mzW = 0.5F * (mzX + 4.0F);

            do
            {
                mzS = mzS + mzW;
                mzN++;
                mzW = mzW * ((mzN * mzX + 4 * mzN) / (2 * mzN - 2));

            } while (Math.Abs(mzW) > mzEps);

            return mzS;
        }

        static float mzCalkaMetodaTrapezow(float mzD, float mzG, float mzEps)
        {
            float mzH;
            float mzCi;
            float mzCi_1;
            float mzSumaFx;

            mzH = mzG - mzD;

            float mzSumaFaFb = mzSumaSzereguPotęgowego(mzD, mzEps) + mzSumaSzereguPotęgowego(mzG, mzEps);
            mzCi = mzH * mzSumaFaFb;
            int mzLicznikIteracji = 1;

            do
            {
                mzCi_1 = mzCi;
                mzLicznikIteracji++;
                mzH = (mzG - mzD) / mzLicznikIteracji;
                mzSumaFx = 0.0F;

                for (int j = 1; j < mzLicznikIteracji; j++)
                {
                    mzSumaFx = mzSumaFx + mzSumaSzereguPotęgowego(mzD + j * mzH, mzEps);
                }

                mzCi = mzH * (mzSumaFaFb + mzSumaFx);
            } while (Math.Abs(mzCi - mzCi_1) > mzEps);

            return mzCi;

        }

        private void txtDolnaGranica_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTabWizual_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            float mzX, mzXd, mzXg, mzH, mzEps;
            dataGridView1.Visible = true;
            txtEps.Enabled = true;

            if (!float.TryParse(txtEps.Text, out mzEps))
            {
                errorProvider1.SetError(txtEps, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtXd.Text, out mzXd))
            {
                errorProvider1.SetError(txtXd, "ERROR: w podanej wartości zmiennej Xd wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtXg.Text, out mzXg))
            {
                errorProvider1.SetError(txtXg, "ERROR: w podanej wartości zmiennej Xg wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtKrokH.Text, out mzH))
            {
                errorProvider1.SetError(txtKrokH, "ERROR: w podanej wartości zmiennej H wystąpił niedozwolony znak!!!");
                return;
            }

            float mzSumaSzeregu;

            for (mzX = mzXd; mzX <= mzXg; mzX = mzX + mzH)
            {
                mzSumaSzeregu = mzSumaSzereguPotęgowego(mzX, mzEps);

                dataGridView1.Rows.Add(mzX, mzSumaSzeregu);
            }

            txtEps.Enabled = false;
            txtXd.Enabled = false;
            txtXg.Enabled = false;
            txtKrokH.Enabled = false;
            btnTabWizual.Enabled = false;
        }

        private void btnObliczanieFunkcji_Click(object sender, EventArgs e)
        {
            float mzX, mzEps;

            errorProvider1.Dispose();

            if (!float.TryParse(txtWartoscX.Text, out mzX))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej X wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtEps.Text, out mzEps))
            {
                errorProvider1.SetError(txtEps, "ERROR: w podanej wartości dokładności obliczeń Eps wystąpił niedozwolony znak!!!");
                return;
            }

            txtWartoscX.Enabled = false;

            float mzObliczonaSumaSzeregu;
            mzObliczonaSumaSzeregu = mzSumaSzereguPotęgowego(mzX, mzEps);

            txtObliczonaSumaSzeregu.Text = string.Format("{0:0.000}", mzObliczonaSumaSzeregu);

            btnObliczanieFunkcji.Enabled = false;
        }

        private void btnObliczCalke_Click(object sender, EventArgs e)
        {
            float mzD, mzG, mzEps;

            errorProvider1.Dispose();


            if (!float.TryParse(txtDolnaGranica.Text, out mzD))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej d wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtGornaGranica.Text, out mzG))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej g wystąpił niedozwolony znak!!!");
                return;
            }


            if (!float.TryParse(txtDokladnosc.Text, out mzEps))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                return;
            }

            txtDolnaGranica.Enabled = false;
            txtGornaGranica.Enabled = false;
            txtDokladnosc.Enabled = false;

            float mzObliczonaCalka;
            mzObliczonaCalka = mzCalkaMetodaTrapezow(mzD, mzG, mzEps);

            txtResultCalka.Text = string.Format("{0:0.000}", mzObliczonaCalka);

            btnObliczCalke.Enabled = false;

        }

        private void btnResetuj_Click(object sender, EventArgs e)
        {
            txtWartoscX.Clear();
            txtWartoscX.Enabled = true;
            txtEps.Clear();
            txtXd.Clear();
            txtXg.Clear();
            txtKrokH.Clear();
            txtEps.Enabled = true;
            txtXd.Enabled = true;
            txtXg.Enabled = true;
            txtKrokH.Enabled = true;
            txtObliczonaSumaSzeregu.Clear();
            txtResultCalka.Clear();
            txtResultCalka.Enabled = false;
            txtObliczonaSumaSzeregu.Enabled = false;
            txtDolnaGranica.Clear();
            txtGornaGranica.Clear();
            txtDokladnosc.Clear();
            txtDolnaGranica.Enabled = true;
            txtGornaGranica.Enabled = true;
            txtDokladnosc.Enabled = true;
            btnObliczanieFunkcji.Enabled = true;
            btnObliczCalke.Enabled = true;
            btnTabWizual.Enabled = true;
            btnGrafikWizual.Enabled = true;
            chart1.BackColor = Color.White;
            chart1.Series[0].Color = Color.DodgerBlue;
            this.ForeColor = Color.Black;
            txtKolotTla.BackColor = Color.White;
            txtKolorLinii.BackColor = Color.DodgerBlue;
            chart1.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            chart1.Series[0].BorderWidth = 1;
            chart1.Visible = false;
            dataGridView1.Visible = false;
        }

        private void kolorTłaWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = this.chart1.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                this.chart1.BackColor = colorDialog1.Color;

            txtKolotTla.BackColor = this.chart1.BackColor;
        }

        private void txtKolotTla_TextChanged(object sender, EventArgs e)
        {

        }

        private void kolorLiniiWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = chart1.Series[0].Color;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                chart1.Series[0].Color = colorDialog1.Color;

            txtKolorLinii.BackColor = chart1.Series[0].Color;
        }

        private void kolorCzcionkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = this.ForeColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                this.ForeColor = colorDialog1.Color;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void kropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderDashStyle = ChartDashStyle.Dot;
        }

        private void kreskowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderDashStyle = ChartDashStyle.Dash;
        }

        private void kreskowokropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
        }

        private void ciągłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderDashStyle = ChartDashStyle.Solid;
        }

        private void rbUkladWspZOpisem_CheckedChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Title = "Wartości zmiennej X";
            chart1.ChartAreas[0].AxisY.Title = "Wartości funkcji F(X)";
            chart1.Series[0].IsVisibleInLegend = true;
            chart1.Legends.FindByName("Legend1").Docking = Docking.Bottom;
        }

        private void rbUkladWspBezOpisu_CheckedChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Title = " ";
            chart1.ChartAreas[0].AxisY.Title = " ";
            chart1.Series[0].IsVisibleInLegend = false;
        }

        private void lblUkladWspolrz_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderWidth = 1;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderWidth = 2;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderWidth = 3;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderWidth = 4;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            chart1.Series[0].BorderWidth = 5;
        }

        private void stylCzcionkitabelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = this.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Font = fontDialog1.Font;
                foreach (Control mzKontrolki in this.Controls)
                    mzKontrolki.Font = fontDialog1.Font;
            }
        }

        private void zapiszTablicęWPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Pliki tekstowe (*.txt)|";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Title = "Zapisanie tablicy w pliku";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter PlikZnakowy =
                    new System.IO.StreamWriter(saveFileDialog1.OpenFile());

                try
                {
                    float mzX, mzXd, mzXg, mzH, mzEps;
                    dataGridView1.Visible = true;
                    txtEps.Enabled = true;

                    if (!float.TryParse(txtEps.Text, out mzEps))
                    {
                        errorProvider1.SetError(txtEps, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                        return;
                    }

                    if (!float.TryParse(txtXd.Text, out mzXd))
                    {
                        errorProvider1.SetError(txtXd, "ERROR: w podanej wartości zmiennej Xd wystąpił niedozwolony znak!!!");
                        return;
                    }

                    if (!float.TryParse(txtXg.Text, out mzXg))
                    {
                        errorProvider1.SetError(txtXg, "ERROR: w podanej wartości zmiennej Xg wystąpił niedozwolony znak!!!");
                        return;
                    }

                    if (!float.TryParse(txtKrokH.Text, out mzH))
                    {
                        errorProvider1.SetError(txtKrokH, "ERROR: w podanej wartości zmiennej H wystąpił niedozwolony znak!!!");
                        return;
                    }

                    float mzSumaSzeregu;

                    for (mzX = mzXd; mzX <= mzXg; mzX = mzX + mzH)
                    {
                        mzSumaSzeregu = mzSumaSzereguPotęgowego(mzX, mzEps);

                        PlikZnakowy.Write(string.Format("\t{0:0.0000};", mzX));
                        PlikZnakowy.Write(string.Format("\t{0:0.0000};\n", mzSumaSzeregu));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: nie udało się otworzyć pliku w pamięci " +
                        "zewnętrznej lub wystąpił błąd przy wpisywaniu danych - oficjalny "+
                        "komunikat o przyczynie błędu: " + ex.Message);
                }
                finally
                {
                    PlikZnakowy.Dispose();
                    PlikZnakowy.Close();
                }

                MessageBox.Show("Zapisywanie tablicy w pliku udało sie!");
            }
        }

        private void odczytajTablicęZPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.Visible)
                dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();

            OpenFileDialog OknoOdczytuPliku = new OpenFileDialog();
            OknoOdczytuPliku.Title = "Odczytywanie tablicy z pliku";

            if (OknoOdczytuPliku.ShowDialog() == DialogResult.OK)
            {
                string NazwaPliku = OknoOdczytuPliku.FileName;
                string[] ZawartoscPliku = File.ReadAllLines(NazwaPliku);
                foreach (string WierszDanych in ZawartoscPliku)
                    dataGridView1.Rows.Add(WierszDanych.Split(';'));
            }
            MessageBox.Show("Odczytywanie tablicy z pliku udało sie!");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult mzOdpowiedzUzytkownika = MessageBox.Show("Czy rzeczywiście chcesz zakończyć "+
                "(wyjść) działania programu (możesz utracić dane umieszczone na wszystkich otwartych formularzach)?",
                this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            if (mzOdpowiedzUzytkownika == DialogResult.Yes)
                Application.Exit();

            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void wyzerujWszystkieKontrolkiTypuTextBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtWartoscX.Clear();
            txtWartoscX.Enabled = true;
            txtEps.Clear();
            txtXd.Clear();
            txtXg.Clear();
            txtKrokH.Clear();
            txtEps.Enabled = true;
            txtXd.Enabled = true;
            txtXg.Enabled = true;
            txtKrokH.Enabled = true;
            txtObliczonaSumaSzeregu.Clear();
            txtResultCalka.Clear();
            txtResultCalka.Enabled = false;
            txtObliczonaSumaSzeregu.Enabled = false;
            txtDolnaGranica.Clear();
            txtGornaGranica.Clear();
            txtDokladnosc.Clear();
            txtDolnaGranica.Enabled = true;
            txtGornaGranica.Enabled = true;
            txtDokladnosc.Enabled = true;
            txtFloatA.Enabled = true;
            txtFloatB.Enabled = true;
            txtEpsSzeregu.Enabled = true;
            txtEpsCalkowania.Enabled = true;
            txtFloatA.Clear();
            txtFloatB.Clear();
            txtEpsSzeregu.Clear();
            txtEpsCalkowania.Clear();
            txtProstokatowResult.Clear();
        }

        private void uaktywnijWszystkieKontrolkiTypuButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnObliczanieFunkcji.Enabled = true;
            btnObliczCalke.Enabled = true;
            btnTabWizual.Enabled = true;
            btnGrafikWizual.Enabled = true;
            btObliczProstokatow.Enabled = true;
            button1.Enabled = false;
        }

        private void btObliczProstokatow_Click(object sender, EventArgs e)
        {
            float mzA, mzB, mzEpsSzeregu, mzEpsCalkowania;

            errorProvider1.Dispose();


            if (!float.TryParse(txtFloatA.Text, out mzA))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej d wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtFloatB.Text, out mzB))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej g wystąpił niedozwolony znak!!!");
                return;
            }


            if (!float.TryParse(txtEpsSzeregu.Text, out mzEpsSzeregu))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtEpsCalkowania.Text, out mzEpsCalkowania))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                return;
            }

            txtFloatA.Enabled = false;
            txtFloatB.Enabled = false;
            txtEpsSzeregu.Enabled = false;
            txtEpsCalkowania.Enabled = false;

            float mzObliczonaCalka;
            mzObliczonaCalka = mzCalkaMetodaProstokatow(mzEpsSzeregu, mzA, mzB, mzEpsCalkowania, out int mzLicznikPrzedzialow ) ;

            txtProstokatowResult.Text = string.Format("{0:0.000}", mzObliczonaCalka);

            btObliczProstokatow.Enabled = false;

        }

        static float mzCalkaMetodaProstokatow(float mzEpsSzeregu, float mzA, float mzB, float mzEpsCalkowania, out int mzLicznikPrzedzialow) 
        {
            float mzH, mzCi, mzCi_1, mzSumaFx;
            float mzSzerokoscPrzedzialu;
            float mzX;

            mzLicznikPrzedzialow = 1;
            mzCi = (mzB - mzA) * mzSumaSzereguPotęgowego((mzA + mzB) / 2.0F, mzEpsSzeregu);

            do
            {
                mzCi_1 = mzCi;
                mzLicznikPrzedzialow = mzLicznikPrzedzialow + mzLicznikPrzedzialow;
                mzH = (mzB - mzA) / mzLicznikPrzedzialow;
                mzX = mzA + mzH / 2.0F;
                mzSumaFx = 0.0F;
                for (ushort i = 0; i < mzLicznikPrzedzialow; i++)
                {
                    mzSumaFx += mzSumaSzereguPotęgowego(mzX + i * mzH, mzEpsSzeregu);
                }
                mzCi = mzH * mzSumaFx;
            } while (Math.Abs(mzCi - mzCi_1) > mzEpsCalkowania);

            mzSzerokoscPrzedzialu = mzH;
            return mzCi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Visible = true;

            float mzX, mzA, mzB, mzEpsSzeregu, mzEpsCalkowania;

            if (!float.TryParse(txtFloatA.Text, out mzA))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej d wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtFloatB.Text, out mzB))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej g wystąpił niedozwolony znak!!!");
                return;
            }


            if (!float.TryParse(txtEpsSzeregu.Text, out mzEpsSzeregu))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                return;
            }

            if (!float.TryParse(txtEpsCalkowania.Text, out mzEpsCalkowania))
            {
                errorProvider1.SetError(txtWartoscX, "ERROR: w podanej wartości zmiennej Eps wystąpił niedozwolony znak!!!");
                return;
            }

            float mzObliczonaCalka;

            for (mzX = 0; mzX <= mzB; mzX += 1)
            {
                mzObliczonaCalka = mzCalkaMetodaProstokatow(mzEpsSzeregu, mzA, mzB, mzEpsCalkowania, out int mzLicznikPrzedzialow);

                dataGridView1.Rows.Add(mzX, mzObliczonaCalka);
            }

            button1.Enabled = false;
        }
    }
}
