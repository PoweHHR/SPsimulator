using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;

namespace SP.InstructionsUnit.Instructions
{
    class MULS : Instruction
    {
        public MULS() : base("MULS", InstructionTypes.MULS) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            ushort nBytes = 0;
            if (strRev)
            {
                res.revStr = this.FuncStr;
            //    if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
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
                        bytes =(byte) regs[instr.rs].value;

                    ushort rs =(byte) regs[instr.rd].value;
                    bool Nsource=false, Ndestination=false;
                    if ((bytes & 0x0080) == 0x0080) { Nsource = true; bytes = (byte)(-bytes); }
                    if ((rs & 0x0080) == 0x0080) { Ndestination = true; rs = (byte)(-rs); }
                     
                        ushort TempRes =(ushort )(rs*bytes);
                      if(Nsource!=Ndestination)
                          TempRes = (ushort)(-TempRes);
                      
                        regs[instr.rd].value =TempRes;

                        
                        // bytes = regs[instr.rd].lowByte;
                        if ((TempRes & 0x8000) == 0x8000)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if (TempRes == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                     

                    
                }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes()) nBytes = GetExtra2Bytes(this);
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
