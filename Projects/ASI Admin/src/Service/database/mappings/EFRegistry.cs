using asi.asicentral.model.sgr;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.model.store;

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

            For<IRepository<Order>>().Use<EFRepository<Order>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<OrderDetail>>().Use<EFRepository<OrderDetail>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<OrderProduct>>().Use<EFRepository<OrderProduct>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<OrderCreditCard>>().Use<EFRepository<OrderCreditCard>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<SupplierMembershipApplication>>().Use<EFRepository<SupplierMembershipApplication>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<DistributorMembershipApplication>>().Use<EFRepository<DistributorMembershipApplication>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");
        }
    }
}
