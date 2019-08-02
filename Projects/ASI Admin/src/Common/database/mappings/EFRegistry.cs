using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.model.call;
using asi.asicentral.model.DM_memberDemogr;
using asi.asicentral.model.excit;
using asi.asicentral.model.findsupplier;
using asi.asicentral.model.news;
using asi.asicentral.model.personify;
using asi.asicentral.model.sgr;
using asi.asicentral.model.show;
using asi.asicentral.model.store;
using StructureMap.Configuration.DSL;

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
            SelectConstructor<DM_MemberDemogrContext>(() => new DM_MemberDemogrContext());
            SelectConstructor<Umbraco_ShowContext>(() => new Umbraco_ShowContext());
            SelectConstructor<PersonifyContext>(() => new PersonifyContext());
            SelectConstructor<AsicentralContext>(() => new AsicentralContext());
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIInternetContext>().Name = "ASIInternetContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<InternetContext>().Name = "InternetContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<StoreContext>().Name = "StoreContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<CallContext>().Name = "CallContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<ASIEmailBlastContext>().Name = "ASIEmailBlastContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<MemberDemogrContext>().Name = "MemberDemogrContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<DM_MemberDemogrContext>().Name = "DM_MemberDemogrContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<Umbraco_ShowContext>().Name = "Umbraco_ShowContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<PersonifyContext>().Name = "PersonifyContext";
            For<IValidatedContext>().HybridHttpOrThreadLocalScoped().Use<AsicentralContext>().Name = "umbracoDbDSN";

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

            For<IRepository<SupUpdateField>>().Use<EFRepository<SupUpdateField>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<SupUpdateRequest>>().Use<EFRepository<SupUpdateRequest>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            For<IRepository<SupUpdateRequestDetail>>().Use<EFRepository<SupUpdateRequestDetail>>()
                .Ctor<IValidatedContext>().Named("ASIInternetContext");

            #endregion ASIInternetContext            

            #region InternetContext

            For<IRepository<News>>().Use<EFRepository<News>>()
                .Ctor<IValidatedContext>().Named("InternetContext");

            For<IRepository<NewsRotator>>().Use<EFRepository<NewsRotator>>()
                .Ctor<IValidatedContext>().Named("InternetContext");

            For<IRepository<NewsSource>>().Use<EFRepository<NewsSource>>()
                .Ctor<IValidatedContext>().Named("InternetContext");

            #endregion InternetContext

            #region MemberDemogrContext

            For<IRepository<SupplierPolicy>>().Use<EFRepository<SupplierPolicy>>()
                .Ctor<IValidatedContext>().Named("MemberDemogrContext");

            For<IRepository<SupplierPhone>>().Use<EFRepository<SupplierPhone>>()
                .Ctor<IValidatedContext>().Named("MemberDemogrContext");

            For<IRepository<SupplierSeadElectronicAddress>>().Use<EFRepository<SupplierSeadElectronicAddress>>()
                .Ctor<IValidatedContext>().Named("MemberDemogrContext");

            For<IRepository<SupplierRating>>().Use<EFRepository<SupplierRating>>()
                .Ctor<IValidatedContext>().Named("MemberDemogrContext");

            #endregion MemberDemogrContext

            #region StoreContext
            For<IRepository<CompanyValidation>>().Use<EFRepository<CompanyValidation>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

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

            For<IRepository<FormType>>().Use<EFRepository<FormType>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<FormInstance>>().Use<EFRepository<FormInstance>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<FormValue>>().Use<EFRepository<FormValue>>()
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

            For<IRepository<LookEquipmentType>>().Use<EFRepository<LookEquipmentType>>()
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

            For<IRepository<StoreDetailEquipmentMembership>>().Use<EFRepository<StoreDetailEquipmentMembership>>()
                .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<StoreDetailSpecialProductItem>>().Use<EFRepository<StoreDetailSpecialProductItem>>()
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

            For<IRepository<StoreSupplierRepresentativeInformation>>().Use<EFRepository<StoreSupplierRepresentativeInformation>>()
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

            For<IRepository<StoreDetailCatalogAdvertisingItem>>().Use<EFRepository<StoreDetailCatalogAdvertisingItem>>()
              .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<TermsConditionsType>>().Use<EFRepository<TermsConditionsType>>()
             .Ctor<IValidatedContext>().Named("StoreContext");

            For<IRepository<TermsConditionsInstance>>().Use<EFRepository<TermsConditionsInstance>>()
             .Ctor<IValidatedContext>().Named("StoreContext");

            #endregion StoreContext

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

            #region DM_MemberDemogrContext
            For<IRepository<CompanyASIRep>>().Use<EFRepository<CompanyASIRep>>()
                .Ctor<IValidatedContext>().Named("DM_MemberDemogrContext");
            #endregion DM_MemberDemogrContext

            #region Umbraco_ShowContext

            For<IRepository<ShowAddress>>().Use<EFRepository<ShowAddress>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowAttendee>>().Use<EFRepository<ShowAttendee>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowCompany>>().Use<EFRepository<ShowCompany>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowEmployee>>().Use<EFRepository<ShowEmployee>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowEmployeeAttendee>>().Use<EFRepository<ShowEmployeeAttendee>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowASI>>().Use<EFRepository<ShowASI>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowType>>().Use<EFRepository<ShowType>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowCompanyAddress>>().Use<EFRepository<ShowCompanyAddress>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowDistShowLogo>>().Use<EFRepository<ShowDistShowLogo>>()
                .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowProfileSupplierData>>().Use<EFRepository<ShowProfileSupplierData>>()
               .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowProfileOptionalDataLabel>>().Use<EFRepository<ShowProfileOptionalDataLabel>>()
              .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowProfileRequests>>().Use<EFRepository<ShowProfileRequests>>()
              .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowProfileOptionalDetails>>().Use<EFRepository<ShowProfileOptionalDetails>>()
              .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            For<IRepository<ShowProfileDistributorData>>().Use<EFRepository<ShowProfileDistributorData>>()
              .Ctor<IValidatedContext>().Named("Umbraco_ShowContext");

            #endregion Umbraco_ShowContext

            #region PersonifyContext
            For<IRepository<PersonifyMapping>>().Use<EFRepository<PersonifyMapping>>()
                .Ctor<IValidatedContext>().Named("PersonifyContext");
            #endregion

            #region AsiCentralContext
            For<IRepository<RateSupplierForm>>().Use<EFRepository<RateSupplierForm>>()
              .Ctor<IValidatedContext>().Named("umbracoDbDSN");
            For<IRepository<RateSupplierFormDetail>>().Use<EFRepository<RateSupplierFormDetail>>()
                    .Ctor<IValidatedContext>().Named("umbracoDbDSN");
            For<IRepository<RateSupplierImport>>().Use<EFRepository<RateSupplierImport>>()
                    .Ctor<IValidatedContext>().Named("umbracoDbDSN");
            #endregion
        }
    }
}
