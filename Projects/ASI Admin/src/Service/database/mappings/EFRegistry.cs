using asi.asicentral.model.sgr;
using asi.asicentral.services.interfaces;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings
{
    /// <summary>
    /// Used to map the appropriate Repository with the appropriate context for the model
    /// </summary>
    public class EFRegistry : Registry
    {
        public EFRegistry()
        {
            //Use only one context across repository per http context or thread
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIInternetContext>().Name = "ASIInternetContext";

            //for each model - get the repository class with the appropriate context
            For<IRepository<Company>>().Use<EFRepository<Company>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<Product>>().Use<EFRepository<Product>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<Category>>().Use<EFRepository<Category>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");
        }
    }
}
