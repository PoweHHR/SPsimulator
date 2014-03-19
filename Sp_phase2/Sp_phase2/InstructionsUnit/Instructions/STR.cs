using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;
using SP.Helpers;

namespace SP.InstructionsUnit.Instructions
{
    class STR :Instruction
    {
        public STR():base("STR",InstructionTypes.STR){}

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            ushort nBytes=0;
            if (strRev)
            {
                res.revStr = this.FuncStr;
                if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
                res.revStr += " "; //+instr.rd.ToString() + ",";
                //if (instr.addressingMode == Decode.AddressingRegister)
                //    res.revStr +=  instr.rs.ToString();

            }
            if (realExcute)
            if (instr.addressingMode == Decode.AddressingAbsoulute || 
                instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
            {
                ushort bytes=0;
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                    bytes = nBytes = GetExtra2Bytes(this);
                else if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                    bytes =(ushort)( regs[instr.rd].value +  (short)Helper.Extend12bit( nBytes = GetExtra2Bytes(this)        ));
                //else if (instr.addressingMode == Decode.AddressingRegister)
                //    bytes = bytes = regs[instr.rs].value;

                if (instr.sizeAndR == Decode.SizeWord)
                {
                    mem.setshortAt(bytes,regs[instr.rs].value);
                   
                }
                else{
                    mem[bytes]  = regs[instr.rs].lowByte;
                    
                }
            }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes()) nBytes = GetExtra2Bytes(this);
            if (strRev)
            {
               
                if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                {
                    if (nBytes != 0)
                        res.revStr += "$" + nBytes.ToString("X");

                    res.revStr += "[" + instr.rd.ToString() + "]," +
                                  instr.rs.ToString();
                }
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                {
                    res.revStr += "$" + nBytes.ToString("X") + "," + instr.rs.ToString();
                }

            }

            return res;
        }
    }
}
