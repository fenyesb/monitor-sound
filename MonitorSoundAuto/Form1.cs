using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 40; i++)
            {
                freqs[i] = 1000 * Math.Exp(i / 16.0);
            }
        }
        double[,] table;
        int table_w = 0, table_h = 0;
        double[] freqs = new double[40];
        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            Location = new Point(0, 0);
            Size = Screen.FromControl(this).Bounds.Size;
            Bitmap bmp = Properties.Resources.wave;
            table_w = bmp.Width;
            table_h = bmp.Height;
            table = new double[table_w, table_h];
            for (int x = 0; x < table_w; x++)
            {
                for (int y = 0; y < table_h; y++)
                {
                    table[x, y] = bmp.GetPixel(x, y).R / 255f;
                }
            }
            timer2.Start();
            sw.Restart();
            Cursor.Hide();
        }
        Queue<int> q = new Queue<int>();
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            const double FRAME_TIME = 1.0 / 60.0;
            double VERTICAL_SIZE = Size.Height;
            int y = 0;
            while (y < VERTICAL_SIZE)
            {
                float op = 0;
                float total_weight = 0;
                for (int i = 0; i < table_h; i++)
                {
                    double wave_time = 1.0 / freqs[table_h - 1 - i];
                    double wavelength = wave_time / FRAME_TIME * VERTICAL_SIZE;
                    int frame = (int)(sw.ElapsedMilliseconds / 100f);
                    op += (float)(Math.Sin(y / wavelength * 2 * Math.PI + frame % 10 + 10) * table[frame % table_w, i]);
                    total_weight += (float)table[frame % table_w, i];
                }
                op /= (total_weight == 0 ? 1 : total_weight);
                op *= 10;
                op += 1;
                op /= 2;
                op *= 255;
                op = Math.Min(255, Math.Max(0, op));
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)op, Color.Black)), 0, y, Size.Width, 1);
                y++;
            }
        }
        Stopwatch sw = new Stopwatch();
        private void Form1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
