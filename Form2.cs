using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static SpeechRecognition.Form1;
using System.IO;
using Microsoft.Speech.Recognition;

namespace SpeechRecognition
{
    public partial class Form2 : Form
    {
        private Form1 form1Instance;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            form1Instance = form1;
        }

        public Form2()
        {
        }
        string pathtofile;
        public void NewWord(string command,string URL)
        {
            string path = @"C:\Users\User\Desktop\КАИ\2 курс\2 семак\Практика\0\команды2.txt";
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(command+"|"+URL);
                sw.Close();
            }
        }

        static void Obnulenie(Form2 f2)
        {
            f2.label3.Visible = false;
            f2.label4.Visible = false;
            f2.textBox3.Visible = false;
            f2.button3.Visible = false;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Obnulenie(this);
            comboBox1.Items.Add("Видео или аудио файл с ПК");
            comboBox1.Items.Add("Видеофайл с WEB");
            comboBox1.Items.Add("Набор музыки");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Choices choices = new Choices();
            //choices = c1.BuildGr(choices);
            //startspe(choices);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var p = new ComFile();
            p.Form1Instance = form1Instance;
            if (comboBox1.Text == "Видеофайл с WEB")
            {
                NewWord(textBox1.Text, textBox3.Text);
                form1Instance.listBox1.Items.Clear();
                p.settingListBox();
                Obnulenie(this);
                Choices choices = new Choices();
                choices = c1.BuildGr(choices);
                startspe(choices);
            }
            else if ((comboBox1.Text == "Видео или аудио файл с ПК") || (comboBox1.Text == "Набор музыки"))
            {

                NewWord(textBox1.Text, pathtofile);
                form1Instance.listBox1.Items.Clear();
                p.settingListBox();
                Obnulenie(this);
                Choices choices = new Choices();
                choices = c1.BuildGr(choices);
                startspe(choices);
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Добавьте команду");
                // p.settingListBox();
                Obnulenie(this);
                //Choices choices = new Choices();
                //choices = c1.BuildGr(choices);
                //startspe(choices); ;
                return;
            }
            textBox1.Text = null;
            textBox3.Text = null;
            comboBox1.Text = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Видеофайл с WEB")
            {
                Obnulenie(this);
                textBox3.Visible = true;
                label4.Visible = true;
            }
            else if ((comboBox1.Text == "Видео или аудио файл с ПК") ||(comboBox1.Text == "Набор музыки"))
            {
                Obnulenie(this);
                button3.Visible = true;
                label3.Visible = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((comboBox1.Text == "Видео или аудио файл с ПК")) pathtofile = setting();
            else if (comboBox1.Text == "Набор музыки") pathtofile = setting2();
        }
    }
}
