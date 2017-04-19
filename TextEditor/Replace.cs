using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Replace : Form
    {
        Form1 f;
        bool isMatch;
        bool isWhole;

        public Replace(Form1 f)
        {
            InitializeComponent();
            this.f = f;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                isMatch = true;
            }
            else isMatch = false;

            if (checkBox2.Checked==true)
            {
                isWhole = true;
            }
            else isWhole = false;

            f.findText(textBox1.Text, isMatch, isWhole, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Replace(textBox2.Text,textBox1.Text,checkBox1.Checked,checkBox2.Checked);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                isMatch = true;
            }
            else isMatch = false;

            if (checkBox2.Checked == true)
            {
                isWhole = true;
            }
            else isWhole = false;

            f.ReplaceAll(textBox1.Text, textBox2.Text, isMatch, isWhole);
        }
    }
}
