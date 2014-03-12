using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SP.MemoryUnit;

namespace SP.GUI
{
    public partial class MemoryView : Form
    {

        uint oldval=0;
        public Memory memUnit ;//= new Memory();

        public MemoryView(Memory memHandler)
        {
            memUnit = memHandler;
            InitializeComponent();
            richTextBox1.Text = "";
            //memUnit[0] = (byte)'H';
            //memUnit[1] = (byte)'O';
            //memUnit[2] = (byte)'S';
            //memUnit[3] = (byte)'A';
            //memUnit[4] = (byte)'M';

            for (int i = 0; i < 16 * 21; i+=16 )
                richTextBox1.Text += memUnit.GetGroup16DataString(i);
                

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
           // this.Text = vScrollBar1.Value.ToString();
           // richTextBox1.Text = "";
            string str = "";
            for (int i = e.NewValue*16; i < e.NewValue*16 + 21*16; i += 16)
                str += memUnit.GetGroup16DataString(i);
            richTextBox1.Text = str;

            AddressBox.Text = (e.NewValue * 16).ToString("X4");
           
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int address;
            uint d = Convert.ToUInt32(txtSearch.Text,16);
            if (oldval == d)
            {
                address = memUnit.FindNextShort();
                if (address == -1) memUnit.OpenSearchSession((ushort)d);
                else
                {
                    AddressBox.Text = address.ToString("X4");
                    address = address - address % 16;
                    string str = "";
                    for (int i = address; i < address + 21 * 16; i += 16)
                        str += memUnit.GetGroup16DataString(i);
                    richTextBox1.Text = str;
                }
            }
            else
            {
                oldval = d;
                memUnit.OpenSearchSession((ushort)d);
                address = memUnit.FindNextShort();
                if (address == -1) memUnit.OpenSearchSession((ushort)d);
                else
                {
                    AddressBox.Text = address.ToString("X4");
                    address = address - address % 16;
                    string str = "";
                    for (int i = address; i < address + 21 * 16; i += 16)
                        str += memUnit.GetGroup16DataString(i);
                    richTextBox1.Text = str;
                }
               
            }
        }
    }
}
