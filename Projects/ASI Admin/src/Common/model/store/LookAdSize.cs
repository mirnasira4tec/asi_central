using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookAdSize
    {
        public int Id { get; set; }
        public MagazineType MagazineId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
