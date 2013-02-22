using asi.asicentral.model.sgr;
using asi.asicentral.model.counselor;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.model.store;
using asi.asicentral.model.news;
using asi.asicentral.model.product;

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
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIPublicationContext>().Name = "ASIPublicationContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<InternetContext>().Name = "InternetContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ProductContext>().Name = "ProductContext";

            //for each model - get the repository class with the appropriate context

            #region ASIInternetContext

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

            For<IRepository<ASPNetMembership>>().Use<EFRepository<ASPNetMembership>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            #endregion ASIInternetContext

            #region ASIPublicationContext

            For<IRepository<CounselorCategory>>().Use<EFRepository<CounselorCategory>>()
                .Ctor<IValidatedContext>().Named("ASIPublicationContext");

            For<IRepository<CounselorContent>>().Use<EFRepository<CounselorContent>>()
                .Ctor<IValidatedContext>().Named("ASIPublicationContext");

            For<IRepository<CounselorFeature>>().Use<EFRepository<CounselorFeature>>()
                .Ctor<IValidatedContext>().Named("ASIPublicationContext");

            #endregion ASIPublicationContext

            #region InternetContext

            For<IRepository<News>>().Use<EFRepository<News>>()
                .Ctor<IValidatedContext>().Named("InternetContext");

            For<IRepository<NewsRotator>>().Use<EFRepository<NewsRotator>>()
                .Ctor<IValidatedContext>().Named("InternetContext");

            For<IRepository<NewsSource>>().Use<EFRepository<NewsSource>>()
                .Ctor<IValidatedContext>().Named("InternetContext");

            #endregion InternetContext

            #region ProductContext

            For<IRepository<Context>>().Use<EFRepository<Context>>()
                .Ctor<IValidatedContext>().Named("ProductContext");

            #endregion ProductContext
        }
    }
}
