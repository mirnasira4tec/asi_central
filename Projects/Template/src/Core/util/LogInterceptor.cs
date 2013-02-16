using asi.asicentral.interfaces;
using asi.asicentral.services;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace asi.asicentral.util
{
    public class LogInterceptor : IInterceptor
    {
        private Type _type;
        private ILogService _log;

        /// <summary>
        /// Constructor passing the target type
        /// </summary>
        /// <param name="type"></param>
        public LogInterceptor(Type type)
        {
            _type = type;
            _log = LogService.GetLog(type != null ? type.FullName : "Unknown");
            _log.Debug(String.Format("Instanciating {0}", type != null ? type.FullName : "Unknown"));
        }

        /// <summary>
        /// Interceptor method, log the information
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            if (_log.IsDebugEnabled)
            {
                Exception exception = null;
                DateTime startTime = DateTime.Now;
                _log.Debug(String.Format("Start {0}", invocation.Method.Name));
                int i = 0;
                foreach (object param in invocation.Arguments)
                {
                    _log.Debug(String.Format("\t{0} Param[{1}] = {2}", invocation.Method.Name, i++, Format(param)));
                }
                try
                {
                    invocation.Proceed();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message + ex.StackTrace);
                    exception = ex;
                }
                DateTime endTime = DateTime.Now;
                var duration = endTime.Subtract(startTime).TotalSeconds.ToString("N3");
                _log.Debug(string.Format("Duration of {0}(): {1}s", invocation.Method.Name, duration));
                if (exception != null) throw exception;
            }
            else
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception exception)
                {
                    _log.Error(exception.Message + exception.StackTrace);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Formats the parameters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Format(object value)
        {
            if (value == null) return "null";
            switch (value.GetType().FullName)
            {
                case "System.Web.Routing.RequestContext":
                    RequestContext context = (RequestContext)value;
                    StringBuilder routeData = new StringBuilder();
                    foreach (object routeValue in context.RouteData.Values.Values)
                    {
                        routeData.Append("/").Append(routeValue);
                    }
                    return routeData.ToString();
                default:
                    return value.ToString();
            }
        }
    }
}
