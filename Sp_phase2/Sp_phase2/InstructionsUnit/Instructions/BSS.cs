using SP.RegistersUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.InstructionsUnit.Instructions
{
    class BSS : Instruction
    {
        public BSS() : base("BSS", InstructionTypes.BSS) { }

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
                if (instr.addressingMode == Decode.AddressingRegister)
                {
                    ushort bytes = 0;

                    bytes = regs[instr.rs].value;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {

                        int count = 0;
                        ushort x = bytes;
                        for (int i = 0; i < 16; i++)
                        {
                            count += x % 2;
                            x /= 2;
                        }

                        if (count == 0) // this is how we change on zero
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;

                        regs[instr.rd].lowByte = (byte)count;
                    }

                    if (instr.sizeAndR == Decode.SizeByte)
                    {
                        int count = 0;
                        ushort x = bytes;
                        for (int i = 0; i < 8; i++)
                        {
                            count += x % 2;
                            x /= 2;
                        }

                        if (count == 0) // this is how we change on zero
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;

                        regs[instr.rd].lowByte = (byte)count;

                    }
                }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes()) // hay m$ fahem l2ee$ ?! 
                nBytes = GetExtra2Bytes(this);

            return res;
        }
    }
}
