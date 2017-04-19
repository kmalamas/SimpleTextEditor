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
    public partial class Find : Form
    {

       public string searchString { get; set; }
       public bool isWhole { get; set; }
       public bool isMatch { get; set; }
       public bool isDown { get; set; }
       private Form1 f;

        public Find(Form1 f)
        {
            InitializeComponent();
            this.f = f;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            searchString = textBox1.Text;
            if (checkBox1.Checked == true)
            {
                isMatch = true;
            }
            else isMatch = false;

            if (checkBox2.Checked)
            {
                isWhole = true;
            }
            else isWhole = false;

            if (radioButton1.Checked)
            {
                isDown = false;
            }
            else isDown = true;

            f.findText(searchString,isMatch,isWhole,isDown);
        }
    }
}