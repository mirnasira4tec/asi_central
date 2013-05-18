using asi.asicentral.interfaces;
using asi.asicentral.services;
using EFTracingProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public abstract class BaseContext : DbContext, IValidatedContext
    {
        private IDictionary<Type, PropertyInfo> _supportedTypes;
        private ILogService _log;

        public BaseContext(string connectionString)
            : base(CreateTracingConnection(connectionString), true)
        {
            if (_supportedTypes == null)
            {
                IDictionary<Type, PropertyInfo> supportedTypes = new Dictionary<Type, PropertyInfo>();
                //read the dbset properties and create a hasmap of 
                var properties = this.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    if (info.PropertyType.IsGenericType)
                    {
                        //only analyze dbset properties
                        var genericType = info.PropertyType.GetGenericTypeDefinition();
                        if (genericType.FullName.StartsWith("System.Data.Entity.DbSet"))
                        {
                            //record the generic type of the 
                            var genericArgument = info.PropertyType.GetGenericArguments();
                            if (genericArgument != null && genericArgument.Length > 0)
                                supportedTypes.Add(genericArgument[0], info);
                        }
                    }
                }
                _supportedTypes = supportedTypes;
            }
        }

        public void EnableTracing(Type type)
        {
            _log = LogService.GetLog(type);
            // enable sql tracing
            ((IObjectContextAdapter)this).ObjectContext.EnableTracing();
            IEnumerable<EFTracingConnection> traceConnections = ((IObjectContextAdapter)this).ObjectContext.Connection.GetTracingConnections();
            List<EFTracingConnection> connectionList = new List<EFTracingConnection>(traceConnections);
            connectionList.ForEach(
                c =>
                {
                    c.CommandExecuting += (s, e) => LogSql(0, e);
                    c.CommandFinished += (s, e) => LogSql(1, e);
                    c.CommandFailed += (s, e) => LogSql(2, e);
                });
        }

        /// <summary>
        /// The logging method.  Overwrite this method to change the logging strategy.
        /// </summary>
        /// <param name="e">The DB command event.</param>
        protected void LogSql(int type, CommandExecutionEventArgs e)
        {
            if (type == 0) _log.Debug("Starting a SQL Execution");
            else if (type == 1) _log.Debug("Executed SQL (" + e.Duration + "): " + e.ToTraceString());
            else _log.Error("Failed to execute SQL: " + e.ToTraceString());
        }

        #region IValidatedContext

        public void Supports(Type type)
        {
            if (!_supportedTypes.Keys.Contains(type))
                if (type.BaseType == null || !_supportedTypes.Keys.Contains(type.BaseType))
                    throw new Exception("Invalid context for the class: " + type.FullName);
        }

        public DbSet<T> GetSet<T>() where T : class
        {
            PropertyInfo info = null;
            Type type = typeof(T);
            if (_supportedTypes.Keys.Contains(type))
                info = _supportedTypes[typeof(T)];
            else if (type.BaseType != null && _supportedTypes.Keys.Contains(type))
                info = _supportedTypes[type.BaseType];
            DbSet<T> value = info.GetValue(this, null) as DbSet<T>;
            return value;
        }

        #endregion IValidatedContext 

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && _supportedTypes != null)
            {
                _supportedTypes.Clear();
                _supportedTypes = null;
            }
        }

        #endregion

        #region tracing supporting methods

        private static DbConnection CreateTracingConnection(string nameOrConnectionString)
        {
            try
            {
                // this only supports entity connection strings http://msdn.microsoft.com/en-us/library/cc716756.aspx
                return EFTracingProviderUtils.CreateTracedEntityConnection(nameOrConnectionString);
            }
            catch (ArgumentException)
            {
                // an invalid entity connection string is assumed to be a normal connection string name or connection string (Code First)
                ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[nameOrConnectionString.Replace("name=", "")];
                string connectionString;
                string providerName;

                if (connectionStringSetting != null)
                {
                    connectionString = connectionStringSetting.ConnectionString;
                    providerName = connectionStringSetting.ProviderName;
                }
                else
                {
                    providerName = "System.Data.SqlClient";
                    connectionString = nameOrConnectionString;
                }

                return CreateTracingConnection(connectionString, providerName);
            }
        }

        private static EFTracingConnection CreateTracingConnection(string connectionString, string providerInvariantName)
        {
            string wrapperConnectionString =
                String.Format(@"wrappedProvider={0};{1}", providerInvariantName, connectionString);

            EFTracingConnection connection =
                new EFTracingConnection
                {
                    ConnectionString = wrapperConnectionString
                };
            return connection;
        }

        #endregion
    }
}
