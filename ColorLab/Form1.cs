using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ColorLab
{
   
    public partial class Form1 : Form
    {
        Form2 form_processando = new Form2();
        Form3 form_debug = new Form3();
        Form form_param = new Form();
       
        Bitmap imagemcarregada;
        Bitmap bmp_anterior;
        String arquivo;
        String titulo = "COLORLAB - MANOEL ";
        String selected_effect = "";
        // int blocksize;
        const float PI = 3.141592f;
        Random rnd = new Random();

        Form form_binary = new Form4();

        public Form1()
        {
            InitializeComponent();
            this.Text = titulo;
            openFileDialog1.Filter = "Image Files| *.BMP; *.JPG; *.JPEG; *.PNG; *.GIF;"; //(BMP/JPG/JPEG/PNG/GIF)
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateProperties(this);
            desfazerÚltimoToolStripMenuItem.Enabled = false;
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

            label6.Text = "";

            var caixasdeimagem = form.Controls.OfType<PictureBox>();
            foreach (var caixa in caixasdeimagem) { caixa.BackColor = Color.Black; };

            button1.Text = "Black and White";
            button2.Text = "Mono Blue";
            button3.Text = "Mono Green";
            button4.Text = "Mono Red";
            button5.Text = "RESET";
            button6.Text = "Get RGB from Image";
        }

        private void desativaBotoes(Form form)
        {
            var botoes = form.Controls.OfType<System.Windows.Forms.Button>();
            foreach (var botao in botoes) { botao.Enabled = false; }
            form_processando.StartPosition = FormStartPosition.CenterScreen;
            form_processando.Show();
            form_processando.Refresh();
            //form_debug.Show();
            //form_debug.Refresh();
        }

        private void ativaBotoes(Form form)
        {
            var botoes = form.Controls.OfType<System.Windows.Forms.Button>();
            foreach (var botao in botoes) { botao.Enabled = true; }
            selected_effect = "";
            form_processando.Hide();
        }

        private void updateColors()
        {
            int r = trackBar1.Value;
            int g = trackBar2.Value;
            int b = trackBar3.Value;

            panel1.BackColor = Color.FromArgb(r, 0, 0);
            panel2.BackColor = Color.FromArgb(0, g, 0);
            panel3.BackColor = Color.FromArgb(0, 0, b);
            label1.Text = r.ToString() + " (#" + r.ToString("X") + ")";
            label2.Text = g.ToString() + " (#" + g.ToString("X") + ")"; ;
            label3.Text = b.ToString() + " (#" + b.ToString("X") + ")"; ;

            panel4.BackColor = Color.FromArgb(r, g, b);
            label4.Text = $"{r}, {g}, {b}";

            int lum = (r + g + b) / 3;

            panel5.BackColor = Color.FromArgb(lum, lum, lum);
            label5.Text = lum.ToString() + " (#" + lum.ToString("X") + ")";

        }

        


        private void carregarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (imagemcarregada != null)
            {
                imagemcarregada.Dispose();
            }
            arquivo = openFileDialog1.FileName;
            imagemcarregada = new Bitmap(arquivo);
            pictureBox1.Image = imagemcarregada;
            label6.Text = $"Dimensões da imagem: X = {imagemcarregada.Width}, Y = {imagemcarregada.Height}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            transform(0, 0, 0);
        }

        private void transform(int r, int g, int b)
        {
            desativaBotoes(this);


            try
            {

                // we pull the bitmap from the image
                bmp_anterior = new Bitmap (imagemcarregada);
          
                Bitmap bmp = imagemcarregada;
                
                int altura = bmp.Height;
                int largura = bmp.Width;
                int progresso;
                float factor = 1.0f;
                int corR, corG, corB;
                int fator_ruido = 100;
                int blocksize = 8;

                if (selected_effect == "Xadrez")
                {
                    blocksize = recebeParametros("Quantidade de blocos (horiz.)", 2, 64, 8);
                    blocksize = largura * 2 / blocksize;
                }

                if (selected_effect == "Ruido")
                {
                    fator_ruido = recebeParametros("Intensidade do ruído", 10, 400, 100);
                }

                // we change some pixels
                for (int y = 0; y < altura; y++)
                {
                    for (int x = 0; x < largura; x++)
                    {


                        Color c = bmp.GetPixel(x, y);
                        int media = (c.R + c.G + c.B) / 3;

                        if (r + g + b == 765 | selected_effect == "Xadrez")
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                        }
                        if (selected_effect == "Xadrez" & (x % blocksize > blocksize / 2 & y % blocksize > blocksize / 2 | x % blocksize < blocksize / 2 & y % blocksize < blocksize / 2))
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(c.R, c.G, c.B));
                        }

                        if (r + g + b > 0 & r + g + b < 765)
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(media * r / 255, media * g / 255, media * b / 255));
                        }
                        if (r + g + b == 0)
                        {

                            bmp.SetPixel(x, y, Color.FromArgb(media | r, media | g, media | b));

                        }
                        if (selected_effect == "Vinheta")
                        {
                            factor = Math.Abs((float)Math.Sin(y / (float)altura * PI) * (float)Math.Sin(x / (float)largura * PI));
                            corR = (int)(c.R * factor);
                            corG = (int)(c.G * factor);
                            corB = (int)(c.B * factor);

                            //form_debug.textBox1.AppendText($"{PI}->{factor}->{corR}-{corG}-{corB}/");

                            bmp.SetPixel(x, y, Color.FromArgb(corR, corG, corB));
                        }

                        if (selected_effect == "Ruido")
                        {
                            corR = c.R + rnd.Next(fator_ruido * 2) - fator_ruido;
                            corG = c.G + rnd.Next(fator_ruido * 2) - fator_ruido;
                            corB = c.B + rnd.Next(fator_ruido * 2) - fator_ruido;

                            corR = Math.Max(Math.Min(corR, 255), 0);
                            corG = Math.Max(Math.Min(corG, 255), 0);
                            corB = Math.Max(Math.Min(corB, 255), 0);

                            bmp.SetPixel(x, y, Color.FromArgb(corR, corG, corB));
                        }

                        if (selected_effect == "Estratificar")
                        {
                            corR = c.R;
                            corG = c.G;
                            corB = c.B;

                            if (corR > 127) { corR = 255; } else { corR = 0; };
                            if (corG > 127) { corG = 255; } else { corG = 0; };
                            if (corB > 127) { corB = 255; } else { corB = 0; };

                            bmp.SetPixel(x, y, Color.FromArgb(corR, corG, corB));
                        }
                    }

                    // add code to show percentage here!
                    progresso = y * 100 / altura;

                    if (progresso % 5 == 0)
                    {
                        form_processando.label1.Text = $"Processando: {progresso}%";
                        form_processando.progressBar1.Value = 100 - progresso;
                        form_processando.Refresh();
                    }

                }

                loadImage(bmp);
               

            }
            catch (Exception) { MessageBox.Show("Imagem não carregada ou erro na Imagem!"); } // MessageBox.Show(ex.ToString()); }
            ativaBotoes(this);

        }


        private void loadImage(Bitmap img)
        {
            pictureBox1.Image = img;
            desfazerÚltimoToolStripMenuItem.Enabled = true;
        }

        private int recebeParametros(string nome_param, int min_param, int max_param, int std_param)
        {
            int param = -1;

            form_param.Controls.Clear();
           
            form_param.Text = selected_effect;
            form_param.StartPosition = FormStartPosition.CenterScreen;
            form_param.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Label label_parametro = new Label();
            Button botao_enter = new Button();
            TextBox entrada_parametro = new TextBox();
            entrada_parametro.Width = 32;
            entrada_parametro.TextAlign = HorizontalAlignment.Center;
            entrada_parametro.Text = std_param.ToString();

            form_param.Controls.Add(label_parametro);
            label_parametro.AutoSize = true;
            label_parametro.Text = $"{nome_param}: ({min_param}-{max_param})?";

            form_param.Controls.Add(entrada_parametro);

            form_param.Controls.Add(botao_enter);
            botao_enter.Text = "OK";
            botao_enter.Click += new EventHandler(botao_enter_click);
            // Assuming you have a control named 'myControl' and a form named 'myForm'
            int y = 0;
            foreach (Control myControl in form_param.Controls)
            {
                int x = (form_param.ClientSize.Width - myControl.Width) / 2;
                myControl.Location = new Point(x, y);
                y += 26;
            }
            form_param.Height = y + 48;
            form_param.ShowDialog();
            try
            {
                param = int.Parse(entrada_parametro.Text);
                if (param < min_param | param > max_param) { throw new ArgumentException(); }
            }
            catch
            {
                MessageBox.Show($"Valor inválido, carregado valor padrão ({std_param})!");
                param = std_param;
            }
            
            return param;
        }

        private void botao_enter_click(object sender, EventArgs e)
        {
            form_param.Close();  
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                imagemcarregada = new Bitmap(arquivo);
                pictureBox1.Image = imagemcarregada;
                desfazerÚltimoToolStripMenuItem.Enabled = false;
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


        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            label4.Text = "Click=Transform";
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            transform(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        private void panel5_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Click=Invert";
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            transform(255, 255, 255);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (button6.Enabled == false)
            {
                capturePixel();
            }

        }

        private void capturePixel()
        {
            Color c = GetColorAtMousePosition();
            int r = c.R; int g = c.G; int b = c.B;

            trackBar1.Value = r;
            trackBar2.Value = g;
            trackBar3.Value = b;

            updateColors();
        }

        private Color GetColorAtMousePosition()
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            Point mousePosition = this.PointToClient(Cursor.Position);
            return bmp.GetPixel(mousePosition.X + 8, mousePosition.Y + 32);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            button6.Enabled = true;
        }

        private void pretoEBrancoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void monoAzulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.PerformClick();
        }

        private void monoVerdeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3.PerformClick();
        }

        private void monoVermelhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.PerformClick();
        }

        private void negativoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transform(255, 255, 255);
        }

        private void aplicarRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transform(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button5.PerformClick();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
           

        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void xadrezToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected_effect = "Xadrez";
         
            transform(255, 255, 255);
        }

        private void vinhetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected_effect = "Vinheta";
            transform(-1, -1, -1);
        }

        private void ruídoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected_effect = "Ruido";
            transform(-2, -2, -2);
        }

        private void estratificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected_effect = "Estratificar";
            transform(-3, -3, -3);
        }

        private void efeitosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void copiarParaÁreaDetransferênciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(pictureBox1.Image);
                MessageBox.Show("Imagem copiada para área de transferência!");
            }
            catch { MessageBox.Show("Sem imagem carregada!"); }
        }

        private void desfazerÚltimoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadImage(bmp_anterior);
            imagemcarregada = bmp_anterior;
            desfazerÚltimoToolStripMenuItem.Enabled = false;
        }

        private void conversorBinárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            form_binary.Show();
            form_binary.Focus();
            Form4.numericUpDowns[2].Value = trackBar1.Value;
            Form4.numericUpDowns[3].Value = trackBar2.Value;
            Form4.numericUpDowns[4].Value = trackBar3.Value;
        }
    }
}
