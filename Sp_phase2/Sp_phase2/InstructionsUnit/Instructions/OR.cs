using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.InstructionsUnit.Instructions
{
    class OR :Instruction
    {
        public OR():base("OR",InstructionTypes.OR){}

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers.Registers regs)
        {
            if (instr.addressingMode == Decode.AddressingAbsoulute)
            {
                if (instr.sizeAndR == Decode.SizeWord)
                {
                    ushort bytes = GetExtra2Bytes(this);
                    regs[instr.rd].value = (ushort)(regs[instr.rd].value | bytes);
                }
                else
                {
                    ushort bytes = GetExtra2Bytes(this);
                    regs[instr.rd].lowByte = (byte)(regs[instr.rd].lowByte | (byte)bytes);


                }
            }


            return new IexcRes();
        }
    }
}
