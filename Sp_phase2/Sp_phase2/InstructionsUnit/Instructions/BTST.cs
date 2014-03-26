using SP.RegistersUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.InstructionsUnit.Instructions
{
    class BTST : Instruction
    {
        public BTST() : base("BTST", InstructionTypes.BTST) { }

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
                if (instr.addressingMode == Decode.AddressingImmediate ||
                    instr.addressingMode == Decode.AddressingRegister)
                {
                    ushort bytes = 0;

                    if (instr.addressingMode == Decode.AddressingImmediate)
                        bytes = nBytes = GetExtra2Bytes(this);
                    else
                        bytes = regs[instr.rs].value;

                    if (instr.sizeAndR == Decode.SizeWord && bytes > 7)
                    {

                        int pos = (int)Math.Pow(2.0, Convert.ToDouble(bytes)); // find the position ,,, start count from 0

                        int chk = (regs[instr.rd].value & pos);

                        if (chk == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                    }
                    if (instr.sizeAndR == Decode.SizeByte && bytes < 8)
                    {
                        int pos = (int)Math.Pow(2.0, Convert.ToDouble(bytes)); // find the position ,,, start count from 0

                        int chk = (regs[instr.rd].value & pos);

                        if (chk == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;

                    }
                }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes())
                nBytes = GetExtra2Bytes(this);
            if (strRev)
            {
                if (instr.addressingMode == Decode.AddressingImmediate)
                {
                    res.revStr += "#$" + nBytes.ToString("X");
                }


            }

            return res;
        }
    }
}
