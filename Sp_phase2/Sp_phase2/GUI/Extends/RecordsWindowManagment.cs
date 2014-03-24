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

    public class ListItem
    {
        public string str;
        public int id;
        public override string ToString()
        {
            return str;
        }
        public ListItem(string d, int _id) {str=d;id= _id; }
        public ListItem(int _id)
        {
            id = _id;
        }
        public override bool Equals(Object obj)
        {
            ListItem Obj= obj as ListItem;
            if (Obj == null)
                return false;
            else
                return this.id == Obj.id;
        }
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
    
    
    }

    public partial class RecordsWindow : Form
    {
        public Memory        memUnit = new Memory();
        List<Record> records = null;
        public Registers     regs = new Registers();
        public FileReader    fReader;
        InstructionExcuter   exec;
        private List<Record> GetRecords(string ObjCodeStr)
        {
            List<Record> rs = new List<Record>();
            Reader fReader = new StringReader(ObjCodeStr);
            int LineCounter = -1;
            fReader.OpenReadingSession();
            string Line;
            LineParser parser = new LineParser();
            string Data = "";
            while ((Line = fReader.GetNextLine()) != null)
            {
                try
                {
                    LineCounter++;
                    Data += Line + Environment.NewLine;
                    rs.Add(parser.TryParseLine(Line));
                }
                catch (ParseException e)
                {

                }
            }

            return rs;
        }
        
        public void LoadFileToMemory()
        {
            LoadFileToMemory(memUnit);
        }

        //Assumption: code is free of errors;
        public void LoadFileToMemory(Memory mem)
        {
                records = GetRecords(RecordBox.Text);
                
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].RecordType == 2)
                        mem.WriteBytesAtAddress(records[i].address, records[i].data);
                    else if (records[i].RecordType ==1)
                        mem.setshortAt(0, records[i].address);
                }
            
        }

        public void FinishedIRexec(IexcRes r)
        {
            UpdateRegistersInterface();
            if (r.id != -1)
            ////MessageBox.Show(r.revStr);
            {
                if (r.inStrMode && !r.inExecMode)
                {
                    InstructionsList.Items.Add(new ListItem(r.revStr, r.InstrLocation));

                }
                if (r.inExecMode)
                {
                    InstructionsList.SelectedItem = new ListItem(r.NextExecutionAddress);
                }
            }
            else
            {
                if (r.inExecMode)
                {
                    InstructionsList.SelectedItem = new ListItem(r.NextExecutionAddress);
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
                exec.OpenExcutionSession(false);
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
            InstructionsList.Items.Clear();
            Memory memTemp = new Memory();
            LoadFileToMemory(memTemp); //reset the memory 


            ushort startExec=0;
            foreach (Record r in records)
            {
                if (r.RecordType == 1)
                {
                    startExec = r.address;
                    break;
                }
            }

            foreach (Record r in records)
            {
                if (r.RecordType == 2)
                {
                    if (r.address < startExec)
                    {
                        if (r.data.Length % 2 == 0)
                        {
                            string str = "DC ";
                            for (int i = 0; i < r.data.Length; i += 2)
                            {
                                str += "$" + Helper.MakeWord(r.data[i],r.data[i+1]).ToString("X");
                                if (i +2 != r.data.Length) str+=",";

                            }
                            InstructionsList.Items.Add(new ListItem(str,r.address));
                        }
                        else
                        {
                            string str = "DC.B ";
                            for (int i = 0; i < r.data.Length; i ++)
                            {
                                str += "$" + r.data[i].ToString("X");
                                if (i + 1 != r.data.Length) str += ",";

                            }
                            InstructionsList.Items.Add(new ListItem(str, r.address));
                        }
                    }

                }
            }



            InstructionExcuter exec = new InstructionExcuter(memTemp, new Registers());
            exec.instructionFinsihed += new InstructionExcutionFinished(FinishedIRexec);
            exec.OpenReverseEngineeringSession();
            while (exec.ExecuteNextInstruction()) ;
            UpdateRegistersInterface();
            exec.DestroyExcuter();



        }


        //public void ReverseCode()
        //{
        //
        //    InstrBox.Clear();
        //    Memory memTemp = new Memory();
        //    // Registers regsTemp = regs;
        //
        //    //memUnit = new Memory();
        //    //regs = new Registers();
        //
        //    LoadFileToMemory(memTemp); //reset the memory 
        //
        //    InstructionExcuter exec = new InstructionExcuter(memTemp, new Registers());
        //    exec.instructionFinsihed += new InstructionExcutionFinished(FinishedIRexec);
        //    exec.OpenReverseEngineeringSession();
        //    while (exec.ExecuteNextInstruction()) ;
        //
        //    // memUnit = memTemp; // restore old mem;
        //    // regs = regsTemp;
        //    UpdateRegistersInterface();
        //    exec.DestroyExcuter();
        //}

    
    }
}
