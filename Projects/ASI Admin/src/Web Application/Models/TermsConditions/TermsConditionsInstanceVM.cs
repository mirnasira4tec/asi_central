using asi.asicentral.model.store;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.web.Models.TermsConditions
{
    public class TermsConditionsInstanceVM
    {
        public int Id { get; set; }
        public string GUID { get; set; }
        [Required]
        [Display(Name = "Customer Email")]
        [StringLength(200, ErrorMessage = "Max email length is 200 characters")]
        public string CustomerEmail { get; set; }
        [Required]
        [Display(Name = "Customer Name")]
        [StringLength(200, ErrorMessage = "Max name length is 200 characters")]
        public string CustomerName { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        [StringLength(200, ErrorMessage = "Max name length is 200 characters")]
        public string CompanyName { get; set; }
        [Display(Name = "Select Terms and Conditions")]
        public string TermsConditionsName { get; set; }
        public int TypeId { get; set; }
        public string IPAddress { get; set; }
        public int? OrderId { get; set; }
        [Display(Name = "Acceptance Date")]
        public DateTime? DateAgreedOn { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Internal Notification Emails")]
        public string NotificationEmail { get; set; }
        public string Messages { get; set; }
        [Display(Name = "Accepted By")]
        public string AcceptedBy { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public List<string> TypesWithSameGuid { get; set; }
        public List<TermsConditionsType> TermList { get; set; }
    }

    public static class TermsConditionsInstanceExtensions
    {
        public static TermsConditionsInstanceVM ToViewModel(this TermsConditionsInstance model)
        {
            TermsConditionsInstanceVM viewModel = null;
            if (model != null)
            {
                viewModel = new TermsConditionsInstanceVM()
                 {
                     Id = model.Id,
                     GUID = model.GUID,
                     CustomerEmail = model.CustomerEmail,
                     CustomerName = model.CustomerName,
                     CompanyName = model.CompanyName,
                     TypeId = model.TypeId,
                     TermsConditionsName = model.TermsAndConditions != null ? model.TermsAndConditions.Name : string.Empty,
                     IPAddress = model.IPAddress,
                     OrderId = model.OrderId,
                     DateAgreedOn = model.DateAgreedOn,
                     CreatedBy = model.CreatedBy,
                     LastUpdatedBy = model.LastUpdatedBy,
                     NotificationEmail = model.NotificationEmail,
                     Messages = model.Messages,
                     AcceptedBy = model.AcceptedBy,
                     CreateDate = model.CreateDate,
                     UpdateDate = model.UpdateDate,
                     UpdateSource = model.UpdateSource
                 };

                viewModel.TypesWithSameGuid = new List<string>();
                if (model.TermsAndConditions != null)
                    viewModel.TypesWithSameGuid.Add(model.TermsAndConditions.Name);
            }
            else
                viewModel = new TermsConditionsInstanceVM();

            return viewModel;
        }

        public static TermsConditionsInstance ToDataModel(this TermsConditionsInstanceVM viewModel)
        {
            TermsConditionsInstance dataModel = null;
            if (viewModel != null)
            {
                dataModel = new TermsConditionsInstance()
                {
                    Id = viewModel.Id,
                    GUID = viewModel.GUID,
                    CustomerEmail = viewModel.CustomerEmail,
                    CustomerName = viewModel.CustomerName,
                    CompanyName = viewModel.CompanyName,
                    TypeId = viewModel.TypeId,
                    IPAddress = viewModel.IPAddress,
                    OrderId = viewModel.OrderId,
                    DateAgreedOn = viewModel.DateAgreedOn,
                    CreatedBy = viewModel.CreatedBy,
                    LastUpdatedBy = viewModel.LastUpdatedBy,
                    NotificationEmail = viewModel.NotificationEmail,
                    Messages = viewModel.Messages,
                    AcceptedBy = viewModel.AcceptedBy,
                    CreateDate = viewModel.Id == 0 ? DateTime.Now : viewModel.CreateDate,
                    UpdateDate = DateTime.Now,
                    UpdateSource = viewModel.UpdateSource
                };
            }
            else
                dataModel = new TermsConditionsInstance();

            return dataModel;
        }
    }
}