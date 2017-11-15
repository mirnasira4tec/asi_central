using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class ContextProductSequence
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Qualifier { get; set; }
        public int PageNumber { get; set; }
        public decimal Cost { get; set; }
        public decimal ApplicationCost { get; set; }
        public virtual ContextProduct Product { get; set; }
        public bool IsBestProduct { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
