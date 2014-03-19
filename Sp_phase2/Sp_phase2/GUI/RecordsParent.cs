using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP.GUI
{
    public partial class RecordsParent : Form
    {
        private int childFormNumber = 0;

        public RecordsParent()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new RecordsWindow();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f == null) return;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                ((RecordsWindow)this.ActiveMdiChild).LoadFileToRecordBox(FileName);
            }
           
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f != null)
            {
                if (f.getRichEditBox().SelectionLength>0)
                {
                    f.getRichEditBox().Cut();
                }
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f != null)
            {
                if (f.getRichEditBox().SelectionLength > 0)
                    f.getRichEditBox().Copy();   
            }

        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f != null)
            {
                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                {
                    f.getRichEditBox().Paste();
                }
            }
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (RecordsWindow childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void memoryViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryView memForm = new MemoryView(((RecordsWindow)this.ActiveMdiChild).memUnit);
       
            memForm.Show();
        }

        private void checkForErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RecordsWindow)this.ActiveMdiChild).checkforErrors();
        }

        private void loadToMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RecordsWindow)this.ActiveMdiChild).LoadFileToMemory();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // ((RecordsWindow)this.ActiveMdiChild).runCode();
            ((RecordsWindow)this.ActiveMdiChild).ExcuteAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f != null)
            {
                if (f.getRichEditBox().CanUndo)
                {
                    f.getRichEditBox().Undo();
                    f.getRichEditBox().ClearUndo();
                }   
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f != null)
            {
                if (f.getRichEditBox().TextLength>0)
                {
                    f.getRichEditBox().SelectAll();
                }
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsWindow f = ((RecordsWindow)this.ActiveMdiChild);
            if (f != null)
            {
                if (f.getRichEditBox().CanRedo)
                {
                    f.getRichEditBox().Redo()
                    ;
                }
            }
        }

        private void reverseTheCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RecordsWindow)this.ActiveMdiChild).ReverseCode();
        }

        private void excuteNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RecordsWindow)this.ActiveMdiChild).ExcuteCode();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ((RecordsWindow)this.ActiveMdiChild).ExcuteCode();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ((RecordsWindow)this.ActiveMdiChild).ExcuteAll();
        }
    }
}
