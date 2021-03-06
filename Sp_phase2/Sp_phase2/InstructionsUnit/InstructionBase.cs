﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.MemoryUnit;
using SP.RegistersUnit;


namespace SP.InstructionsUnit
{
    public class IexcRes
    {
        public bool Success = true;
        public bool CanMoveToNextInstruction = true;
        public Instruction caller;
        public string revStr = null;
        public int id;
        public bool inStrMode = false;
        public bool inExecMode = false;
        public int InstrLocation=0;
        public int NextExecutionAddress = 0;
    }
    public enum FuncCatchRes
    {
        Catched,
        notCached,
        Halt
    }

    public delegate void   InstructionExcutionFinished(IexcRes res);
    public delegate ushort InstructionNeedsExtraTwoBytes(Instruction caller);
    public abstract class  Instruction
    {
        protected string FuncStr;
        protected ushort funcOpcode;

        //tools for the programmer of the instruction
        private static Registers regs;
        private static Memory    memUnit;

        public static void AssignUnits(Registers _reg, Memory _mem)
        {
            regs = _reg;
            memUnit = _mem;
        }

        public Instruction(string _functStr,InstructionTypes typ)
        {
            FuncStr = _functStr;
            funcOpcode = (ushort)typ;
        }

        public FuncCatchRes ProcessDecodedInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id)
        {
            if (instr.opcode != funcOpcode) return FuncCatchRes.notCached;
            int PCbeforeExecute = regs[RegistersIndex.PC].value - 2;
            IexcRes r = ProcessTheInstruction(instr, strRev, realExcute, CallInSerial, id, memUnit, regs);
            
            r.InstrLocation = PCbeforeExecute;
            r.NextExecutionAddress = regs[RegistersIndex.PC].value;

            instructionFinsihed(r);
            if (instr.opcode == (ushort)InstructionTypes.HLT) return FuncCatchRes.Halt;
            return FuncCatchRes.Catched ;
        }
        protected abstract IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id,Memory mem,Registers regs);
        


        public  event InstructionExcutionFinished instructionFinsihed;
        public  event InstructionNeedsExtraTwoBytes instructionNeedsTwoBytes;
        protected ushort GetExtra2Bytes(Instruction caller)
        {
            return instructionNeedsTwoBytes(caller);
        }



    }
    
}
