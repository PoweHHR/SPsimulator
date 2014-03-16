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
using SP.Registers;
using SP.Helpers;
using SP.GUI;
using SP.Records;
using SP.InstructionsUnit;

namespace SP.GUI
{
    public partial class RecordsWindow : Form
    {

        public Memory memUnit = new Memory();
        List<Record> records = new List<Record>();
        public SP.Registers.Registers regs = new Registers.Registers();
        public FileReader fReader;

        
        public RecordsWindow()
        {
            InitializeComponent();
        }

        private void RecordsWindow_Resize(object sender, EventArgs e)
        {
           // panel1.Size = this.Size;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

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
           

            string Line;
            LineParser parser = new LineParser();
            bool NoErrorsFound = true;
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



                }
                catch (ParseException e)
                {
                    NoErrorsFound = false;
                    ErrorsList.Items.Add("Error: "+ e.Message + "(Line." + LineCounter + ")");
                }
            }
            //RecordBox.Text = Data;


            //load to memory part
            
        }

        public void LoadFileToMemory(){
            memUnit.Reset();
            Reader  fReader= new StringReader(RecordBox.Text);
            ErrorsList.Items.Clear();
            int LineCounter=-1;
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
                        memUnit.WriteBytesAtAddress(records[i].address, records[i].data);
                    else
                        memUnit.setshortAt(0, records[i].address);
                }
            }
        }


        public void ExcuteCode()
        {
            InstructionExcuter exec = new InstructionExcuter(memUnit, regs);
            exec.OpenExcutionSession(false);
            while (exec.ExecuteNextInstruction()) ;
            MessageBox.Show("X4: " + regs[RegistersIndex.X4].value.ToString("X4"));

        }
    }
}
