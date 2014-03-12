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

namespace SP.GUI
{
    public partial class RecordsWindow : Form
    {

        public Memory memUnit = new Memory();
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
    
    
        public void LoadFile(string file){
            fReader = new FileReader (file);
            int linesCount = fReader.GetLineCount();
            int LineCounter=-1;
            fReader.OpenReadingSession();
           
            List<Record> records = new List<Record>();
            List<Filter> filters = new List<Filter>();
            
            filters.Add(new CheckSumFilter());
            filters.Add(new duplicateRecorcdType1Filter());
            filters.Add(new AddressSeqFilter() );
            filters.Add(new MemoryBoundFilter());

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
                            ErrorsList.Items.Add(filters[j].GetReason());
                        }
                    }

                    
                   
                }catch(ParseException e){
                    NoErrorsFound = false;
                    ErrorsList.Items.Add(e.Message +"(Line." + LineCounter+")"  );
                }
            }
            RecordBox.Text = Data;
           
        }
    
    }
}
