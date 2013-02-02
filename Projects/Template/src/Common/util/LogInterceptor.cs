using asi.asicentral.interfaces;
using asi.asicentral.services;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util
{
    public class LogInterceptor : IInterceptor
    {
        private Type _type;
        private ILogService _log;

        public LogInterceptor(Type type)
        {
            _type = type;
            _log = LogService.GetLog(type != null ? type.FullName : "Unknown");
            _log.Debug(String.Format("Instanciating {0}", type != null ? type.FullName : "Unknown"));
        }

        public void Intercept(IInvocation invocation)
        {
            DateTime startTime = DateTime.Now;
            _log.Debug(String.Format("Start {0}", invocation.Method.Name));
            invocation.Proceed();
            DateTime endTime = DateTime.Now;
            var duration = endTime.Subtract(startTime).TotalSeconds.ToString("N3");
            _log.Debug(string.Format("Duration of {0}(): {1}s", invocation.Method.Name, duration));
        }
    }
}
