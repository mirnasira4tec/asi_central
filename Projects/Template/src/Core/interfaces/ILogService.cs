using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Whether Logging debugging message is enabled
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);
    }
}
