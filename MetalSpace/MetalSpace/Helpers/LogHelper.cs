
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>LogHelper</c> class contains all the neccessary tools to save
    /// message in a log file. 
    /// </summary>
    public class LogHelper
    {
        #region Fields

        /// <summary>
        /// Name of the log file.
        /// </summary>
        private string _logFileName;

        /// <summary>
        /// StreamWriter used to save the log messages.
        /// </summary>
        private StreamWriter _writer = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>LogHelper</c> class.
        /// </summary>
        /// <param name="logFileName">Name of the log file.</param>
        public LogHelper(string logFileName)
        {
            _logFileName = logFileName;

            // Open the file that contains the log messages
            FileStream file = new FileStream(_logFileName, FileMode.OpenOrCreate,
                                             FileAccess.Write, FileShare.ReadWrite);
            _writer = new StreamWriter(file);

            // Go to the end of the file
            _writer.BaseStream.Seek(0, SeekOrigin.End);

            // Activate autoflush
            _writer.AutoFlush = true;

            // Add initial information
            _writer.WriteLine("Session started at: " + DateTime.Today);
        }

        #endregion

        #region Write Method

        /// <summary>
        /// Write a new message in the log file.
        /// </summary>
        /// <param name="message">Message to be writed.</param>
        public void Write(string message)
        {
            DateTime ct = DateTime.Now;
            string s = "[" + ct.Hour.ToString("00") + ":" + 
                       ct.Minute.ToString("00") + ":" + 
                       ct.Second.ToString("00") + "] " + message;

            _writer.WriteLine(s);
        }

        #endregion
    }
}
