using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class Context
    {
        public Context()
        {
            if (this.GetType() == typeof(Context))
            {
                Features = new List<ContextFeature>();
                Products = new List<ContextProductSequence>();
            }
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string NotificationEmails { get; set; }
        public int NumberOfPages { get; set; }
        public string HeaderImage { get; set; }
        public string ChatSettings { get; set; }
        public virtual List<ContextFeature> Features { get; set; }
        public virtual List<ContextProductSequence> Products { get; set; }

        public override string ToString()
        {
            return string.Format("Context: {0} - {1}", Id, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Context context = obj as Context;
            if (context != null) equals = context.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
