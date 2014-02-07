﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookMagazineIssue
    {
        public int Id { get; set; }
        public MagazineType ProductId { get; set; }
        public DateTime Issue { get; set; }
        public DateTime ReservationDeadline { get; set; }
        public DateTime MaterialDeadline { get; set; }
        public DateTime MailingDate { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
