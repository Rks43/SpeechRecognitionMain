using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Media;
using NAudio;
using WMPLib;
using System.Threading;
using System.IO;
using System.Windows.Media;
using System.Runtime.InteropServices;
using static SpeechRecognition.Form2;

namespace SpeechRecognition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
        static SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
        public Label Label1 { get { return label1; } }
        static int k = 0;
        static SoundPlayer player;
        static string musicFolderPath = string.Empty;
        public static ComFile c1 = new ComFile();
        static Choices numbers;
     //   private ComFile c1;

        public Form2 Form2Instance { get; set; }

         public static void startspe(Choices numbers)
            {
                sre.UnloadAllGrammars();
                GrammarBuilder gb = new GrammarBuilder();
                gb.Culture = ci;
                gb.Append(numbers);
                Grammar g = new Grammar(gb);
                sre.LoadGrammar(g);
        }
        public class ComFile
        {
            public Form1 Form1Instance { get; set; }
            string path = @"C:\Users\User\Desktop\КАИ\2 курс\2 семак\Практика\0\команды2.txt";

            public void ClearListBox()
            {
                
            }

            public void settingListBox()
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    while (((s = sr.ReadLine()) != null))
                    {
                        string[] st = s.Split('|');
                        Form1Instance.listBox1.Items.Add(st[0]);
                    }
                    sr.Close();
                }
            }

            public Choices BuildGr(Choices numbers)
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(s)) // Skip empty strings
                        {
                            string[] st = s.Split('|');
                            numbers.Add(st[0]);
                        }
                    }
                    return numbers;
                    sr.Close();
                }
            }
           
        }





        // Метод обработки для музыки
        public static void muzika(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Contains("музыку") || (e.Result.Text.Contains("стоп")) || (e.Result.Text.Contains("переключи")))
                    {
                if (player == null)
                            {
                               MessageBox.Show("Настройте папку");
                            }
                
                else  if (e.Result.Text == "включи музыку") player.Play();
                else  if (e.Result.Text == "стоп") player.Stop();
                else  if (e.Result.Text == "переключи")
                {
                    k++;
                    string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.wav");
                    if (k >= musicFiles.Length)
                    {
                        k = 0;
                    }
                    player = new SoundPlayer(musicFiles[k]);
                    player.Play();
                  //  MessageBox.Show("Всё работает");
                }
            }
        }

        // Настройки папки с музыкой
        public static string setting()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.mp4|All Files|*.*"; 
            openFileDialog.Title = "Select an audio/video file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                return selectedFilePath;
            }
            return "";
        }

        public static string setting2()
        {
            player = null;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
               string m1 = folderBrowserDialog.SelectedPath;
                return m1;
            }
            else
            {
                return "";
            }

            string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.wav");

            if (musicFiles.Length > 0)
            {
                player = new SoundPlayer(musicFiles[0]);
                player.Stop();

            }
            else
            {
                MessageBox.Show("В выбранной папке нет музыкальных файлов.");
            }
        }



        // Обработка движка распознания 
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Video video = new Video();
            Form1 form1Instance = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1Instance != null)
            {
                form1Instance.Invoke(new Action(() =>
                {
                    form1Instance.Label1.Text = e.Result.Text;
                }));
            }

            string URL = progonka(e.Result.Text);
            if (e.Result.Text == "закрыть видео") video.closevideo();
            else if (e.Result.Text == "полный экран") video.fullscreen();
            else if (URL.Contains("https"))
                {
                Process.Start(URL);
                Thread.Sleep(2000);
                SendKeys.SendWait("{f}");
            }
            else if ((URL.Contains(".mp4")) || (URL.Contains(".mp3")) || (URL.Contains(".wav")))
            {
                try
                {
                    Process.Start(URL);
                }
                catch
                {
                    MessageBox.Show("Не правильно указан путь");
                }
            }
            else if (URL.Length > 0)
            {
                player = null;
                try
                {
                    musicFolderPath = URL;
                    string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.wav");

                    if (musicFiles.Length > 0)
                    {
                        player = new SoundPlayer(musicFiles[0]);
                        player.Stop();

                    }
                }
                catch
                {
                  MessageBox.Show("В выбранной папке нет музыкальных файлов.");
                    
                }
            }
        }

        static string progonka(string st)
        {
            string path = @"C:\Users\User\Desktop\КАИ\2 курс\2 семак\Практика\0\команды2.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(s)) // Skip empty strings
                    {
                        string[] sk = s.Split('|');
                        if (sk[0] == st)
                        {
                            sr.Close();
                            return sk[1];
                        }
                    }
                }
                sr.Close();
            }
            return "Попробуйте снова";
        }

       public class Video
        {
            public void fullscreen()
            {
                SendKeys.SendWait("^{ENTER}");
              //  SendKeys.SendWait("{f}");
            }
            public void closevideo()
           
            {
                SendKeys.SendWait("%{F4}"); 
            }
        }





        private void Form1_Shown(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            Form2Instance = form2;
            //ComFile c1 = new ComFile();
            //c1.Form1Instance = this;
            //var numbers = new Choices();
            //c1.BuildGr(numbers);
            //c1.settingListBox();
            //var zap = new Grammatika();
            //zap.startspe();
            ComFile c1 = new ComFile();
            var numbers = new Choices();
            c1.BuildGr(numbers);
            //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
            //SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(muzika);
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;
            gb.Append(numbers);
            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            c1.Form1Instance = this;
            c1.settingListBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var c1 = new ComFile();
            //c1.ClearListBox();
            //var p1 = new Grammatika();
            //p1.startspe();
            string path = @"C:\Users\User\Desktop\КАИ\2 курс\2 семак\Практика\0\команды2.txt";
            this.listBox1.Items.Clear();
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Create(path).Close();
            }
            if (player != null) player.Stop();
            player = null;
            Form2Instance.NewWord("включи музыку", "");
            Form2Instance.NewWord("стоп", "");
            Form2Instance.NewWord("переключи", "");
            Form2Instance.NewWord("закрыть видео", "");
            Form2Instance.NewWord("полный экран", "");
            c1.Form1Instance = this;
            Choices choices = new Choices();
            choices = c1.BuildGr(choices);
            startspe(choices);
            c1.settingListBox();

        }
    }
    }
