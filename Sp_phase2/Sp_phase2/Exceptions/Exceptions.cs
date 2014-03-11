using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Exceptions
{
    class ParseException : Exception
    {
        public ParseException() : base() { }
        public ParseException(string message) : base(message) { }
    
    }
    class IlligalRecordTypeException : ParseException 
    {
        public IlligalRecordTypeException() : base("Record type is undefined! it must be type 1 or type 2") { }
    }
    class RecordFormatException: ParseException
    {
        public RecordFormatException(string message) : base(message) { }
    }
    class EmptyRecordLineException : ParseException
    {
        public EmptyRecordLineException() : base("Empty Line Record Exception") { }
    }
    class MissingRecordParameters: ParseException
    {
        public MissingRecordParameters(string message) : base(message) { }
    }


}
