using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.util.store
{
    public static class Helper
    {
        public static readonly int[] SPECIAL_CONTEXT_IDS = { 21, 10016, 24, 11012 };

        public static IList<SelectListItem> GetTitles()
        {
            IList<SelectListItem> titles = new List<SelectListItem>();
            titles.Add(new SelectListItem() { Text = "- Select a title -", Value = "" });
            titles.Add(new SelectListItem() { Text = "Accountant", Value = "Accounting" });
            titles.Add(new SelectListItem() { Text = "Administrative Support", Value = "Administration" });
            titles.Add(new SelectListItem() { Text = "Copywriter", Value = "Editorial" });
            titles.Add(new SelectListItem() { Text = "Designer", Value = "Art" });
            titles.Add(new SelectListItem() { Text = "Engineer", Value = "Engineering" });
            titles.Add(new SelectListItem() { Text = "Event Planner", Value = "Event Planner" });
            titles.Add(new SelectListItem() { Text = "Executive", Value = "Executive/Corporate Mgmt" });
            titles.Add(new SelectListItem() { Text = "Executive Officer", Value = "Founder" });
            titles.Add(new SelectListItem() { Text = "Finance Staff", Value = "Purchasing/Procurement" });
            titles.Add(new SelectListItem() { Text = "HR Coordinator", Value = "Human Resources" });
            titles.Add(new SelectListItem() { Text = "IT / IS Staff", Value = "IT / IS Staff" });
            titles.Add(new SelectListItem() { Text = "Manager", Value = "General Management" });
            titles.Add(new SelectListItem() { Text = "Other", Value = "Not Defined" });
            titles.Add(new SelectListItem() { Text = "Owner", Value = "Owner" });
            titles.Add(new SelectListItem() { Text = "Project Manager", Value = "Prod Dev / Mgt" });
            titles.Add(new SelectListItem() { Text = "PR/Marketing Staff", Value = "Communications" });
            titles.Add(new SelectListItem() { Text = "Retailer", Value = "Retailer" });
            titles.Add(new SelectListItem() { Text = "Sales Staff", Value = "Sales" });
            titles.Add(new SelectListItem() { Text = "Support Specialist", Value = "Customer Svc" });
            titles.Add(new SelectListItem() { Text = "Trainer", Value = "Education" });
            titles.Add(new SelectListItem() { Text = "3rd Party Consultant", Value = "Consulting" });
            return titles;
        }
    }
}
