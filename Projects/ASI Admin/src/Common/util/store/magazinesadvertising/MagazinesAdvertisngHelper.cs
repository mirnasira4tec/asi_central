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
    public class MagazinesAdvertisngHelper
    {
        //Ad size
        public readonly string[] MAGAZINESADVERTISNG_ADSIZE = { "Full Page - 8” x 9 1/2”", "2/3 Vertical - 4 3/4” x 9 7/9”", "1/2 Horizontal - 8” x 4 1/2”", "1/2 Island - 5 1/4” x 4 3/4”", "1/3 Vertical - 2 1/2” x 9 7/8”", "1/3 Square - 5 1/4” x 4 3/4”", "1/4 Vertical - 2” x 9 7/8”", "Classified 4“ - 2 3/8” x 4“", "Classified 3“ - 2 3/8” x 3“", "Classified 2“ - 2 3/8” x 2“" };

        //Position
        public readonly string[] MAGAZINESADVERTISNG_POSITION = { "Best Possible", "Upfront", "Far Forward", "Left Hand", "Right Hand" };

        //Art Work
        public readonly string[] MAGAZINESADVERTISNG_ARTWORK = { "Upload Now", "Upload Later (Save 15%)", "Far Forward", "Left Hand", "Right Hand" };

        
    }
}
