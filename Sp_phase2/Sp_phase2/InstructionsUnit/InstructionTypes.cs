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
        HLT     = 0xE000


    }
}
