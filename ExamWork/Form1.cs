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
using System.Xml.Linq;

namespace ExamWork
{
    public partial class Form1 : Form
    {
        int ProgressBarCount = 0;
        Random rand = new Random();
        List<ProgressBar> ProgressBarList = new List<ProgressBar>();
        List<Label> LabelList = new List<Label>();
        List<Thread> ThreadList = new List<Thread>();
        Semaphore semaphore = null;

        public Form1()
        {
            InitializeComponent();
            semaphore = new Semaphore(3, 3);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (ProgressBarCount < 10)
            {
                ProgressBar p = new ProgressBar();
                p.Location = new Point(MousePosition.X, MousePosition.Y);
                p.Name = "p" + ProgressBarCount + rand.Next(100, 200);
                p.Step = 1;

                Label l = new Label();
                l.Text = rand.Next(1, Convert.ToInt32(numericUpDown1.Value)).ToString();
                l.Location = new Point(p.Location.X + 102, p.Location.Y);
                l.Name = "l" + ProgressBarCount + rand.Next(100, 200);
                p.Maximum = Convert.ToInt32(l.Text);
                p.Minimum = 0;

                this.Controls.Add(p);
                this.Controls.Add(l);
                ProgressBarList.Add(p);
                LabelList.Add(l);
                ThreadList.Add(new Thread(new ParameterizedThreadStart(AsyncWork)));
                ProgressBarCount++;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                timer1.Enabled = false;
            else
                timer1.Enabled = true;
        }

        private void AsyncWork(object index)
        {
            int _index = (int)index;
            ProgressBar tmpProgressBar = ProgressBarList[_index];
            Label tmpLabel = LabelList[_index];
            Thread tmpThread = ThreadList[_index];
            semaphore.WaitOne();
            for (int i = 0; i < ProgressBarList.Find(x => x.Name.Contains(tmpProgressBar.Name)).Maximum; i++)
            {
                Thread.Sleep(1000);
                this.Invoke((MethodInvoker)delegate () 
                { 
                    ProgressBarList.Find(x => x.Name.Contains(tmpProgressBar.Name)).PerformStep(); 
                });
            }
            this.Invoke((MethodInvoker)delegate () 
            { 
                this.Controls.Remove(ProgressBarList.Find(x => x.Name.Contains(tmpProgressBar.Name))); 
            });
            this.Invoke((MethodInvoker)delegate () 
            { 
                this.Controls.Remove(LabelList.Find(x => x.Name.Contains(tmpLabel.Name))); 
            });

            ProgressBarList.Remove(tmpProgressBar);
            LabelList.Remove(tmpLabel);
            ThreadList.Remove(tmpThread);
            ProgressBarCount--;
            semaphore.Release();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ThreadList.Count == 0)
                return;

            for (int i = 0; i < ThreadList.Count; i++)
            {
                if (ThreadList[i].IsAlive != true)
                    ThreadList[i].Start(i);
            }    
        }
    }
}
