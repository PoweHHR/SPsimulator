using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.MemoryUnit;
using SP.RegistersUnit;



namespace SP.InstructionsUnit
{
    
    public class InstructionExcuter
    {
        public List<Instruction> Instructions = new List<Instruction>();

        public Fetch fetcher;
        int i = 0;
        bool strRev = false;
        bool realExcu = false;
        Memory mem;
        Registers regs;
        ushort CurrentPC;
        public event InstructionExcutionFinished instructionFinsihed;
        public InstructionExcuter(Memory _mem,Registers _regs)
        {
            mem = _mem;
            regs= _regs;
            fetcher = new Fetch(_mem, _regs);
            Instruction.AssignUnits(_regs,_mem);
            Instruction.instructionNeedsTwoBytes += new InstructionNeedsExtraTwoBytes(ExtraTwoBytesGetter);
            Instruction.instructionFinsihed += new InstructionExcutionFinished(FinishedTemp);

            Instructions.Add(new Instructions.ADD());
            Instructions.Add(new Instructions.AND());
            Instructions.Add(new Instructions.DIVU());
            Instructions.Add(new Instructions.HLT());
            Instructions.Add(new Instructions.LD());
            Instructions.Add(new Instructions.LSP());
            Instructions.Add(new Instructions.MIN());
            Instructions.Add(new Instructions.MULS());
            Instructions.Add(new Instructions.NOT());
            Instructions.Add(new Instructions.OR());
            Instructions.Add(new Instructions.POP());
            Instructions.Add(new Instructions.PUSH());
            Instructions.Add(new Instructions.STR());
            Instructions.Add(new Instructions.SUB());


        
        }
        public void DestroyExcuter()
        {
            Instruction.instructionNeedsTwoBytes -= new InstructionNeedsExtraTwoBytes(ExtraTwoBytesGetter);
            Instruction.instructionFinsihed -= new InstructionExcutionFinished(FinishedTemp);
           
        }
        private void FinishedTemp(IexcRes r)
        {
            r.id = i;
            if (instructionFinsihed != null) instructionFinsihed(r);
        }
        private ushort ExtraTwoBytesGetter(Instruction caller)
        {
            return fetcher.FetchNextWord();
        }
        public void OpenReverseEngineeringSession()
        {
            regs.ResetRegisters();
            i = 0;
            strRev = true;
            realExcu = false;
            CurrentPC = regs[RegistersIndex.PC].value;
            regs[RegistersIndex.PC].value = mem.getshortAt(0);
            SendUpdateInterfaceEvent();

        }
        public void OpenExcutionSession(bool withString)
        {
            regs.ResetRegisters();
            strRev = withString;
            realExcu = true;
            i = 0;
            CurrentPC = regs[RegistersIndex.PC].value;
            regs[RegistersIndex.PC].value = mem.getshortAt(0);
            SendUpdateInterfaceEvent();
        }
        private void SendUpdateInterfaceEvent()
        {
            IexcRes r = new IexcRes();
            r.id = -1;
            if (instructionFinsihed != null) instructionFinsihed(r);
        }

        public bool ExecuteNextInstruction()
        {
            Decode d = Decode.DecodeInstruction(fetcher.FetchNextWord());
            FuncCatchRes res;
            foreach (Instruction x in Instructions)
            {
                res = x.ProcessDecodedInstruction(d,strRev,realExcu,true,i);
                if (res == FuncCatchRes.Catched) { i++; return true; }
                if (res == FuncCatchRes.Halt   ) return false;
            }

            return false;//can not read more
        }



    }
}
