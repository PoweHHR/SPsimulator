using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class BNE : Instruction
    {
        public BNE() : base("BNE", InstructionTypes.BNE) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();

            if (strRev)
            {
                res.revStr = this.FuncStr;
                if (instr.sizeAndR == Decode.SizeWord) res.revStr += " &$";


            }
            if (realExcute)
            {
                ushort bytes = 0;



                if (instr.sizeAndR == Decode.SizeWord)
                {

                    bytes = GetExtra2Bytes(this);
                    if (regs[RegistersIndex.CR][Register.Z] == 0)

                        regs[RegistersIndex.PC].value = bytes;

                }
                else
                {

                    short offset = (short)Helpers.Helper.Extend10bit(instr.offset10);
                    if (regs[RegistersIndex.CR][Register.Z] == 0)
                        regs[RegistersIndex.PC].value = (ushort)(regs[RegistersIndex.PC].value + offset);
                }

            }
            ushort nbytes = 0;
            if (!realExcute && strRev && (instr.sizeAndR == Decode.SizeWord)) nbytes = GetExtra2Bytes(this);
            if (strRev)
            {
                if (instr.sizeAndR == Decode.SizeWord)
                {
                    res.revStr += nbytes.ToString("X");
                }
                else
                {

                    res.revStr += " $" + (instr.offset10).ToString("X");
                }

            }


            return res;
        }
    }
}