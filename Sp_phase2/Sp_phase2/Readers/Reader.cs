using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Readers
{
    public abstract class Reader
    {
        /// <summary>
        /// prepare the streamReading unit and set the reading pointer on the head of the data; at Line0
        /// </summary>
        public abstract void OpenReadingSession();
        /// <summary>
        /// Create session from the given data in the parameter data
        /// </summary>
        /// <param name="data">the data to be parsed to lines</param>
        public abstract void OpenReadingSession(string data);
        /// <summary>
        /// always gets a line from the data and advance the pointer to the next line of data
        /// </summary>
        public abstract string GetNextLine();
        /// <summary>
        /// Get the number of line in the data
        /// </summary>
        public abstract int GetLineCount();
    }
}
