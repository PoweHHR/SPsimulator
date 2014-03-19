using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;
using SP.Helpers;

namespace SP.InstructionsUnit.Instructions
{
    class EXG : Instruction
    {
        public EXG() : base("EXG", InstructionTypes.EXG) { }

        protected override IexcRes ProcessTheInstruction(Decode instr, bool strRev, bool realExcute, bool CallInSerial, int id, MemoryUnit.Memory mem, Registers regs)
        {
            IexcRes res = new IexcRes();
            ushort nBytes = 0;
            ushort temp;
            if (strRev)
            {

                res.revStr = this.FuncStr;
                if (instr.sizeAndR == Decode.SizeWord) res.revStr += ".W";
                res.revStr += " ";

                //if (instr.addressingMode == Decode.AddressingRegister)
                //    res.revStr +=  instr.rs.ToString();

            }
            /*
             if (realExcute)
            if (instr.addressingMode == Decode.AddressingAbsoulute || 
                instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
            {
                ushort bytes=0;
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                    bytes = nBytes = GetExtra2Bytes(this);
                else if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                    bytes =(ushort)( regs[instr.rd].value +  (short)Helper.Extend12bit( nBytes = GetExtra2Bytes(this)        ));
                //else if (instr.addressingMode == Decode.AddressingRegister)
                //    bytes = bytes = regs[instr.rs].value;

                if (instr.sizeAndR == Decode.SizeWord)
                {
                    mem.setshortAt(bytes,regs[instr.rs].value);
                   
                }
                else{
                    mem[bytes]  = regs[instr.rs].lowByte;
                    
                }
            } 
             */
            if (realExcute)
                if (instr.addressingMode == Decode.AddressingAbsoulute ||
                    instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                {
                    ushort bytes = 0;
                    if (instr.addressingMode == Decode.AddressingAbsoulute)
                        bytes = nBytes = GetExtra2Bytes(this);
                    else if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                        bytes = (ushort)(regs[instr.rd].value + (short)Helper.Extend12bit(nBytes = GetExtra2Bytes(this)));

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        temp = mem[bytes];
                        mem.setshortAt(bytes, regs[instr.rs].value);
                        regs[instr.rs].value = temp;
                    }
                    else
                    {
                        temp = mem[bytes];
                        mem[bytes] = regs[instr.rs].lowByte;
                        regs[instr.rs].lowByte = (byte)temp;
                    }

                    if (instr.sizeAndR == Decode.SizeWord)
                    {
                        //  regs[instr.rd].value = bytes;
                        //bytes = regs[instr.rd].value;
                        if ((regs[instr.rs].lowByte & 0x8000) == 0x8000)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if (regs[instr.rs].value == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                    }
                    else
                    {
                        //  regs[instr.rd].lowByte = ((byte)bytes);
                        if ((regs[instr.rs].value & 0x0080) == 0x0080)
                            regs[RegistersIndex.CR][Register.N] = 1;
                        else
                            regs[RegistersIndex.CR][Register.N] = 0;
                        if ((byte)regs[instr.rs].value == 0)
                            regs[RegistersIndex.CR][Register.Z] = 1;
                        else
                            regs[RegistersIndex.CR][Register.Z] = 0;
                    }

                }


            if (!realExcute && strRev && instr.NeedsExtra2Bytes()) nBytes = GetExtra2Bytes(this);
            if (strRev)
            {
                if (instr.addressingMode == Decode.AddressingIndirectWithDisplacement)
                {
                    if (nBytes != 0)
                        res.revStr += "$" + nBytes.ToString("X");

                    res.revStr += "[" + instr.rd.ToString() + "]," +
                                  instr.rs.ToString();
                }
                if (instr.addressingMode == Decode.AddressingAbsoulute)
                {
                    res.revStr += "$" + nBytes.ToString("X") + "," + instr.rs.ToString();
                }

            }

            return res;
        }
    }
}