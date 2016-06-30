using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace svlnp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = @"C:\SteamLibrary\steamapps\common\ARK\ShooterGame\Content\Mods";
            textBox2.Text = @"C:\SteamLibrary\ShooterGameServer\Sv01\ShooterGame\Content\Mods";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s;
            if (null != (s = openDir()))
                textBox1.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s;
            if (null != (s = openDir()))
                textBox2.Text = s;
        }

        private string openDir()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    return fbd.SelectedPath;
            }
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var modIds = Directory.EnumerateFiles(textBox1.Text, "*.mod")
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .Distinct()
                .OrderBy(x => x)
                .ToArray()
            ;

            var sb = new StringBuilder();
            var ci = textBox1.Text;
            var si = textBox2.Text;
            var fi = textBox4.Text.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
            foreach (var f in modIds)
            {
                if (fi.Any() && !fi.Contains(f))
                    continue;
                var s = string.Concat(f, ".mod");
                sb.AppendFormat("mklink \"{0}\" \"{1}\"", Path.Combine(si, s), Path.Combine(ci, s));
                sb.AppendLine();
                sb.AppendFormat("mklink /D \"{0}\" \"{1}\"", Path.Combine(si, f), Path.Combine(ci, f));
                sb.AppendLine();
            }
            textBox3.Text = sb.ToString();

            if (fi.Any())
            {
                foreach (var f in fi.Except(modIds))
                    textBox3.Text += "mod " + f + " is missing." + Environment.NewLine;
            }
        }
    }
}
