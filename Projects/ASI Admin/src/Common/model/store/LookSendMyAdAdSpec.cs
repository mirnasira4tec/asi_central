﻿using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookSendMyAdAdSpec : IDateUTCAndSource
    {

        public int Id { get; set; }

        public virtual LookAdSize Size { get; set; }
        
        public int AdSpecId { get; set; }

        public DateTime CreateDateUTC { get; set; }

        public DateTime UpdateDateUTC { get; set; }

        public string UpdateSource { get; set; }
    }
}
