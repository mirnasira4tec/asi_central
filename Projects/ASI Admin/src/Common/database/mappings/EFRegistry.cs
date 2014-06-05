using asi.asicentral.model.sgr;
using asi.asicentral.model.counselor;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.model.store;
using asi.asicentral.model.news;
using asi.asicentral.model.timss;
using asi.asicentral.model.call;

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
            SelectConstructor<ASIInternetContext>(() => new ASIInternetContext());
            SelectConstructor<StoreContext>(() => new StoreContext());
            SelectConstructor<ASIEmailBlastContext>(() => new ASIEmailBlastContext());
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIInternetContext>().Name = "ASIInternetContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIPublicationContext>().Name = "ASIPublicationContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<InternetContext>().Name = "InternetContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<StoreContext>().Name = "StoreContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<TIMSSContext>().Name = "TIMSSContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<CallContext>().Name = "CallContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIEmailBlastContext>().Name = "ASIEmailBlastContext";


            //for each model - get the repository class with the appropriate context 

            #region ASIInternetContext

            For<IRepository<ASPNetMembership>>().Use<EFRepository<ASPNetMembership>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<ASPNetUser>>().Use<EFRepository<ASPNetUser>>()
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

            For<IRepository<LegacyOrderAddress>>().Use<EFRepository<LegacyOrderAddress>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderCatalog>>().Use<EFRepository<LegacyOrderCatalog>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderCatalogOption>>().Use<EFRepository<LegacyOrderCatalogOption>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderContact>>().Use<EFRepository<LegacyOrderContact>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderCreditCard>>().Use<EFRepository<LegacyOrderCreditCard>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderMagazineAddress>>().Use<EFRepository<LegacyOrderMagazineAddress>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyMagazineAddress>>().Use<EFRepository<LegacyMagazineAddress>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<LegacyOrderDistributorAddress>>().Use<EFRepository<LegacyOrderDistributorAddress>>()
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

            For<IRepository<CENTUserProfilesPROF>>().Use<EFRepository<CENTUserProfilesPROF>>()
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

            For<IRepository<Coupon>>().Use<EFRepository<Coupon>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailHallmarkRequest>>().Use<EFRepository<StoreDetailHallmarkRequest>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookCatalogOption>>().Use<EFRepository<LookCatalogOption>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookDecoratorImprintingType>>().Use<EFRepository<LookDecoratorImprintingType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookDistributorAccountType>>().Use<EFRepository<LookDistributorAccountType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookDistributorRevenueType>>().Use<EFRepository<LookDistributorRevenueType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookEventMerchandiseProduct>>().Use<EFRepository<LookEventMerchandiseProduct>>()
                .Ctor<IValidatedContext>().Named("StoreContext"); 

            For<IRepository<LookProductCollections>>().Use<EFRepository<LookProductCollections>>()
              .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookProductLine>>().Use<EFRepository<LookProductLine>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookProductShippingRate>>().Use<EFRepository<LookProductShippingRate>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookSupplierDecoratingType>>().Use<EFRepository<LookSupplierDecoratingType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreAddress>>().Use<EFRepository<StoreAddress>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreCompany>>().Use<EFRepository<StoreCompany>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreCompanyAddress>>().Use<EFRepository<StoreCompanyAddress>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreCreditCard>>().Use<EFRepository<StoreCreditCard>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailCatalog>>().Use<EFRepository<StoreDetailCatalog>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailDecoratorMembership>>().Use<EFRepository<StoreDetailDecoratorMembership>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailDistributorMembership>>().Use<EFRepository<StoreDetailDistributorMembership>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailEspTowerAd>>().Use<EFRepository<StoreDetailEspTowerAd>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailSupplierMembership>>().Use<EFRepository<StoreDetailSupplierMembership>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreIndividual>>().Use<EFRepository<StoreIndividual>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreMagazineSubscription>>().Use<EFRepository<StoreMagazineSubscription>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreOrder>>().Use<EFRepository<StoreOrder>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreOrderDetail>>().Use<EFRepository<StoreOrderDetail>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailESPAdvertising>>().Use<EFRepository<StoreDetailESPAdvertising>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailESPAdvertisingItem>>().Use<EFRepository<StoreDetailESPAdvertisingItem>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailPayForPlacement>>().Use<EFRepository<StoreDetailPayForPlacement>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreTieredProductPricing>>().Use<EFRepository<StoreTieredProductPricing>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<TaxRate>>().Use<EFRepository<TaxRate>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailEmailExpress>>().Use<EFRepository<StoreDetailEmailExpress>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailEmailExpressItem>>().Use<EFRepository<StoreDetailEmailExpressItem>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailProductCollection>>().Use<EFRepository<StoreDetailProductCollection>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailProductCollectionItem>>().Use<EFRepository<StoreDetailProductCollectionItem>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailMagazineAdvertisingItem>>().Use<EFRepository<StoreDetailMagazineAdvertisingItem>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookMagazineIssue>>().Use<EFRepository<LookMagazineIssue>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookAdPosition>>().Use<EFRepository<LookAdPosition>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookAdSize>>().Use<EFRepository<LookAdSize>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookSendMyAdPublication>>().Use<EFRepository<LookSendMyAdPublication>>()
           .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookSendMyAdAdSpec>>().Use<EFRepository<LookSendMyAdAdSpec>>()
               .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<LookSendMyAdCountryCode>>().Use<EFRepository<LookSendMyAdCountryCode>>()
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

            For<IRepository<PersonifyMapping>>().Use<EFRepository<PersonifyMapping>>()
                .Ctor<IValidatedContext>().Named("TIMSSContext");

            #endregion TIMSS Context

            #region Call Context

            For<IRepository<CallQueue>>().Use<EFRepository<CallQueue>>()
                .Ctor<IValidatedContext>().Named("CallContext");

            For<IRepository<CallRequest>>().Use<EFRepository<CallRequest>>()
                .Ctor<IValidatedContext>().Named("CallContext");

            #endregion

            #region ASIEmailBlastContext

            For<IRepository<ClosedCampaignDate>>().Use<EFRepository<ClosedCampaignDate>>()
                .Ctor<IValidatedContext>().Named("ASIEmailBlastContext");

            #endregion ASIEmailBlastContext
        }
    }
}
