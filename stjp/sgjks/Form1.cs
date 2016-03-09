using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace sgjks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = @"C:\SteamLibrary\steamapps\common\ARK\ShooterGame\Content\Localization\Game\en\ShooterGame.archive";
            textBox2.Text = @"T:\T3\git\stjp\stjp\ShooterGame.json";
        }

        private string openpenpen(string p)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.FileName = "ShooterGame.archive";
                ofd.InitialDirectory = p;

                if (ofd.ShowDialog() == DialogResult.OK)
                    return ofd.FileName;
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string p;
            if (null != (p = openpenpen(@"C:\SteamLibrary\steamapps\common\ARK\ShooterGame\Content\Localization\Game\en\")))
                textBox1.Text = p;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string p;
            if (null != (p = openpenpen(@"T:\T3\git\stjp\stjp\")))
                textBox2.Text = p;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();

            textBox3.Text = string.Empty;
            var j1 = JsonConvert.DeserializeObject<JContainer>(File.ReadAllText(textBox1.Text));
            var j2 = JsonConvert.DeserializeObject<JContainer>(File.ReadAllText(textBox2.Text));

            var n1 = string.Join(",", j1["Subnamespaces"].Select(x => x["Namespace"]).OrderBy(x => x));
            var n2 = string.Join(",", j2["Subnamespaces"].Select(x => x["Namespace"]).OrderBy(x => x));
            if (n1 != n2)
            {
                textBox3.AppendText("Namespace difference");
                textBox3.AppendText(Environment.NewLine);
                textBox3.AppendText(string.Format("in:{0}", n1));
                textBox3.AppendText(Environment.NewLine);
                textBox3.AppendText(string.Format("mf:{0}", n2));
                textBox3.AppendText(Environment.NewLine);
                return;
            }

            foreach (var n in j1.SelectTokens("Subnamespaces").Children())
            {
                textBox3.AppendText(string.Format("Namespace[{0}]", n["Namespace"]));
                textBox3.AppendText(Environment.NewLine);

                var hs1 = new HashSet<string>(n["Children"].Select(x => (string)x["Source"]["Text"]));
                var hs2 = j2["Subnamespaces"].Single(x => x["Namespace"] == n["Namespace"])["Children"].Select(x => (string)x["Source"]["Text"]);

            }

            //var r = v1.Values("Children").Select(x => x.Children()).ToArray();

            //foreach (var y in v1.Values("Children").Select(x => x["Source"]))
            //{


            //}

        }
    }
}
