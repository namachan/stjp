using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Drawing.Text;
using System.Windows.Media;
using System.Globalization;
using System.IO;

namespace ttfsr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox1.Items.AddRange(Fonts.SystemTypefaces.Select(x => x.FontFamily.Source).Distinct().OrderBy(x => x).ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            var cb1 = (string)comboBox1.SelectedItem;
            var ffs = Fonts.SystemTypefaces.Where(x => x.FontFamily.Source == cb1).Select(x => x.Style.ToString()).Distinct();
            comboBox2.Items.AddRange(ffs.OrderBy(x => x).ToArray());
            tb4();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            var cb1 = (string)comboBox1.SelectedItem;
            var cb2 = (string)comboBox2.SelectedItem;
            var ffss = Fonts.SystemTypefaces.Where(x => x.FontFamily.Source == cb1 && x.Style.ToString() == cb2)
                .Select(x => x.Weight.ToString()).Distinct();
            comboBox3.Items.AddRange(ffss.OrderBy(x => x).ToArray());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();

            var cb1 = (string)comboBox1.SelectedItem;
            var cb2 = (string)comboBox2.SelectedItem;
            var cb3 = (string)comboBox3.SelectedItem;
            var ffssw = Fonts.SystemTypefaces.Where(x => x.FontFamily.Source == cb1 && x.Style.ToString() == cb2 && x.Weight.ToString() == cb3)
                .Select(x => x.Stretch.ToString()).Distinct();
            comboBox4.Items.AddRange(ffssw.OrderBy(x => x).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            var cb1 = (string)comboBox1.SelectedItem;
            var cb2 = (string)comboBox2.SelectedItem;
            var cb3 = (string)comboBox3.SelectedItem;
            var cb4 = (string)comboBox4.SelectedItem;
            var ffssws = Fonts.SystemTypefaces
                .Where(x => x.FontFamily.Source == cb1)
                .Where(x => string.IsNullOrEmpty(cb2) || x.Style.ToString() == cb2)
                .Where(x => string.IsNullOrEmpty(cb3) || x.Weight.ToString() == cb3)
                .Where(x => string.IsNullOrEmpty(cb4) || x.Stretch.ToString() == cb4);

            // test WTF
            if (ffssws.Count() != 1)
            {
                textBox1.AppendText("WTF");
                return;
            }

            Typeface tf = ffssws.Single();
            GlyphTypeface gtf;
            // test GTF
            if (false == tf.TryGetGlyphTypeface(out gtf))
            {
                textBox1.AppendText("GTF");
                return;
            }

            var iu = gtf.CharacterToGlyphMap.Keys.OrderBy(x => x).ToArray();
            textBox1.AppendText(sphs(iu));
            textBox2.Text = iu.Length.ToString();
        }

        private string sphs<T>(IList<T> il)
        {
            var hs = new HashSet<string>();
            dynamic c = il[0], n = il[0];
            Action phs = () =>
            {
                hs.Add(string.Format("{0:X4}-{1:X4}", c, n));
            };

            for (int i = 1; i < il.Count; n = il[i++])
            {
                if (il[i] != n + 1)
                {
                    phs();
                    c = il[i];
                }
            }

            phs();
            return string.Join(",", hs);
        }

        private IEnumerable<uint> xix(IEnumerable<string> ss)
        {
            Func<string, uint> xul = (x) =>
            {
                return uint.Parse(x, NumberStyles.AllowHexSpecifier);
            };

            foreach (var s in ss)
            {
                var x = s.Replace("..", ".").Split('.');

                if (x.Length == 1)
                    yield return xul(x[0]);
                else
                {
                    uint hx = xul(x[1]);
                    for (uint lx = xul(x[0]); lx <= hx; lx++)
                        yield return lx;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;

            var s3 = textBox3.Text.Replace("\r", string.Empty).Split('\n')
                .Select(x => x.Trim()).Where(x => x.Length != 0 && !x.StartsWith("#") && x.IndexOf('<') == -1)
                .Select(x => x.Substring(0, x.IndexOf(';')).TrimEnd());

            var s3u = xix(s3).Distinct().OrderBy(x => x).ToArray();
            textBox1.AppendText(sphs(s3u));
            textBox2.Text = s3u.Length.ToString();
        }

        private void tb4()
        {
            var ff = (string)comboBox1.SelectedItem;
            if (string.IsNullOrEmpty(ff))
                ff = "MS UI Gothic";

            var t = textBox4.Text;
            var c1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var c2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            using (var g1 = Graphics.FromImage(c1))
            using (var g2 = Graphics.FromImage(c2))
            using (var f = new Font(ff, 9))
            {
                richTextBox1.Font = f;
                richTextBox1.Text = t;

                g1.DrawString(t, f, System.Drawing.Brushes.Black, 0, 0);
                pictureBox1.Image = c1;

                TextRenderer.DrawText(g2, t, f, new Point(), System.Drawing.Color.Black);
                pictureBox2.Image = c2;
            }

            textBox5.Text = string.Join(",", t.Select(x => ((ulong)x).ToString("X")));
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            tb4();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var s = textBox6.Text;
            var u = new UTF32Encoding(false, false, true);
            File.WriteAllText(textBox7.Text, s, u);

            textBox1.Text = textBox6.Text;
        }
    }
}
