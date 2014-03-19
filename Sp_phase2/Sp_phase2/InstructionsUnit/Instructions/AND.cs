using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class AND : Instruction
    {
        public AND() : base("AND", InstructionTypes.AND) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            ushort nBytes = 0;
            if (strRev)
            {
                res.revStr = this.FuncStr;
                if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
                res.revStr += " " + instr.rd.ToString() + ",";
                if (instr.addressingMode == Decode.AddressingRegister)
                    res.revStr += instr.rs.ToString();

            }
            if (realExcute)
                if (instr.addressingMode == Decode.AddressingAbsoulute ||
                    instr.addressingMode == Decode.AddressingImmediate ||
                    instr.addressingMode == Decode.AddressingRegister)
                {
                    ushort bytes = 0;
                    if (instr.addressingMode == Decode.AddressingAbsoulute)
                        bytes = mem.getshortAt(nBytes = GetExtra2Bytes(this));
                    else if (instr.addressingMode == Decode.AddressingImmediate)
                        bytes = nBytes = GetExtra2Bytes(this);
                    else
                        bytes = regs[instr.rs].value;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        regs[instr.rd].value = (ushort)(regs[instr.rd].value & bytes);
                        bytes = regs[instr.rd].value;
                        if ((bytes & 0x8000) == Register.shortSignBit)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if (bytes == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                    }
                    else
                    {
                        regs[instr.rd].lowByte = (byte)(regs[instr.rd].lowByte & (byte)bytes);
                        bytes = regs[instr.rd].lowByte;
                        if ((bytes & 0x0080) == 0x0080)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if ((byte)bytes == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                    }
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
