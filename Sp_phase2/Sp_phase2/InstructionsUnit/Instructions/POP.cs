using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class POP : Instruction
    {
        public POP() : base("POP", InstructionTypes.POP) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            //ushort nBytes = 0;
            if (strRev)
            {
                res.revStr = this.FuncStr;
                if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
                res.revStr += " " + instr.rd.ToString();

            }
            if (realExcute)
                if (instr.addressingMode == Decode.AddressingRegister)
                {

                    instr.rs = instr.rd;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        ushort newSPAddress = (ushort)(regs[RegistersIndex.SP].value + 2);
                        if ((newSPAddress < 0xFC00) & newSPAddress != 0) res.Success = false;
                        else
                        {
                            regs[instr.rs].value = mem.getshortAt(regs[RegistersIndex.SP].value);
                            regs[RegistersIndex.SP].value = newSPAddress;
                            if ((regs[instr.rs].value & 0x8000) == 0x8000)
                                regs[RegistersIndex.CR][Register.N] = 1;
                            else
                                regs[RegistersIndex.CR][Register.N] = 0;
                            if (regs[instr.rs].value == 0)
                                regs[RegistersIndex.CR][Register.Z] = 1;
                            else
                                regs[RegistersIndex.CR][Register.Z] = 0;

                        }
                    }
                    else
                    {
                        ushort newSPAddress = (ushort)(regs[RegistersIndex.SP].value + 1);
                        if ((newSPAddress < 0xFC00) & newSPAddress != 0) res.Success = false;
                        else
                        {
                            regs[instr.rs].lowByte = mem[regs[RegistersIndex.SP].value];
                            regs[RegistersIndex.SP].value = newSPAddress;
                            if ((regs[instr.rs].lowByte & 0x0080) == 0x0080)
                                regs[RegistersIndex.CR][Register.N] = 1;
                            else
                                regs[RegistersIndex.CR][Register.N] = 0;
                            if (regs[instr.rs].lowByte == 0)
                                regs[RegistersIndex.CR][Register.Z] = 1;
                            else
                                regs[RegistersIndex.CR][Register.Z] = 0;

                        }
                    }
                }




            return res;
        }
    }
}