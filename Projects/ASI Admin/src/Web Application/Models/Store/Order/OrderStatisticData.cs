using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store.order
{
    public class OrderStatisticData
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Campaign { get; set; }
        public IList<GroupedData> Data { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class GroupedData
    {
        public string Campaign { get; set; }
        public string StepLabel { get; set; }
        public int CompletedStep { get; set; }
        public int Count { get; set; }
        public decimal? Amount { get; set; }
    }
}