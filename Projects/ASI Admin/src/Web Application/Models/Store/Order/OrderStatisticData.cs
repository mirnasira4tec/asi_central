using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store.order
{
    public class OrderStatisticData
    {
        public const String Statistics_Campaign = "Campaign";
        public const String Statistics_Product = "Product";
        public const String Statistics_Coupon = "Coupon";

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Campaign { get; set; }
        public string Product { get; set; }
        public string Coupon { get; set; }
        public string FormTab { get; set; }
        public string Name { get; set; }
        public IList<Group> Data { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { set; get; }

    }
    public class GroupedData
    {
        public string GroupName { get; set; }
        public string StepLabel { get; set; }
        public int CompletedStep { get; set; }
        public int Count { get; set; }
        public decimal? Amount { get; set; }

        public int CountRejected { get; set; }
        public decimal? AmountRejected { set; get; }
        public int CountApproved { get; set; }
        public decimal? AmountApproved { set; get; }
    }
    
    public class Group
    {
        public Group()
        {
            Data = new GroupData[8];
            Data[0] = new GroupData() { Name = "Clicked on the link Only", Amount = 0, Count = 0 };
            Data[1] = new GroupData() { Name = "Selected a product", Amount = 0, Count = 0 };
            Data[2] = new GroupData() { Name = "Entered Company information", Amount = 0, Count = 0 };
            Data[3] = new GroupData() { Name = "Entered billing/shipping information", Amount = 0, Count = 0 };
            Data[4] = new GroupData() { Name = "Confirmed the order", Amount = 0, Count = 0 };
            Data[5] = new GroupData() { Name = "Rejected", Amount = 0, Count = 0 };
            Data[6] = new GroupData() { Name = "Pending Approval", Amount = 0, Count = 0 };
            Data[7] = new GroupData() { Name = "Total", Amount = 0, Count = 0 };
        }
        public string Name { get; set; }
        public GroupData[] Data { get; set; }
    }
    public class GroupData
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public decimal AnnualizedAmount { get; set; }
    }
    
}