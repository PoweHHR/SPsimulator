using SP.RegistersUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Helpers;

namespace SP.InstructionsUnit.Instructions
{
    class CMP : Instruction
    {
        public CMP() : base("CMP", InstructionTypes.CMP) { }

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
                    instr.addressingMode == Decode.AddressingRegister  ||
                    instr.addressingMode == Decode.AddressingAbsoulute ||
                    instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                {
                    ushort bytes = 0;

                    if (instr.addressingMode == Decode.AddressingAbsoulute)
                        if (instr.sizeAndR == Decode.SizeWord)
                            bytes = mem.getshortAt(nBytes = GetExtra2Bytes(this));
                        else
                            bytes = mem[(int)(nBytes = GetExtra2Bytes(this))];
                    else if (instr.addressingMode == Decode.AddressingImmediate)
                        bytes = nBytes = GetExtra2Bytes(this);
                    else if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                        if (instr.sizeAndR == Decode.SizeWord)
                            bytes = mem.getshortAt((ushort)(regs[instr.rs].value + (short)Helper.Extend12bit(nBytes = GetExtra2Bytes(this))));
                        else
                            bytes = mem[(ushort)(regs[instr.rs].value + (short)Helper.Extend12bit(nBytes = GetExtra2Bytes(this)))];
                    else if (instr.addressingMode == Decode.AddressingRegister)
                        bytes = nBytes = regs[instr.rs].value;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        ushort rs = regs[instr.rd].value;
                        int TempRes = ((ushort)regs[instr.rd].value - (ushort)(~bytes) + (ushort)1);
                        //regs[instr.rd].value = (ushort)TempRes;
                        //   bytes = regs[instr.rd].value;
                        if ((((ushort)TempRes) & 0x8000) == 0x8000)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if (((ushort)TempRes) == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                        if ((TempRes >> 16) != 0)
                            regs[RegistersIndex.CR][Register.C] = 1;
                        else
                            regs[RegistersIndex.CR][Register.C] = 0;
                        if ((((bytes & 0x8000) == 0x8000) & ((rs & 0x8000) == 0x0000) & ((((ushort)TempRes) & 0x8000) == 0x8000)) ||
                           (((bytes & 0x8000) == 0x0000) & ((rs & 0x8000) == 0x8000) & ((((ushort)TempRes) & 0x8000) == 0x0000)))
                            regs[RegistersIndex.CR][Register.V] = 1;
                        else
                            regs[RegistersIndex.CR][Register.V] = 0;


                    }
                    else
                    {
                        ushort rs = regs[instr.rd].value;
                        ushort TempRes = (ushort)((byte)regs[instr.rd].value + (byte)bytes + (byte)1);
                        //regs[instr.rd].lowByte = (byte)(TempRes);
                        // bytes = regs[instr.rd].lowByte;
                        if ((TempRes & 0x0080) == 0x0080)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if ((byte)TempRes == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                        if ((TempRes >> 8) != 0)
                            regs[RegistersIndex.CR][Register.C] = 1;
                        else
                            regs[RegistersIndex.CR][Register.C] = 0;
                        if ((((bytes & 0x0080) == 0x0080) & ((rs & 0x0080) == 0x0000) & ((TempRes & 0x0080) == 0x0080)) ||
                            (((bytes & 0x0080) == 0x0000) & ((rs & 0x0080) == 0x0080) & ((TempRes & 0x0080) == 0x0000)))
                            regs[RegistersIndex.CR][Register.V] = 1;
                        else
                            regs[RegistersIndex.CR][Register.V] = 0;

                    }
                }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes()) nBytes = GetExtra2Bytes(this);
            if (strRev)
            {
                if (instr.addressingMode == Decode.AddressingImmediate)
                {
                    res.revStr += "#$" + nBytes.ToString("X");
                }
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                {
                    res.revStr += "$" + nBytes.ToString("X");
                }
                if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                {
                    if (nBytes != 0)
                        res.revStr += "$" + nBytes.ToString("X");
                    res.revStr += "[" + instr.rs.ToString() + "]";
                }

            }

            return res;
        }
    }
}