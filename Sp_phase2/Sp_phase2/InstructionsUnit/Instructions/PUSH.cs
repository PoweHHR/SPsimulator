using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class PUSH : Instruction
    {
        public PUSH() : base("PUSH", InstructionTypes.PUSH) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            //ushort nBytes = 0;
            if (strRev)
            {
                res.revStr = this.FuncStr;
                if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
                res.revStr += " " + instr.rs.ToString();

            }
            if (realExcute)
                if (instr.addressingMode == Decode.AddressingRegister)
                {
                    ushort bytes = 0;
                    bytes = regs[instr.rs].value;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        ushort newSPAddress = (ushort)(regs[RegistersIndex.SP].value - 2);
                        if (newSPAddress < 0xFC00) res.Success = false;
                        else
                        {
                            regs[RegistersIndex.SP].value = newSPAddress;
                            mem.setshortAt(newSPAddress, regs[instr.rs].value);
                        }
                    }
                    else
                    {
                        ushort newSPAddress = (ushort)(regs[RegistersIndex.SP].value - 1);
                        if (newSPAddress < 0xFC00) res.Success = false;
                        else
                        {
                            regs[RegistersIndex.SP].value = newSPAddress;
                            mem[newSPAddress] = regs[instr.rs].lowByte;
                        }
                    }
                }




            return res;
        }
    }
}