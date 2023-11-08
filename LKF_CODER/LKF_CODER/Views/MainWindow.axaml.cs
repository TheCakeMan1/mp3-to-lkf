using Avalonia.Controls;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;

namespace LKF_CODER.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public bool CheckBoxs = false;
        static string temp_string_1 = "", temp_string_2 = "", temp_string_3 = "", temp_string_4 = "";
        static int proc = Environment.ProcessorCount;
        static string? subpath;
        static string[]? list_files;
        static int thread_base = 0, thread_base1, thread_base2, thread_base3;

        void Start(object sender, RoutedEventArgs e)
        {
            TextRes.Text = "";
            if (Folder1.Text == "" || Folder2.Text == "" || Authors.Text == "")
            { TextRes.Text = "Не все поля заполнены!"; }
            else
            {
                TextRes.Text = "Начинаю";
                string path = Folder1.Text;
                int text = new DirectoryInfo(path).GetFileSystemInfos("*.mp3").Length;
                string[] files = Directory.Exists(path) ? Directory.GetFiles(path, "*.mp3") : new string[0];
                if (Int32.Parse(Nomers.Text) < 10)
                {
                    subpath = "BOOK_00" + Nomers.Text;
                }
                else if (Int32.Parse(Nomers.Text) > 99 && Int32.Parse(Nomers.Text) < 999)
                {
                    subpath = "BOOK_" + Nomers.Text;
                }
                else
                {
                    subpath = "BOOK_0" + Nomers.Text;
                }
                thread_base = text; 
                temp_string_1 = "#Title=" + Titles.Text + "\r\n#Author=" + Authors.Text + "\r\n#Announcer=" + Authors.Text + "\r\n#File_num=" + thread_base + "\r\n#Total_size_KB=0\r\n#Total_length_SEC=0\r\n#Publish_date=2030\r\n#Publish_place=  \r\n#SubTitle=   \r\n#RecordSource= 1980\r\n";
                list_files = files;
                temp_string_2 = "";
                temp_string_3 = ""; 
                temp_string_4 = "";
                Thread t1 = new Thread(new ThreadStart(Thread1Proc));
                Thread t2 = new Thread(new ThreadStart(Thread2Proc));
                Thread t3 = new Thread(new ThreadStart(Thread3Proc));
                switch (CheckBoxs)
                {
                    case true:
                        switch (proc)
                        {
                            case 3:
                                TextRes.Text = "Начинаю";
                                thread_base1 = thread_base / 2;
                                thread_base2 = thread_base;
                                Directory.CreateDirectory($"{Folder2.Text}/{subpath}");
                                t1.Start();
                                t2.Start();
                                t1.Join();
                                t2.Join();
                                temp_string_1 = temp_string_1 + temp_string_2 + temp_string_3;
                                break;
                            case 4:
                                TextRes.Text = "Начинаю";
                                thread_base1 = thread_base;
                                thread_base1 = thread_base / 3;
                                thread_base2 = thread_base * 2 / 3;
                                thread_base3 = thread_base;
                                Directory.CreateDirectory($"{Folder2.Text}/{subpath}");
                                t1.Start();
                                t2.Start();
                                t3.Start();
                                t1.Join();
                                t2.Join();
                                t3.Join();
                                temp_string_1 = temp_string_1 + temp_string_2 + temp_string_3 + temp_string_4;
                                break;
                            default:
                                TextRes.Text = "Начинаю";
                                thread_base1 = thread_base;
                                Directory.CreateDirectory($"{Folder2.Text}/{subpath}");
                                t1.Start();
                                t1.Join();
                                temp_string_1 = temp_string_1 + temp_string_2;
                                break;
                        }
                        break;
                    case false:
                        TextRes.Text = "Начинаю";
                        thread_base1 = thread_base;
                        Directory.CreateDirectory($"{Folder2.Text}/{subpath}");
                        t1.Start();
                        t1.Join();
                        temp_string_1 = temp_string_1 + temp_string_2;
                        break;
                }
            }
            using (FileStream fstream = new FileStream($"{Folder2.Text}/{subpath}.lgk", FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(temp_string_1);
                fstream.Write(buffer, 0, buffer.Length);
            }
            TextRes.Text = $"Я записал {thread_base} файлов.";
        }

        private void Thread1Proc()
        {
            for (int j = 0; j < thread_base1; j++)
            {
                this.ConvertFileBack(list_files[j]);
                list_files[j] = list_files[j].Replace(".mp3", ".lkf");
                File.Move(list_files[j], $"{Folder2.Text}/{subpath}/{Path.GetFileName(list_files[j])}");
                temp_string_2 = temp_string_2 + subpath + @"\" + Path.GetFileName(list_files[j]) + "\n";
            }
        }
        private void Thread2Proc()
        {
            for (int j = thread_base1; j < thread_base2; j++)
            {
                this.ConvertFileBack(list_files[j]);
                list_files[j] = list_files[j].Replace(".mp3", ".lkf");
                File.Move(list_files[j], $"{Folder2.Text}/{subpath}/{Path.GetFileName(list_files[j])}");
                temp_string_3 = temp_string_3 + subpath + @"\" + Path.GetFileName(list_files[j]) + "\n";
            }
        }
        private void Thread3Proc()
        {
            for (int j = thread_base2; j < thread_base3; j++)
            {
                this.ConvertFileBack(list_files[j]);
                list_files[j] = list_files[j].Replace(".mp3", ".lkf");
                File.Move(list_files[j], $"{Folder2.Text}/{subpath}/{Path.GetFileName(list_files[j])}");
                temp_string_4 = temp_string_4 + subpath + @"\" + Path.GetFileName(list_files[j]) + "\n";
            }
        }


        #region Основной алгоритм шифрования
        private uint[] fcuk = new uint[] { 0x8ac14c27, 0x42845ac1, 0x136506bb, 0x5d47c66 };

        private uint CalcKey(uint local1, uint local2, uint local3, uint local5)
        {
            uint num = (local3 >> 2) & 3;
            uint num2 = (local1 >> 5) ^ (local2 << 2);
            uint num3 = (local2 >> 3) ^ (local1 << 4);
            num2 += num3;
            num3 = local3 ^ local2;
            uint index = (local5 & 3) ^ num;
            uint num5 = this.fcuk[index] ^ local1;
            num5 += num3;
            return (num5 ^ num2);
        }
        private uint CalcLocal3(uint l)
        {
            uint num = 0x9e3779b9;
            uint tmp = num * l;
            return (num * l);
        }

        private void ConvertFileBack(string inputFileName)
        {
            string path = inputFileName.Replace(".mp3", ".lkf");
            try
            {
                BinaryReader reader = new BinaryReader(File.Open(inputFileName, FileMode.Open));
                BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create));
                FileInfo info = new FileInfo(inputFileName);
                uint[] numArray = new uint[0x80];
                for (int i = 0; i < ((info.Length / ((int)numArray.Length)) / 4); i++)
                {
                    for (int k = 0; k < numArray.Length; k++)
                    {
                        numArray[k] = reader.ReadUInt32();
                    }
                    for (uint m = 1; m <= 3; m++)
                    {
                        numArray[0] += this.CalcKey(numArray[numArray.Length - 1], numArray[1], this.CalcLocal3(m), (uint)(0));
                        for (int num5 = 1; num5 < numArray.Length - 1; num5++)
                        {
                            numArray[num5] += this.CalcKey(numArray[num5 - 1], numArray[num5 + 1], this.CalcLocal3(m), (uint)num5);
                        }
                        numArray[numArray.Length - 1] += this.CalcKey(numArray[numArray.Length - 2], numArray[0], this.CalcLocal3(m), (uint)(numArray.Length - 1));
                    }
                    for (int n = 0; n < numArray.Length; n++)
                    {
                        writer.Write(numArray[n]);
                    }
                }
                long num7 = info.Length - (info.Length / ((long)numArray.Length) / 4L * (numArray.Length * 4));
                for (int j = 0; j < num7; j++)
                {
                    writer.Write(reader.ReadByte());
                }
                reader.Close();
                writer.Flush();
                writer.Close();
            }
            catch {
                TextRes.Text = $"Ошибка доступа {inputFileName}";
            }
            
        }
        #endregion

        #region Функции для взаимодействия с элементами
        public async void DialodFolder_in(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync(this);
            Folder1.Text = result;
        }

        public async void DialodFolder_out(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync(this);
            Folder2.Text = result;
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            CheckBoxs = true;
        }
        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBoxs = false;
        }
        #endregion
    }
}