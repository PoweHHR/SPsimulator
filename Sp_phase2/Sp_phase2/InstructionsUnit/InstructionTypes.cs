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
        ADD     = 0x2000,
        SUB     = 0x2800,
        MULS    = 0x3000,
        NOT     = 0x5000
    }
}
