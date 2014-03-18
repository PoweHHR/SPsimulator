using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class OR :Instruction
    {
        public OR():base("OR",InstructionTypes.OR){}

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {

            if (instr.addressingMode == Decode.AddressingAbsoulute ||
                instr.addressingMode == Decode.AddressingImmediate ||
                instr.addressingMode == Decode.AddressingRegister)
            {
                ushort bytes;
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                    bytes = mem.getshortAt(GetExtra2Bytes(this));
                else if (instr.addressingMode == Decode.AddressingImmediate)
                    bytes = GetExtra2Bytes(this);
                else
                    bytes = regs[instr.rs].value;

                if (instr.sizeAndR == Decode.SizeWord)
                {
                    regs[instr.rd].value = (ushort)(regs[instr.rd].value | bytes);
                    bytes = regs[instr.rd].value;
                    if ((bytes & 0x8000) == 0x8000)
                        regs[RegistersIndex.CR][Register.N] = 1;
                    else
                        regs[RegistersIndex.CR][Register.N] = 0;
                    if (bytes ==0)
                        regs[RegistersIndex.CR][Register.Z] = 1;
                    else
                        regs[RegistersIndex.CR][Register.N] = 0;
                }
                else{
                    regs[instr.rd].lowByte = (byte)(regs[instr.rd].lowByte | (byte)bytes);
                    if ((bytes & 0x0080) == 0x0080)
                        regs[RegistersIndex.CR][Register.N] = 1;
                    else
                        regs[RegistersIndex.CR][Register.N] = 0;
                    if ((byte)bytes == 0)
                        regs[RegistersIndex.CR][Register.Z] = 1;
                    else
                        regs[RegistersIndex.CR][Register.N] = 0;
                }
            }
            


            return new IexcRes();
        }
    }
}
