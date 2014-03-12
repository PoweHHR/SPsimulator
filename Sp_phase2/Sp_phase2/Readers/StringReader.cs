using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SP.Readers
{
    class StringReader : Reader
    {
       
        string code;
        string [] Lines;
        int i = 0;
        public StringReader(string Code)
        {
            code = Code;
        }
        public override void OpenReadingSession()
        {

           Lines  = code.Split(new string[]{Environment.NewLine,"\n"},StringSplitOptions.None);
           i = 0;
        }
        public override void OpenReadingSession(string _code)
        {
            code = _code;
            Lines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            i = 0;
        }
        public override int GetLineCount()
        {
            return Lines.Length;
        }
        public override string GetNextLine()
        {
            
            if (i >= Lines.Length) return null;
            if (Lines[i] == "" || Lines[i].CompareTo("") == 0) return null;
            return Lines[i++];
        }

    }
}
