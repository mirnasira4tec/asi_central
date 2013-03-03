﻿using asi.asicentral.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class OrderDetailApplication
    {
        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        public string Company { get; set; }

        public override string ToString()
        {
            return string.Format( this.GetType().Name + ": {0}", Id);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            OrderDetailApplication orderApplication = obj as OrderDetailApplication;
            if (orderApplication != null) equals = orderApplication.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}