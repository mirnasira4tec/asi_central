using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public partial class LegacyMagazineAddress
    {
        public LegacyMagazineAddress()
        {
        }

        public int MAGA_SubscribeID { get; set; }
        public string MAGA_ASINo { get; set; }
        public string MAGA_FName { get; set; }
        public string MAGA_LName { get; set; }
        public string MAGA_Title { get; set; }
        public string MAGA_Company { get; set; }
        public string MAGA_Street1 { get; set; }
        public string MAGA_Street2 { get; set; }
        public string MAGA_City { get; set; }
        public string MAGA_Zip { get; set; }
        public string MAGA_State { get; set; }
        public string MAGA_Country { get; set; }
        public string MAGA_Phone { get; set; }
        public string MAGA_Fax { get; set; }
        public string MAGA_Email { get; set; }
        public Nullable<bool> MAGA_Digital { get; set; }
    }
}
