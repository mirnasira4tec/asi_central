using System;
using System.Collections.Generic;

namespace Publications.Models
{
    public partial class COUN_Content_CSCO
    {
        public COUN_Content_CSCO()
        {
            this.COUN_Categories_CSCT = new List<COUN_Categories_CSCT>();
            this.COUN_FeatureContent_COFC = new List<COUN_FeatureContent_COFC>();
            //this.COUN_FeatureContentRotator_CFCR = new List<COUN_FeatureContentRotator_CFCR>();
            this.COUN_Categories_CSCT1 = new List<COUN_Categories_CSCT>();
        }

        public int ID_CSCO { get; set; }
        public string Title_CSCO { get; set; }
        public Nullable<System.DateTime> Date_CSCO { get; set; }
        public string Author_CSCO { get; set; }
        public string TagLine_CSCO { get; set; }
        public string Teaser_CSCO { get; set; }
        public string Content_CSCO { get; set; }
        public Nullable<bool> Active_CSCO { get; set; }
        public string Img_Sm_CSCO { get; set; }
        public string Img_Lg_CSCO { get; set; }
        //public virtual COUN_AwardsCatContent_CACC COUN_AwardsCatContent_CACC { get; set; }
        public virtual ICollection<COUN_Categories_CSCT> COUN_Categories_CSCT { get; set; }
        public virtual ICollection<COUN_FeatureContent_COFC> COUN_FeatureContent_COFC { get; set; }
        //public virtual ICollection<COUN_FeatureContentRotator_CFCR> COUN_FeatureContentRotator_CFCR { get; set; }
        public virtual ICollection<COUN_Categories_CSCT> COUN_Categories_CSCT1 { get; set; }
    }
}
