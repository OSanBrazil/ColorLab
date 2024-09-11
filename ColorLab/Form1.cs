using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ColorLab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        public void UpdateProperties(Form form)
        {
            // Get all buttons on the form
            var paineis = form.Controls.OfType<Panel>();
            
            // Update properties for each button
            foreach (var painel in paineis)
            {
                painel.BackColor = Color.Black; // Example property update
            }
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            updateColors();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            updateColors();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            updateColors();
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
    }
}
