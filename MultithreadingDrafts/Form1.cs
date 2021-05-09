using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultithreadingDrafts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Task.Run(() =>
            //{
            //    DumpWords();
            //});
            //Task.Run(() =>
            //{
            //    SetText(textBox2, StartLongRunningTask().ToString());
            //});

            var box1Thread = new Thread(() =>
            {
                DumpWords();
            });

            var box2Thread = new Thread(() =>
            {
                SetText(textBox2, StartLongRunningTask().ToString());
            });

            box1Thread.Start();
            box2Thread.Start();

            lock(box1Thread)
            {

            }
        }

        private DateTime StartLongRunningTask()
        {
            Thread.Sleep(5000);
            return DateTime.Now;
        }

        private void DumpWords()
        {
            while (true)
            {
                SetText(textBox1, GetText(textBox1) + $"\n Still Running..");
                Thread.Sleep(1000);
            }
        }

        private void SetText(TextBox textBox, string text)
        {
            if (IsHandleCreated)
            {
                Action<TextBox, string> setTextCallback = (textBox, text) => textBox.Text = text;
                Invoke(setTextCallback, textBox, text);
            }
        }

        private string GetText(TextBox textBox)
        {
            if (IsHandleCreated)
            {
                Func<TextBox, string> getTextCallback = textBox => textBox.Text;
                return (string)Invoke(getTextCallback, textBox);
            }
            return "";
        }
    }
}
