using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;
using SP.Helpers;

namespace SP.InstructionsUnit.Instructions
{
    class LSP :Instruction
    {
        public LSP():base("LSP",InstructionTypes.LSP){}

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            ushort nBytes=0;
            if (strRev)
            {
                res.revStr = this.FuncStr+" ";
               // if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
                if (instr.addressingMode == Decode.AddressingRegister)
                    res.revStr +=  instr.rs.ToString();

            }
            if (realExcute)
            if (instr.addressingMode == Decode.AddressingAbsoulute ||
                instr.addressingMode == Decode.AddressingImmediate ||
                instr.addressingMode == Decode.AddressingRegister  )
            {
                ushort bytes=0;
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                    bytes = mem.getshortAt(nBytes = GetExtra2Bytes(this));
                else if (instr.addressingMode == Decode.AddressingImmediate)
                    bytes = nBytes = GetExtra2Bytes(this);
                else if (instr.addressingMode == Decode.AddressingRegister)
                    bytes = regs[instr.rs].value;

                regs[RegistersIndex.SP].value = bytes; 
               
            }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes()) nBytes = GetExtra2Bytes(this);
            if (strRev)
            {
                if (instr.addressingMode == Decode.AddressingImmediate)
                {
                    res.revStr += "#$" + nBytes.ToString("X"); 
                }
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                {
                    res.revStr += "$" + nBytes.ToString("X");
                }
            }

            return res;
        }
    }
}
