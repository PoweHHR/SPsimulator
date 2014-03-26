using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;
using SP.Helpers;

namespace SP.InstructionsUnit.Instructions
{
    class LSHL : Instruction
    {
        public LSHL() : base("LSHL", InstructionTypes.LSHL) { }

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
                    instr.addressingMode == Decode.AddressingRegister )
                {
                    ushort bytes = 0;
                    if (instr.addressingMode == Decode.AddressingImmediate)
                    {
                        bytes = nBytes =(ushort)( 0x00FF & GetExtra2Bytes(this));   
                        //anding makes sure we are working with the lower byte only
                    }
                    else if (instr.addressingMode == Decode.AddressingRegister)
                        bytes = nBytes = regs[instr.rs].lowByte;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        //ushort tmp = regs[instr.rd].value;

                        regs[RegistersIndex.CR][Register.C] = regs[instr.rd][(ushort)(16 - bytes)];
                        regs[instr.rd].shiftWordLeft(bytes);
                        
                        bytes = regs[instr.rd].value;
                        if ((bytes & 0x8000) == 0x8000)
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
                        regs[RegistersIndex.CR][Register.C] = regs[instr.rd][(ushort)(8 - bytes)];
                        regs[instr.rd].shiftLowByteLeft(bytes);
                        bytes = regs[instr.rd].lowByte;
                        if ((bytes & 0x0080) == 0x0080)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if (bytes == 0)
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
                    nBytes = (ushort)(nBytes & 0x00FF);
                    res.revStr += "#$" + nBytes.ToString("X");
                }
            }

            return res;
        }
    }
}
