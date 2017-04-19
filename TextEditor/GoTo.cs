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
    public partial class GoTo : Form
    {
        RichTextBox tb;
        public GoTo(RichTextBox tb)
        {
            InitializeComponent();
            this.tb = tb;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int line = int.Parse(this.textBox1.Text);
            GotoLine(line);
            this.Close();
        }
        private void GotoLine(int i)
        {
            int index = tb.GetFirstCharIndexFromLine(i - 1);
            tb.Select(index, 0);
        }

 
    }
}
