using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.IO;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        private bool isEdited;
        private bool exists;
        private int count = 1;
        private string docTitle;
        Find find;
        int counter = 0;
        



        public Form1()
        {
            InitializeComponent();
            isEdited = false;
            exists = false;
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.statusStrip1.Visible = false;
            updateTitle("New Document " + count.ToString());
            updateStatus();
            
            
        }

        #region Methods

        private void updateTitle(string doc)
        {
            this.Text = "Editor - " + doc;
        }

        public int CurrentColumn
        {
            get { return richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexOfCurrentLine() + 1; }
        }

        public int CurrentLine
        {
            get { return richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart)+1; }
        }

        private void updateStatus()
        {
            this.status.Text = "Line: "+CurrentLine.ToString()+", Column: "+CurrentColumn.ToString()+"";
        }

        private DialogResult saveChanges()
        {
            if (isEdited == true)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Changes where made to the document", MessageBoxButtons.YesNoCancel);
                return result;
            }
            else return DialogResult.No;

        }

        private void newDocument()
        {
            richTextBox1.Clear();
            count++;
            updateTitle("New Document " + count.ToString());
            exists = false;
            isEdited = false;
        }
         
        private void openFile()
                   
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)                      
            {
                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);    
                isEdited = false;
                updateTitle(openFileDialog.FileName); 
                docTitle = openFileDialog.FileName;
                exists = true;
                
            }         
        }

        internal void findText(string search, bool isMatch, bool isWhole, bool isDown)
        {
            RichTextBoxFinds options = RichTextBoxFinds.None;
            int from;
            if (counter == 0)
            {
                from = richTextBox1.SelectionStart;
            }
            else from = richTextBox1.SelectionStart + 1;

            int to = richTextBox1.TextLength - 1;
            if (isMatch == true)
            {
                options = options | RichTextBoxFinds.MatchCase;
            }
            if (isWhole == true)
            {
                options = options | RichTextBoxFinds.WholeWord;
            }
            if (isDown == false)
            {
                options = options | RichTextBoxFinds.Reverse;
                to = from;
                from = 0;
                counter = 0;
            }


            int start = 0;
            start = richTextBox1.Find(search, from, to, options);

            if (start > 0)
            {
                richTextBox1.SelectionStart = start;
                richTextBox1.SelectionLength = search.Length;
                richTextBox1.ScrollToCaret();
                richTextBox1.Refresh();
                richTextBox1.Focus();
                counter++;
            }
            else
            {
                MessageBox.Show("No match found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        internal void Replace(string newString, string search, bool isMatch, bool isWhole)
        {
            if (!string.IsNullOrEmpty(richTextBox1.SelectedText))
                this.richTextBox1.SelectedText = newString;
            else findText(search, isMatch, isWhole, true);
        }

        internal void ReplaceAll(string searchString, string newString, bool isMatch, bool isWhole)
        {
            Console.WriteLine(isMatch.ToString() + isWhole.ToString() + searchString + newString);

            string searchText;
            if (isWhole == true)
            {
                searchText = string.Format(@"\b{0}\b", searchString);
                Console.WriteLine(searchText);
            }
            else searchText = searchString;
            if (isMatch == false)
            {
                richTextBox1.Text = Regex.Replace(this.richTextBox1.Text, searchText, newString, RegexOptions.IgnoreCase);
            }
            else richTextBox1.Text = Regex.Replace(this.richTextBox1.Text, searchText, newString);
        }
        #endregion


        #region RichTextBox Events

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isEdited = true;
            updateStatus();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            updateStatus();
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            updateStatus();
        }
        #endregion
        

        #region File

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isEdited == false)
            {
                newDocument();
            }
            else
            {
                DialogResult res = saveChanges();
                if (res == DialogResult.Yes)
                {
                    if (exists == true)
                    {
                        this.richTextBox1.SaveFile(docTitle, RichTextBoxStreamType.PlainText);
                        newDocument();

                    }
                    else
                    {
                        DialogResult result = saveFileDialog.ShowDialog();
                        if (result == DialogResult.Cancel)
                            return;
                        else if (result == DialogResult.OK)
                        {
                            try
                            {
                                richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("error while saving file " + ex.Message);
                            }
                            newDocument();
                        }
                    }
                }
                else if (res == DialogResult.No)
                {
                    newDocument();
                }
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isEdited == false)
            {
                openFile();
            }
            else
            {
                DialogResult res = saveChanges();
                if (res == DialogResult.Yes)
                {
                    if (exists == true)
                    {
                        this.richTextBox1.SaveFile(docTitle, RichTextBoxStreamType.PlainText);
                        openFile();

                    }
                    else
                    {
                        DialogResult result = saveFileDialog.ShowDialog();
                        if (result == DialogResult.Cancel)
                            return;
                        else if (result == DialogResult.OK)
                        {
                            try
                            {
                                richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("error while saving file " + ex.Message);
                            }
                            openFile();
                        }
                    }
                }
                else if (res == DialogResult.No)
                {
                    openFile();
                }
            }

        }
        
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (exists == true)
            {
                this.richTextBox1.SaveFile(docTitle, RichTextBoxStreamType.PlainText);
            }
            else
            {
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.OK)
                {
                    try
                    {
                        richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                        updateTitle(saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error while saving file " + ex.Message);
                    }
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
             DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.OK)
                {
                    try
                    {
                        richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                        updateTitle(saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error while saving file " + ex.Message);
                    }
                }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.DocumentName = this.Text;
                printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
                printDoc.Print();
                
            }
        }
       


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion


        #region Edit

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectedText = "";
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            this.richTextBox1.AppendText(date.ToShortTimeString() + " ");
            this.richTextBox1.AppendText(date.ToShortDateString());
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            find = new Find(this);
            find.Show();
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (find != null && !find.IsDisposed)
            {
                this.findText(find.searchString,find.isMatch,find.isWhole, find.isDown);
            }
            else
            {
                find = new Find(this);
                find.Show();
            }
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoTo go = new GoTo(richTextBox1);
            go.ShowDialog();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Replace rep = new Replace(this);
            rep.Show();
        }


        #endregion


        #region Format


        private void wordWrapToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.wordWrapToolStripMenuItem.Checked == true)
            {
                this.statusBarToolStripMenuItem.Enabled = false;
                this.statusStrip1.Visible = false;
                this.richTextBox1.WordWrap = true;
            }
            else
            {
                this.statusBarToolStripMenuItem.Enabled = true;
                this.statusStrip1.Visible = true;
                this.richTextBox1.WordWrap = false;
            }


        }


        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = fontDialog.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                Font font = fontDialog.Font;
                this.richTextBox1.Font = font;
            }
        }


        #endregion

           
        #region View

        private void statusBarToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.statusBarToolStripMenuItem.Checked == true)
            {
                this.statusStrip1.Visible = true;
            }
            else
                this.statusStrip1.Visible = false;
        }

        #endregion




       

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {

            StringReader reader = new StringReader(richTextBox1.Text);
            float LinesPerPage = 0;
            float YPosition = 0;
            int Count = 0;
            float LeftMargin = e.MarginBounds.Left;
            float TopMargin = e.MarginBounds.Top;
            string Line = null;
            Font PrintFont = this.richTextBox1.Font;
            SolidBrush PrintBrush = new SolidBrush(Color.Black);

            LinesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);

            while (Count < LinesPerPage && ((Line = reader.ReadLine()) != null))
            {
                YPosition = TopMargin + (Count * PrintFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(Line, PrintFont, PrintBrush, LeftMargin, YPosition, new StringFormat());
                Count++;
            }

            if (Line != null)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            PrintBrush.Dispose();

        }

      



 

    

        

     


      



       

       

      

       


        #region Help
        #endregion




       

       

       

    }
}
