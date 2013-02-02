
using asi.asicentral.model;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using asi.asicentral.util;

namespace asi.asicentral.database.mappings
{
    /// <summary>
    /// Used to map the appropriate Repository with the appropriate context for the model
    /// </summary>
    public class EFRegistry : Registry
    {
        public EFRegistry()
        {
            ProxyGenerator generator = new ProxyGenerator();

            //Use only one context across repository per http context or thread
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIInternetContext>().Name = "ASIInternetContext";

            //for each model - get the repository class with the appropriate context
            For<IRepository<Publication>>().Use<EFRepository<Publication>>()
                .EnrichWith(repository => generator.CreateClassProxyWithTarget(repository.GetType(), repository, new object[] { null }, new IInterceptor[] { new LogInterceptor(repository.GetType()) }))
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<PublicationIssue>>().Use<EFRepository<PublicationIssue>>()
                .EnrichWith(repository => generator.CreateClassProxyWithTarget(repository.GetType(), repository, new object[] { null }, new IInterceptor[] { new LogInterceptor(repository.GetType()) }))
                .Ctor<IValidatedContext>().Named("ASIInternetContext");
        }
    }
}
