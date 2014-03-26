using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.InstructionsUnit
{
    public enum InstructionTypes
    {
        LD      = 0x0000,
        STR     = 0x0800,
        AND     = 0x4800,
        OR      = 0x5800,
        HLT     = 0xE000,
        LSP     = 0x1000,
        NOT     = 0x5000,
        PUSH    = 0x7800,
        POP     = 0x8000,
        EXG     = 0x1800,
        ADD     = 0x2000,
        SUB     = 0x2800,
        MULS    = 0x3000,
        DIVU    = 0x3800,
        MIN     = 0x4000,
        ASHR    = 0x6000,
        LSHL    = 0x6800,
        RCR     = 0x7000,
        BRA     = 0xA800,
        BEQ     = 0xB000,
        BNE     = 0xB800,
        BCS     = 0xC000,
        BLT     = 0xC800,
        BSUB    = 0xD000,
        RSUB    = 0xD800,
        BCLR    = 0xA000,
        BSS     = 0x9800,
        BTST    = 0x9000,
        CMP     = 0x8800


    }
}
