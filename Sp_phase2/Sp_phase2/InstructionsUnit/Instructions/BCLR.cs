using SP.RegistersUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.InstructionsUnit.Instructions
{
    class BCLR : Instruction
    {

        public BCLR() : base("BCLR", InstructionTypes.BCLR) { }
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

                    //                    x = (ushort)(x >> 4);
                    //                   x = (ushort)(x << 4);

                    if (instr.addressingMode == Decode.AddressingImmediate)
                        bytes = nBytes = GetExtra2Bytes(this);
                    else
                        bytes = regs[instr.rs].value;

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        ushort temp = regs[instr.rd].value;
                        temp = (ushort)(temp >> bytes);
                        temp = (ushort)(temp << bytes);
                        regs[instr.rd].value = temp;

                        if (bytes == 16) // else ma ra7 y9eer ta3'eer 2da ma w9el 2a5er bit
                        {
                            regs[RegistersIndex.CR][Register.Z] = 1;
                            regs[RegistersIndex.CR][Register.N] = 0;
                        }
                        else
                        {
                            if (regs[instr.rd].value == 0x00)
                                regs[RegistersIndex.CR][Register.Z] = 1;
                            else
                                regs[RegistersIndex.CR][Register.Z] = 0;

                            if (regs[instr.rd][Register.shortSignBit] == 0)
                                regs[RegistersIndex.CR][Register.N] = 0;
                            else
                                regs[RegistersIndex.CR][Register.N] = 1;


                            /*
                            regs[rd][Register.ByteSignBit]
                            regs[rd][Register.wordSignBit]
                            */
                        }

                    }
                    if (instr.sizeAndR == Decode.SizeByte)
                    {
                        ushort temp = regs[instr.rd].value;
                        temp = (ushort)(temp >> bytes);
                        temp = (ushort)(temp << bytes);

                        regs[instr.rd].lowByte = (byte)temp;

                        if (bytes == 8) // else ma ra7 y9eer ta3'eer 2da ma w9el 2a5er bit
                        {
                            regs[RegistersIndex.CR][Register.Z] = 1;
                            regs[RegistersIndex.CR][Register.N] = 0;

                        }
                        else
                        {
                            if (regs[instr.rd].lowByte == 0x00)
                                regs[RegistersIndex.CR][Register.Z] = 1;
                            else
                                regs[RegistersIndex.CR][Register.Z] = 0;

                            if (regs[instr.rd][Register.byteSignBit] == 0)
                                regs[RegistersIndex.CR][Register.N] = 0;
                            else
                                regs[RegistersIndex.CR][Register.N] = 1;


                            /*
                            regs[rd][Register.ByteSignBit]
                            regs[rd][Register.wordSignBit]
                            */
                        }

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
