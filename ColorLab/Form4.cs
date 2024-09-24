﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorLab
{
    public partial class Form4 : Form
    {
        CheckBox[] checkBoxes = new CheckBox[33];
        Label[] labels = new Label[5];
        NumericUpDown[] numericUpDowns = new NumericUpDown[5];

        string[] labelsText = { "1 bit  ->", "8 bit (1 Byte)  ->", "R  ->", "G  ->", "B  ->" };
        string[] binValue = new string[5];
        string binSequence;
        int std_Height = 28;


        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.Text = "Conversor Binário";

            int x = 7, y = 0, ly = 0;


            for (int i = 0; i < 33; i++)
            {


                if ((i - 1) % 8 == 0) { y++; x = 0; }

                if (i == 9) { y++; }
                checkBoxes[i] = new CheckBox();
                this.Controls.Add(checkBoxes[i]);
                checkBoxes[i].Checked = false;
                checkBoxes[i].Height = std_Height;
                checkBoxes[i].Width = checkBoxes[i].Height;
                checkBoxes[i].Appearance = Appearance.Button;
                checkBoxes[i].BackColor = Color.Black;
                checkBoxes[i].CheckedChanged += new EventHandler(checkBoxes_Changed);
                Point controlLocation = new Point((x + 6) * checkBoxes[i].Width, (y + 1) * 2 * checkBoxes[i].Height);
                checkBoxes[i].Location = controlLocation;

                x++;
            }

            for (int i = 0; i < 5; i++)
            {
                labels[i] = new Label();
                numericUpDowns[i] = new NumericUpDown();
                labels[i].Height = std_Height;
                labels[i].TextAlign = ContentAlignment.MiddleRight;
                labels[i].Text = labelsText[i];
                labels[i].Location = new Point(checkBoxes[1].Left - labels[i].Width - std_Height, (ly + 1) * 2 * std_Height);
                numericUpDowns[i].Font = new Font(Font.FontFamily, 10);
                numericUpDowns[i].Width = std_Height * 2;
                numericUpDowns[i].Location = new Point(checkBoxes[1].Left + std_Height * 9, (ly + 1) * 2 * std_Height + 2);
                if (i == 0) { numericUpDowns[i].Maximum = 1; }
                else { numericUpDowns[i].Maximum = 255; }
                this.Controls.Add(labels[i]);
                this.Controls.Add(numericUpDowns[i]);
                numericUpDowns[i].ValueChanged += new EventHandler(numericUpDowns_Changed);
                ly++;
                if (i == 1) { ly++; }
            }

            button1.Location = new Point(checkBoxes[1].Left + std_Height * 9, (ly + 1) * 2 * std_Height + 2);


        }
        private void checkBoxes_Changed(object sender, EventArgs e)
        {
            binSequence = "";
            foreach (CheckBox checkBox in checkBoxes)
            {
                if (checkBox.Checked == true) { checkBox.BackColor = Color.White; binSequence += 1; }
                else { checkBox.BackColor = Color.Black; binSequence += 0; }

            }

            this.Text = binSequence;
        }

        private void numericUpDowns_Changed(object sender, EventArgs e)
        {
            int number;
            string newBinSequence;
            newBinSequence = "";
            newBinSequence += numericUpDowns[0].Value;
            for (int i = 1; i < 5; i++)
            {
                number = (int)numericUpDowns[i].Value;
                newBinSequence += Convert.ToString(number, 2).PadLeft(8, '0');
            }
            UpdateDisplay(newBinSequence);
            

        }
        private void UpdateDisplay(string newBinSequence)
        {
            int n = 0;
            foreach (CheckBox checkBox in checkBoxes)
            {
                if (newBinSequence[n] == '1')
                {
                    checkBox.Checked = true;
                }
                else
                {
                    checkBox.Checked = false;
                }
                n++;
            }

        }
        private void Form4_Shown(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (NumericUpDown numericUpDown in numericUpDowns)
            {
                numericUpDown.Value = 0;
            }
        }
    }
}