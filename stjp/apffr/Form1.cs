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

namespace apffr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = openDir();
            button2.Enabled = !string.IsNullOrEmpty(textBox1.Text);
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

        public class ArktribeData
        {
            public string FileID;
            public string TribeName;
            public string[] MembersPlayerName;
            public string[] TribeLog;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            var df = Path.Combine(textBox1.Text, string.Concat("dst_", DateTime.Now.ToString("yyyyMMddHHmmss"), ".txt"));
            textBox2.AppendText(df);
            textBox2.AppendText(Environment.NewLine);
            textBox2.AppendText(Environment.NewLine);

            foreach (var f in Directory.EnumerateFiles(textBox1.Text, "*.arktribe"))
            {
                textBox2.AppendText(f);
                textBox2.AppendText(Environment.NewLine);

                var ad = ReadArktribeData(f);
                textBox2.AppendText(string.Concat("Name:", ad.TribeName));
                textBox2.AppendText(Environment.NewLine);

                textBox2.AppendText(string.Concat("Members:", string.Join(",", ad.MembersPlayerName)));
                textBox2.AppendText(Environment.NewLine);

                textBox2.AppendText(string.Concat("Log:", Environment.NewLine, string.Join(Environment.NewLine, ad.TribeLog)));
                textBox2.AppendText(Environment.NewLine);
                textBox2.AppendText(Environment.NewLine);
            }

            File.WriteAllText(df, textBox2.Text);
        }

        private ArktribeData ReadArktribeData(string path)
        {
            var ad = new ArktribeData();
            var enca = Encoding.ASCII;
            var encu = Encoding.Unicode;
            var data = File.ReadAllBytes(path);
            int gi = 0;
            int gdl = data.Length;

            Func<byte[], int, int> sbi = (bb, bi) =>
            {
                int ddi = 0;
                for (; ddi < bb.Length && bb[ddi] == data[bi + ddi]; ddi++) ;
                return (ddi == bb.Length) ? bi : -1;
            };

            Func<int, bool, byte[]> rbs = (bi, a) =>
            {
                if (a)
                    return data.Skip(bi).TakeWhile(x => x != 0x00).ToArray();

                var baus = new List<byte>();
                for (int z = 0; bi < data.Length; bi++)
                {
                    if (z == 2 && data[bi] == 0x00)
                    {
                        baus.RemoveAt(baus.Count - 1);
                        break;
                    }

                    z = (data[bi] == 0x00) ? z + 1 : 0;
                    baus.Add(data[bi]);
                }
                return baus.ToArray();
            };

            Func<int, string> rnaus = (bi) =>
            {
                var a = (data[bi] == 0x00);
                var enc = a ? enca : encu;
                var li = a ? 0x01 : 0x01; //0x0D;
                var b = rbs(bi + li, a);
                gi += b.Length;
                gi += a ? 2 : 3;
                return enc.GetString(b);
            };

            Func<string, int, string> rsp = (s, fd) =>
            {
                var sb = enca.GetBytes(s);
                while (gi < gdl)
                {
                    if (-1 != sbi(sb, gi))
                    {
                        gi += fd;
                        return rnaus(gi);
                    }
                    gi++;
                }
                return null;
            };

            Func<string, int, string[]> rasp = (s, fd) =>
            {
                var sb = enca.GetBytes(s);
                while (gi < gdl)
                {
                    if (-1 != sbi(sb, gi))
                    {
                        gi += fd;
                        int c = data[gi];

                        gi += 4;
                        var sa = new List<string>();
                        for(int ai = 0; ai < c; ai++)
                        {
                            gi += 3;
                            sa.Add(rnaus(gi));
                        }
                        return sa.ToArray();
                    }
                    gi++;
                }
                return new string[0];
            };

            ad.FileID = Path.GetFileNameWithoutExtension(path);
            ad.TribeName = rsp("TribeName", 0x25);
            ad.MembersPlayerName = rasp("MembersPlayerName", 0x3C);
            ad.TribeLog = rasp("TribeLog", 0x33);
            return ad;
        }
    }
}
