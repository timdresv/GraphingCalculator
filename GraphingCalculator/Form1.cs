using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GraphingCalculator
{
    public partial class Form1 : Form
    {
        Graphics g;
        static int w, h, c, sc, ind, ind1, ind2, q = 20;
        double x, val, f = 0.01;
        String s, s1;
        float[] pointsX;
        float[] pointsY;
        private double eval(String s)
        {
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            if (s[0] == '-')
                s = "0" + s;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    sc = 1;
                    for (int j = i + 1; j < s.Length; j++)
                    {
                        if (s[j] == '(')
                        {
                            sc += 1;
                        }
                        else if (s[j] == ')')
                        {
                            sc -= 1;
                        }
                        if (sc == 0)
                        {
                            s = s.Substring(0, i) + "(" + eval(s.Substring(i + 1, j - i - 1)).ToString() + ")" + s.Substring(j + 1);
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]) && (s[i - (i == 0 ? 0 : 1)] != '(') && (s[i - (i == 0 ? 0 : 1)] != '-'))
                {
                    for (int j = i; j < s.Length; j++)
                    {
                        if (!char.IsDigit(s[j]) && s[j] != '.')
                        {
                            s = s.Substring(0, i) + "(" + s.Substring(i, j - i) + ")" + s.Substring(j);
                            i = j + 2;
                            break;
                        }
                        else if (j == s.Length - 1)
                        {
                            s = s.Substring(0, i) + "(" + s.Substring(i, j - i + 1) + ")";
                            i = j + 2;
                            break;
                        }
                    }
                }
                else if (char.IsDigit(s[i]) && (s[i - (i == 0 ? 0 : 1)] == '('))
                {
                    for (int j = i; j < s.Length; j++)
                    {
                        if (!char.IsDigit(s[j]) && s[j] != '.')
                        {
                            i = j;
                            break;
                        }
                    }
                }
                else if (char.IsDigit(s[i]) && (s[i - (i == 0 ? 0 : 1)] == '-'))
                {
                    if (i - 1 == 0)
                    {
                        for (int j = i; j < s.Length; j++)
                        {
                            if (!char.IsDigit(s[j]) && s[j] != '.')
                            {
                                s = "(" + s.Substring(i - 1, j - i + 1) + ")" + s.Substring(j);
                                i = j + 2;
                                break;
                            }
                            else if (j == s.Length - 1)
                            {
                                s = "(" + s.Substring(i - 1, j - i + 2) + ")";
                                i = j + 2;
                                break;
                            }
                        }
                    }
                    else if (char.IsDigit(s[i - 2]) || s[i - 2] == ')')
                    {
                        for (int j = i; j < s.Length; j++)
                        {
                            if (!char.IsDigit(s[j]) && s[j] != '.')
                            {
                                s = s.Substring(0, i) + "(" + s.Substring(i, j - i) + ")" + s.Substring(j);
                                i = j + 2;
                                break;
                            }
                            else if (j == s.Length - 1)
                            {
                                s = s.Substring(0, i) + "(" + s.Substring(i, j - i + 1) + ")";
                                i = j + 2;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = i; j < s.Length; j++)
                        {
                            if (!char.IsDigit(s[j]) && s[j] != '.')
                            {
                                i = j;
                                break;
                            }
                        }
                    }
                }
            }
            ind = s.IndexOf("log");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                ind2 = s.IndexOf(')', ind1 + 1);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Log(Convert.ToDouble(s.Substring(ind1 + 2, ind2 - ind1 - 2)), Convert.ToDouble(s.Substring(ind + 4, ind1 - ind - 4))), 4).ToString() + ")" + s.Substring(ind2 + 1);
                ind = s.IndexOf("log");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("ln");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Log(Convert.ToDouble(s.Substring(ind + 3, ind1 - ind - 3))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("ln");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("exp");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Exp(Convert.ToDouble(s.Substring(ind + 4, ind1 - ind - 4))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("exp");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("sqrt");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Sqrt(Convert.ToDouble(s.Substring(ind + 5, ind1 - ind - 5))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("sqrt");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("arcsin");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Asin(Convert.ToDouble(s.Substring(ind + 7, ind1 - ind - 7))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("arcsin");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("arccos");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Acos(Convert.ToDouble(s.Substring(ind + 7, ind1 - ind - 7))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("arccos");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("arctan");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Atan(Convert.ToDouble(s.Substring(ind + 7, ind1 - ind - 7))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("arctan");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("sin");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Sin(Convert.ToDouble(s.Substring(ind + 4, ind1 - ind - 4))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("sin");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("cos");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Cos(Convert.ToDouble(s.Substring(ind + 4, ind1 - ind - 4))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("cos");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("tan");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Tan(Convert.ToDouble(s.Substring(ind + 4, ind1 - ind - 4))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("tan");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("abs");
            while (ind != -1)
            {
                ind1 = s.IndexOf(')', ind);
                s = s.Substring(0, ind) + "(" + Math.Round(Math.Abs(Convert.ToDouble(s.Substring(ind + 4, ind1 - ind - 4))), 4).ToString() + ")" + s.Substring(ind1 + 1);
                ind = s.IndexOf("abs");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOf("^");
            while (ind != -1)
            {
                ind1 = s.LastIndexOf('(', ind);
                ind2 = s.IndexOf(')', ind);
                s = s.Substring(0, ind1) + "(" + Math.Round(Math.Pow(Convert.ToDouble(s.Substring(ind1 + 1, ind - ind1 - 2)), Convert.ToDouble(s.Substring(ind + 2, ind2 - ind - 2))), 4).ToString() + ")" + s.Substring(ind2 + 1);
                ind = s.IndexOf("^");
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOfAny(new char[] { '*', '/' });
            while (ind != -1)
            {
                ind1 = s.LastIndexOf('(', ind);
                ind2 = s.IndexOf(')', ind);
                if (s[ind] == '*')
                    s = s.Substring(0, ind1) + "(" + Math.Round(Convert.ToDouble(s.Substring(ind1 + 1, ind - ind1 - 2)) * Convert.ToDouble(s.Substring(ind + 2, ind2 - ind - 2)), 4).ToString() + ")" + s.Substring(ind2 + 1);
                else
                    s = s.Substring(0, ind1) + "(" + Math.Round(Convert.ToDouble(s.Substring(ind1 + 1, ind - ind1 - 2)) / Convert.ToDouble(s.Substring(ind + 2, ind2 - ind - 2)), 4).ToString() + ")" + s.Substring(ind2 + 1);
                ind = s.IndexOfAny(new char[] { '*', '/' });
            }
            if (s.IndexOf('E') != -1)
                return Double.NaN;
            ind = s.IndexOfAny(new char[] { '+', '-' });
            while (ind != -1)
            {
                ind1 = s.LastIndexOf('(', ind);
                ind2 = s.IndexOf(')', ind);
                if (s[ind] == '+')
                {
                    if (s[ind - 1] != 'E')
                    {
                        s = s.Substring(0, ind1) + "(" + Math.Round(Convert.ToDouble(s.Substring(ind1 + 1, ind - ind1 - 2)) + Convert.ToDouble(s.Substring(ind + 2, ind2 - ind - 2)), 4).ToString() + ")" + s.Substring(ind2 + 1);
                        ind = s.IndexOfAny(new char[] { '+', '-' });
                    }
                    else
                    {
                        ind = s.IndexOfAny(new char[] { '+', '-' }, ind + 1);
                    }
                }
                else
                {
                    if (s[ind - 1] != '(')
                    {
                        s = s.Substring(0, ind1) + "(" + Math.Round(Convert.ToDouble(s.Substring(ind1 + 1, ind - ind1 - 2)) - Convert.ToDouble(s.Substring(ind + 2, ind2 - ind - 2)), 4).ToString() + ")" + s.Substring(ind2 + 1);
                        ind = s.IndexOfAny(new char[] { '+', '-' });
                    }
                    else
                    {
                        ind = s.IndexOfAny(new char[] { '+', '-' }, ind + 1);
                    }
                }
            }
            return Convert.ToDouble(s.Substring(1, s.Length - 2));
        }
        private void PaintFunc()
        {
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
            g.ScaleTransform(1, -1);
            s = textBox1.Text;
            x = -w;
            c = 0;
            while (x <= w)
            {
                x = Math.Round(x, 3);
                s1 = s;
                for (int i = 0; i < s1.Length; i++)
                {
                    if ((s1[i] == 'x') && (s1[i - (i == 0 ? 0 : 1)] != 'e'))
                    {
                        s1 = s1.Substring(0, i) + "(" + x.ToString() + ")" + s1.Substring(i + 1);
                    }
                }
                try
                {
                    val = eval(s1);
                }
                catch
                {
                    textBox1.Text = "Error";
                    break;
                }
                pointsX[c] = q * Convert.ToSingle(x);
                pointsY[c] = val > 5 * h ? float.NaN : (val < -5 * h ? float.NaN : q * Convert.ToSingle(val));
                c += 1;
                x += f;
            }
            c = 0;
            if (textBox1.Text != "Error")
            {
                for (int i = 0; i < pointsX.Length; i++)
                {
                    if (float.IsNaN(pointsY[i]))
                    {
                        if (i - c > 1)
                        {
                            var p = new PointF[i - c];
                            for (int j = c; j < i; j++)
                            {
                                p[j - c] = new PointF(pointsX[j], pointsY[j]);
                            }
                            g.DrawCurve(new Pen(Color.Red, 3f), p, 0f);
                        }
                        else if (i - c > 0)
                        {
                            g.FillEllipse(new SolidBrush(Color.Red), pointsX[c] - 2, pointsY[c] - 2, 4f, 4f);
                        }
                        c = i + 1;
                    }
                }
                if (c != pointsX.Length && pointsX.Length - c > 1)
                {
                    var p = new PointF[pointsX.Length - c];
                    for (int j = c; j < pointsX.Length; j++)
                    {
                        p[j - c] = new PointF(pointsX[j], pointsY[j]);
                    }
                    g.DrawCurve(new Pen(Color.Red, 3f), p, 0f);
                }
                else if (c != pointsX.Length && pointsX.Length - c > 0)
                {
                    g.FillEllipse(new SolidBrush(Color.Red), pointsX[c] - 2, pointsY[c] - 2, 4f, 4f);
                }
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
            g.ScaleTransform(1, -1);
            for (int i = -h; i <= h; i++)
            {
                g.DrawLine(new Pen(Color.DarkGray), -w * q, i * q, w * q, i * q);
            }
            for (int i = -w; i <= w; i++)
            {
                g.DrawLine(new Pen(Color.DarkGray), i * q, -h * q, i * q, h * q);
            }
            g.ScaleTransform(1, -1);
            for (int i = 2; i < w; i += 2)
            {
                g.DrawString(i.ToString(), new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Black), (i - 0.3f) * q, 0);
                g.DrawString("-" + i.ToString(), new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Black), (-i - 0.5f) * q, 0);
            }
            for (int i = 2; i < h - 1; i += 2)
            {
                g.DrawString("-" + i.ToString(), new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Black), 0, (i - 0.3f) * q);
                g.DrawString(i.ToString(), new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Black), 0, (-i - 0.5f) * q);
            }
            g.DrawString("0", new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Black), 0, 0);
            g.ScaleTransform(1, -1);
            g.DrawLine(new Pen(Color.Black), -w * q, 0 * q, w * q, 0 * q);
            g.DrawLine(new Pen(Color.Black), 0 * q, -h * q, 0 * q, h * q);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();
            dialog.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PaintFunc();
            }
            catch
            {
                textBox1.Text = "Error";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            q = trackBar1.Value * 5;
            w = Convert.ToInt32(pictureBox1.Width / 2 / q + 1.0);
            h = Convert.ToInt32(pictureBox1.Height / 2 / q + 1.0);
            pointsX = new float[100 * 2 * w + 1];
            pointsY = new float[100 * 2 * w + 1];
            pictureBox1.Invalidate();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PaintFunc();
            }
            else if (e.KeyCode == Keys.F1)
            {
                Form2 dialog = new Form2();
                dialog.Show();
            }
            else if (e.KeyCode == Keys.F2)
            {
                pictureBox1.Invalidate();
            }
        }
        public Form1()
        {
            InitializeComponent();
            w = Convert.ToInt32(pictureBox1.Width / 2 / q + 1.0);
            h = Convert.ToInt32(pictureBox1.Height / 2 / q + 1.0);
            pointsX = new float[100 * 2 * w + 1];
            pointsY = new float[100 * 2 * w + 1];
            pictureBox1.Invalidate();
            CultureInfo culture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}
