using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public abstract class BaseContext : DbContext, IValidatedContext
    {
        private static IDictionary<Type, PropertyInfo> _supportedTypes;

        public BaseContext(string connectionString) : base(connectionString)
        {
            if (_supportedTypes == null)
            {
                _supportedTypes = new Dictionary<Type, PropertyInfo>();
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
                                _supportedTypes.Add(genericArgument[0], info);
                        }
                    }
                }
            }
        }

        #region IValidatedContext

        public void Supports(Type type)
        {
            if (!_supportedTypes.Keys.Contains(type))
            {
                throw new Exception("Invalid context for the class: " + type.FullName);
            }
        }

        public DbSet<T> GetSet<T>() where T : class
        {
            PropertyInfo info = _supportedTypes[typeof(T)];
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
    }
}
