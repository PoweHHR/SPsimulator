using SP.RegistersUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SP.InstructionsUnit.Instructions
{
    class DIVU : Instruction
    {
        public DIVU() : base("DIVU", InstructionTypes.MIN) { }
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
                        bytes = regs[instr.rs].value;
                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        ushort TempRes;
                        ushort rd = regs[instr.rd].value;
                        ushort rs = bytes;
                        if (rs == 0x0000) { res.Success = false; }
                        else
                        {
                            TempRes = regs[instr.rd].value = (ushort)(rd / rs);


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
                    }
                    else
                    {
                        byte TempRes;
                        byte rd = (byte)regs[instr.rd].value;
                        byte rs = (byte)bytes;
                        if (rs == 0x00) { res.Success = false; }
                        else
                        {
                            TempRes = regs[instr.rd].highByte = (byte)(rd / rs);
                            regs[instr.rd].lowByte = (byte)(rd - TempRes * rs);

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
                    }

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