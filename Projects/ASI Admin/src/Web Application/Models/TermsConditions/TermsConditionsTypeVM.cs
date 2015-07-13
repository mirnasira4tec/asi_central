using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.TermsConditions
{
    public class TermsConditionsTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public static class TermsConditionsTypeExtensions
    {
        public static TermsConditionsTypeVM ToViewModel(this TermsConditionsType model)
        {
            TermsConditionsTypeVM viewModel = null;
            if (model != null)
            {
                viewModel = new TermsConditionsTypeVM()
                 {
                     Id = model.Id,
                     Name = model.Name,
                     Header = model.Header,
                     Body = model.Body,
                     IsActive = model.IsActive,
                     StartDate = model.StartDate,
                     EndDate = model.EndDate,
                     CreateDate = model.CreateDate,
                     UpdateDate = model.UpdateDate,
                     UpdateSource = model.UpdateSource
                 };
            }
            else
                viewModel = new TermsConditionsTypeVM();

            return viewModel;
        }

        public static TermsConditionsType ToDataModel(this TermsConditionsTypeVM viewModel)
        {
            TermsConditionsType dataModel = null;
            if (viewModel != null)
            {
                dataModel = new TermsConditionsType()
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Header = viewModel.Header,
                    Body = viewModel.Body,
                    IsActive = viewModel.IsActive,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    CreateDate = viewModel.Id == 0 ? DateTime.Now : viewModel.CreateDate,
                    UpdateDate = DateTime.Now,
                    UpdateSource = viewModel.UpdateSource
                };
            }
            else
                dataModel = new TermsConditionsType();

            return dataModel;
        }
    }
}