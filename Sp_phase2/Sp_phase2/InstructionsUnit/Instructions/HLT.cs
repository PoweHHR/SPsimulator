using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;
using SP.Helpers;

namespace SP.InstructionsUnit.Instructions
{
    class HLT :Instruction
    {
        public HLT():base("HLT",InstructionTypes.HLT){}

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            
            if (strRev)
            {
                res.revStr = this.FuncStr;
            }
            return res;
        }
    }
}
