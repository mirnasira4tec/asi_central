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

        public MagazineType MagazineId { get; set; }

        public DateTime Issue { get; set; }

        public bool IsChecked { get; set; }

    }
}
