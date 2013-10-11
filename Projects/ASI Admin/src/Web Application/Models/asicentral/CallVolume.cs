using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.asicentral
{
    public class CallVolume
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public IList<Volume> Data { get; set; }
    }

    public class Volume
    {
        public int QueueIdentifier { get; set; }
        public string QueueName { get; set; }
        public int Amount { get; set; }
    }
}