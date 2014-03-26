using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class BSUB : Instruction
    {
        public BSUB() : base("BRA", InstructionTypes.BSUB) { }

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

                ushort newSPAddress = (ushort)(regs[RegistersIndex.SP].value - 2);
                if (newSPAddress < 0xFC00) res.Success = false;
                else
                {
                    regs[RegistersIndex.SP].value = newSPAddress;
                    mem.setshortAt(newSPAddress, regs[RegistersIndex.PC].value);
                }
                if (res.Success)
                    if (instr.sizeAndR == Decode.SizeWord)
                    {

                        bytes = GetExtra2Bytes(this);
                        regs[RegistersIndex.PC].value = bytes;

                    }
                    else
                    {

                        short offset = (short)Helpers.Helper.Extend10bit(instr.offset10);
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