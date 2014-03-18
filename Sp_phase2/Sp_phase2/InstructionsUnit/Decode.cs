using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.RegistersUnit;
namespace SP.InstructionsUnit
{

   

    public class Decode
    {
        public const ushort AddressingIndirectWithDisplacement = 0x0000;
        public const ushort AddressingRegister = 0x0100;
        public const ushort AddressingImmediate= 0x0200;
        public const ushort AddressingAbsoulute= 0x0300;
        public const ushort SizeByte = 0x0000;
        public const ushort SizeWord = 0x0400;

        public ushort opcode;
        public ushort sizeAndR;
        public ushort addressingMode;
        public ushort offset10;
        public RegistersIndex rs;
        public RegistersIndex rd;
        public static Decode DecodeInstruction(ushort instr)
        {
            Decode d = new Decode();
            d.opcode         = (ushort)(instr & 0xF800);
            d.sizeAndR       = (ushort)(instr & 0x0400);
            d.addressingMode = (ushort)(instr & 0x0300);
            d.rs = (RegistersIndex)((instr & 0x00E0) >> 5);
            d.rd = (RegistersIndex)((instr & 0x001C) >> 2);
            d.offset10 = (ushort)(instr & 0x03FF);
            return d;

        }

    }
}
