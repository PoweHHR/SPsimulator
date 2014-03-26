using SP.RegistersUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.InstructionsUnit.Instructions
{
    class RSUB : Instruction
    {
        public RSUB() : base("RSUB", InstructionTypes.RSUB) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            //ushort nBytes = 0;
            if (strRev)
            {
                res.revStr = this.FuncStr;


            }
            if (realExcute)
            {
                ushort newSPAddress = (ushort)(regs[RegistersIndex.SP].value + 2);
                if (newSPAddress < 0xFC00) res.Success = false;
                else
                {
                    regs[RegistersIndex.PC].value = mem.getshortAt(regs[RegistersIndex.SP].value);
                    regs[RegistersIndex.SP].value = newSPAddress;


                }


            }

            return res;

        }





    }
}
