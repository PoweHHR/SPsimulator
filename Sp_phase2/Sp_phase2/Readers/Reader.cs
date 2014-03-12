using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Readers
{
    public abstract class Reader
    {
        public abstract void OpenReadingSession();
        public abstract void OpenReadingSession(string data);
        public abstract string GetNextLine();
        public abstract int GetLineCount();
    }
}
