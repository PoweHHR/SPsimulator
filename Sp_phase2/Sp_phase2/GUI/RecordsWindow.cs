using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SP.Exceptions;
using SP.Filters;
using SP.MemoryUnit;
using SP.Readers;
using SP.RegistersUnit;
using SP.Helpers;
using SP.GUI;
using SP.Records;
using SP.InstructionsUnit;

namespace SP.GUI
{
    public partial class RecordsWindow : Form
    {

        public Memory     memUnit = new Memory();
        List<Record>      records = new List<Record>();
        public Registers  regs = new Registers();
        public FileReader fReader;

        InstructionExcuter exec;
        
        public RecordsWindow()
        {
            InitializeComponent();
            RecordBox.ShortcutsEnabled = true;
            RecordBox.AllowDrop = true;
            RecordBox.AcceptsTab = true;


            //exec = new InstructionExcuter(memUnit, regs);
            //exec.instructionFinsihed += new InstructionExcutionFinished(FinishedIRexec);

        }

        private void RecordsWindow_Resize(object sender, EventArgs e)
        {
           // panel1.Size = this.Size;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        public RichTextBox getRichEditBox()
        {
            return this.RecordBox;
        }

        public void runCode()
        {
            for (int i =0 ; i < records.Count ;i++){
                if (records[i].RecordType == 1)
                {
                    regs[RegistersIndex.PC].value = records[i].address;
                    txtPC.Text = records[i].address.ToString("X4");
                    return;
                }
            }
            MessageBox.Show("Load code first!");
        }

        public void LoadFileToRecordBox(string file)
        {
            fReader = new FileReader(file);
           // int linesCount = fReader.GetLineCount();
           // int LineCounter = -1;
            fReader.OpenReadingSession();
            string Data = "";
            string Line;
            while ((Line = fReader.GetNextLine()) != null)
            {
                Data += Line + Environment.NewLine;
            }
            RecordBox.Text = Data;
        }


        public void checkforErrors()
        {
            Reader fReader = new StringReader(RecordBox.Text);
            ErrorsList.Items.Clear();
            int LineCounter = -1;
            fReader.OpenReadingSession();
            int linesCount = fReader.GetLineCount();
            records = new List<Record>();
            List<Filter> filters = new List<Filter>();

            filters.Add(new AddressSeqFilter());
            filters.Add(new ByteCountFilter());
            filters.Add(new CheckSumFilter());
            filters.Add(new duplicateRecorcdType1Filter());
            filters.Add(new MemoryBoundFilter());
            filters.Add(new MissingRecord2Filter());
            filters.Add(new MissingType1filter());
           

            string Line;
            LineParser parser = new LineParser();
            //bool NoErrorsFound = true;
            string Data = "";
            while ((Line = fReader.GetNextLine()) != null)
            {
                try
                {
                    LineCounter++;
                    Data += Line + Environment.NewLine;
                    records.Add(parser.TryParseLine(Line));

                    for (int j = 0; j < filters.Count; j++)
                    {
                        ErrorLevel e = filters[j].IsValidRecord(records[records.Count - 1], LineCounter, linesCount);
                        if (e == ErrorLevel.Error || e == ErrorLevel.Warning)
                        {
                            string s = "Error: ";
                            if (e == ErrorLevel.Warning) s = "Warning: ";
                            ErrorsList.Items.Add(s+ filters[j].GetReason() + "(Line." + LineCounter + ")");
                        }
                    }


                  //  NoErrorsFound = true;
                }
                catch (ParseException e)
                {
                   // NoErrorsFound = false;
                    ErrorsList.Items.Add("Error: "+ e.Message + "(Line." + LineCounter + ")");
                }
            }
            //RecordBox.Text = Data;


            //load to memory part
            
        }

        public void LoadFileToMemory()
        {
            LoadFileToMemory(memUnit);
        }
        public void LoadFileToMemory(Memory mem){
            mem.Reset();
            Reader  fReader= new StringReader(RecordBox.Text);
            ErrorsList.Items.Clear();
            int LineCounter=-1;
            fReader.OpenReadingSession();
            int linesCount = fReader.GetLineCount();

            records = new List<Record>();
            List<Filter> filters = new List<Filter>();

            //filters.Add(new AddressSeqFilter());
            //filters.Add(new ByteCountFilter());
            //filters.Add(new CheckSumFilter());
            //filters.Add(new duplicateRecorcdType1Filter());
            //filters.Add(new MemoryBoundFilter());
            //filters.Add(new MissingRecord2Filter());

            string Line;
            LineParser parser = new LineParser();
            bool NoErrorsFound = true;
            string Data="";
            while ( (Line = fReader.GetNextLine())!= null            ){
                try{
                    LineCounter++;
                    Data += Line + Environment.NewLine;
                    records.Add ( parser.TryParseLine(Line)) ;  

                    for (int j =0 ; j < filters.Count;j++){
                        ErrorLevel e = filters[j].IsValidRecord(records[records.Count-1],LineCounter,linesCount);
                        if (e == ErrorLevel.Error || e == ErrorLevel.Warning){
                            string s = "Error: ";
                            if (e == ErrorLevel.Warning) s = "Warning: ";
                            ErrorsList.Items.Add(s+filters[j].GetReason() + "(Line." + LineCounter + ")");
                        }
                    }

                    
                   
                }catch(ParseException e){
                    NoErrorsFound = false;
                    ErrorsList.Items.Add("Error: " +e.Message +"(Line." + LineCounter+")"  );
                }
            }
            //RecordBox.Text = Data;


            //load to memory part
            if (NoErrorsFound)
            {
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].RecordType != 1)
                        mem.WriteBytesAtAddress(records[i].address, records[i].data);
                    else
                        mem.setshortAt(0, records[i].address);
                }
            }
        }

        public void FinishedIRexec(IexcRes r)
        {
            UpdateRegistersInterface();
            if (r.id != -1)
            //MessageBox.Show(r.revStr);
            {
                if (r.inStrMode && !r.inExecMode)
                    InstrBox.Text += r.revStr + Environment.NewLine;
                
                if (r.inExecMode && r.inStrMode)
                {
                    InstrBox.SelectionBackColor = Color.White;
                    int s = InstrBox.GetFirstCharIndexFromLine(r.id + 1);
                    int e = InstrBox.GetFirstCharIndexFromLine(r.id + 2);
                    InstrBox.Select(s, e - s);
                   // InstrBox.SelectedText = r.revStr;
                    InstrBox.SelectionBackColor = Color.Red;
                }
            }
            else
            {
                if (r.inStrMode && r.inExecMode)
                {
                    int s = InstrBox.GetFirstCharIndexFromLine(r.id + 1);
                    int e = InstrBox.GetFirstCharIndexFromLine(r.id + 2);
                    InstrBox.SelectionBackColor = Color.White;
                    InstrBox.Select(s, e - s);
                    InstrBox.SelectionBackColor = Color.Red;
                }

            }
            
        }
        public void ExcuteCode()
        {
            if (exec == null)
            {
                LoadFileToMemory();
                ReverseCode();
                exec = new InstructionExcuter(memUnit, regs);
                exec.instructionFinsihed += new InstructionExcutionFinished(FinishedIRexec);
                exec.OpenExcutionSession(true);
            }
            else
                if (!exec.ExecuteNextInstruction())
                {
                    exec.IsSessionStarted = false;
                    exec.DestroyExcuter();
                    exec = null;

                }
            //exec.DestroyExcuter();
           // MessageBox.Show("X4: " + regs[RegistersIndex.X4].value.ToString("X4"));

        }
        public void ExcuteAll()
        {
            if (exec == null)
            {
                LoadFileToMemory();
                //ReverseCode();
                exec = new InstructionExcuter(memUnit, regs);
                exec.instructionFinsihed += new InstructionExcutionFinished(FinishedIRexec);
                exec.OpenExcutionSession(false);
            }
            else
            {
                while (exec.ExecuteNextInstruction()) ;
                
                    exec.IsSessionStarted = false;
                    exec.DestroyExcuter();
                    exec = null;
            }
            //exec.DestroyExcuter();
            // MessageBox.Show("X4: " + regs[RegistersIndex.X4].value.ToString("X4"));

        }
        public void ReverseCode()
        {
            
            InstrBox.Clear();
            Memory memTemp = new Memory();
           // Registers regsTemp = regs;

            //memUnit = new Memory();
            //regs = new Registers();

            LoadFileToMemory(memTemp); //reset the memory 

            InstructionExcuter exec = new InstructionExcuter( memTemp, new Registers());
            exec.instructionFinsihed += new InstructionExcutionFinished(FinishedIRexec);
            exec.OpenReverseEngineeringSession();
            while (exec.ExecuteNextInstruction()) ;

           // memUnit = memTemp; // restore old mem;
           // regs = regsTemp;
            UpdateRegistersInterface();
            exec.DestroyExcuter();
        }


        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void UpdateRegistersInterface()
        {
            txtX0.Text = regs[RegistersIndex.X0].value.ToString("X4");
            txtX1.Text = regs[RegistersIndex.X1].value.ToString("X4");
            txtX2.Text = regs[RegistersIndex.X2].value.ToString("X4");
            txtX3.Text = regs[RegistersIndex.X3].value.ToString("X4");
            txtX4.Text = regs[RegistersIndex.X4].value.ToString("X4");
            txtX5.Text = regs[RegistersIndex.X5].value.ToString("X4");
            txtX6.Text = regs[RegistersIndex.X6].value.ToString("X4");
            txtX7.Text = regs[RegistersIndex.X7].value.ToString("X4");
            txtPC.Text = regs[RegistersIndex.PC].value.ToString("X4");
            txtSP.Text = regs[RegistersIndex.SP].value.ToString("X4");
           // if (   )
            txtC.Text = regs[RegistersIndex.CR][Register.C].ToString();
            txtN.Text = regs[RegistersIndex.CR][Register.N].ToString();
            txtV.Text = regs[RegistersIndex.CR][Register.V].ToString();
            txtZ.Text = regs[RegistersIndex.CR][Register.Z].ToString();
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
