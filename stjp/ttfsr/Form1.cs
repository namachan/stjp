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
            var hs = new HashSet<string>();
            int c = iu[0], n = iu[0];
            Action phs = () =>
            {
                hs.Add(string.Format("{0:X4}-{1:X4}", c, n));
            };

            for (int i = 1; i < iu.Length; n = iu[i++])
            {
                if (iu[i] != n + 1)
                {
                    phs();
                    c = iu[i];
                }
            }

            phs();
            textBox1.AppendText(string.Join(",", hs));
        }
    }
}
