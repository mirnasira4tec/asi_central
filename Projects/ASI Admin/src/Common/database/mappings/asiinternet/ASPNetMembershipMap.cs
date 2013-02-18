﻿using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class ASPNetMembershipMap : EntityTypeConfiguration<ASPNetMembership>
    {
        public ASPNetMembershipMap()
        {
            ToTable("aspnet_Membership");
            HasKey(memb => memb.UserId);
        }
    }
}
