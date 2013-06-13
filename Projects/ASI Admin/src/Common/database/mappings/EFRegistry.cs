﻿using asi.asicentral.model.sgr;
using asi.asicentral.model.counselor;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.model.store;
using asi.asicentral.model.news;
using asi.asicentral.model.timss;

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
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<StoreContext>().Name = "StoreContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<TIMSSContext>().Name = "TIMSSContext";

            //for each model - get the repository class with the appropriate context 

            #region ASIInternetContext

            For<IRepository<ASPNetMembership>>().Use<EFRepository<ASPNetMembership>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<Category>>().Use<EFRepository<Category>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<Company>>().Use<EFRepository<Company>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyDistributorAccountType>>().Use<EFRepository<LegacyDistributorAccountType>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyDistributorBusinessRevenue>>().Use<EFRepository<LegacyDistributorBusinessRevenue>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyDistributorMembershipApplication>>().Use<EFRepository<LegacyDistributorMembershipApplication>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyDistributorProductLine>>().Use<EFRepository<LegacyDistributorProductLine>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrder>>().Use<EFRepository<LegacyOrder>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderCreditCard>>().Use<EFRepository<LegacyOrderCreditCard>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderDetail>>().Use<EFRepository<LegacyOrderDetail>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderProduct>>().Use<EFRepository<LegacyOrderProduct>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<Product>>().Use<EFRepository<Product>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacySupplierDecoratingType>>().Use<EFRepository<LegacySupplierDecoratingType>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacySupplierMembershipApplication>>().Use<EFRepository<LegacySupplierMembershipApplication>>()
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

            #region StoreContext

            For<IRepository<Context>>().Use<EFRepository<Context>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<ContextProduct>>().Use<EFRepository<ContextProduct>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<ContextFeature>>().Use<EFRepository<ContextFeature>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<ContextFeatureProduct>>().Use<EFRepository<ContextFeatureProduct>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<ContextProductSequence>>().Use<EFRepository<ContextProductSequence>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookDistributorAccountType>>().Use<EFRepository<LookDistributorAccountType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookDistributorRevenueType>>().Use<EFRepository<LookDistributorRevenueType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");
            
            For<IRepository<LookProductLine>>().Use<EFRepository<LookProductLine>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreAddress>>().Use<EFRepository<StoreAddress>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreCompany>>().Use<EFRepository<StoreCompany>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreCompanyAddress>>().Use<EFRepository<StoreCompanyAddress>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreCreditCard>>().Use<EFRepository<StoreCreditCard>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailDistributorMembership>>().Use<EFRepository<StoreDetailDistributorMembership>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreIndividual>>().Use<EFRepository<StoreIndividual>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreOrder>>().Use<EFRepository<StoreOrder>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreOrderDetail>>().Use<EFRepository<StoreOrderDetail>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<TaxRate>>().Use<EFRepository<TaxRate>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            #endregion StoreContext

            #region TIMSS Context

            For<IRepository<TIMSSAccountType>>().Use<EFRepository<TIMSSAccountType>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            For<IRepository<TIMSSAdditionalInfo>>().Use<EFRepository<TIMSSAdditionalInfo>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            For<IRepository<TIMSSCompany>>().Use<EFRepository<TIMSSCompany>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            For<IRepository<TIMSSContact>>().Use<EFRepository<TIMSSContact>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            For<IRepository<TIMSSCreditInfo>>().Use<EFRepository<TIMSSCreditInfo>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            For<IRepository<TIMSSProductType>>().Use<EFRepository<TIMSSProductType>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            #endregion TIMSS Context
        }
    }
}
