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
        MIN     = 0x4000
    }
}
