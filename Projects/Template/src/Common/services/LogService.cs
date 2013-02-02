using asi.asicentral.interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    /// <summary>
    /// Service to be used to log information
    /// </summary>
    public class LogService : ILogService
    {
        private ILog _log;

        private LogService(string name)
        {
            _log = LogManager.GetLogger(name);
        }

        private LogService(Type type)
        {
            _log = LogManager.GetLogger(type);
        }

        /// <summary>
        /// Whether debug is enabled
        /// </summary>
        public bool IsDebugEnabled
        {
            get
            {
                return _log.IsDebugEnabled;
            }
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            _log.Debug(message);
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// Gets a instance of the log referencing a name
        /// </summary>
        /// <param name="name">Name to use to reference the log</param>
        /// <returns></returns>
        public static LogService GetLog(string name) 
        {
            return new LogService(name);
        }

        /// <summary>
        /// Gets a  instance of the log for a type
        /// </summary>
        /// <param name="type">Type the log is created for</param>
        /// <returns></returns>
        public static LogService GetLog(Type type)
        {
            return new LogService(type);
        }        
    }
}
