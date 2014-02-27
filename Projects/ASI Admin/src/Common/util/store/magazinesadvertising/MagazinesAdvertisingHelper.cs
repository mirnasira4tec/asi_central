using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asi.asicentral.interfaces;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class MagazinesAdvertisingHelper
    {
        //Art Work
        public readonly string[] MAGAZINESADVERTISING_ARTWORK = { "Supply Your Ad (Save 15%)", "Have us create your Ad" };
        //Color
        public readonly string[] MAGAZINESADVERTISING_COLOR = { "4-Color", "Black & White" };

        public decimal Discount = 0.15M;
    }
}
