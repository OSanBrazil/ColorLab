using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ColorLab
{
    public partial class Form1 : Form
    {
        Bitmap imagemcarregada;
        String arquivo;

        public Form1()
        {
            InitializeComponent();
            this.Text = "COLORLAB - MANOEL";
           
        }

        public void UpdateProperties(Form form)
        {
            // Get all buttons on the form
            var paineis = form.Controls.OfType<Panel>();
            // Update properties for each panel
            foreach (var painel in paineis) { painel.BackColor = Color.Black; }

            // Get all labels on the form
            var rotulos = form.Controls.OfType<Label>();
            // Update properties for each label
            foreach (var rotulo in rotulos) { rotulo.Text = "0"; }

            var caixasdeimagem = form.Controls.OfType<PictureBox>();
            foreach (var caixa in caixasdeimagem) {  caixa.BackColor = Color.Black; };

            button1.Text = "Black and White";
            button2.Text = "Mono Blue";
            button3.Text = "Mono Green";
            button4.Text = "Mono Red";
            button5.Text = "RESET";

        }

       

        private void updateColors()
        {
            int r = trackBar1.Value;
            int g = trackBar2.Value;
            int b = trackBar3.Value;

            panel1.BackColor = Color.FromArgb(r, 0, 0);
            panel2.BackColor = Color.FromArgb(0, g, 0);
            panel3.BackColor = Color.FromArgb(0, 0, b);
            label1.Text = r.ToString();
            label2.Text = g.ToString();
            label3.Text = b.ToString();

            panel4.BackColor = Color.FromArgb(r, g, b);
            label4.Text = $"{r}, {g}, {b}";
           
            int lum = (r + g + b) / 3;

            panel5.BackColor = Color.FromArgb(lum, lum, lum);
            label5.Text = lum.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateProperties(this);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void carregarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            arquivo = openFileDialog1.FileName;
            imagemcarregada = new Bitmap(arquivo);
            pictureBox1.Image = imagemcarregada;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           transform(0,0,0);
           
        }

        private void transform(int r, int g, int b)
        {
            try
            {
                // we pull the bitmap from the image
                Bitmap bmp = (Bitmap)imagemcarregada;

                // we change some picels
                for (int y = 0; y < bmp.Height; y++)
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color c = bmp.GetPixel(x, y);
                        int media = (c.R + c.G + c.B) / 3;
                        bmp.SetPixel(x, y, Color.FromArgb(media | r, media |g, media | b));
                    }
                // we need to re-assign the changed bitmap
                pictureBox1.Image = (Bitmap)bmp;
            }
            catch { MessageBox.Show("Imagem não carregada ou erro na Imagem!"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            transform(0, 0, 255);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            transform(0, 255, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            transform(255, 0, 0);
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                imagemcarregada = new Bitmap(arquivo);
                pictureBox1.Image = imagemcarregada;


            }
            catch { }
            finally
            {
                trackBar1.Value = 0;
                trackBar2.Value = 0;
                trackBar3.Value = 0;
            }

            }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            updateColors();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            updateColors();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            updateColors();
        }

        private void panel4_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            label4.Text = "Click=Transform!";
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            transform(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }
    }
}
