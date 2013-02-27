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

        public int ContextId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string NotificationEmails { get; set; }
        public virtual List<ContextFeature> Features { get; set; }
        public virtual List<ContextProductSequence> Products { get; set; }

        public override string ToString()
        {
            return string.Format("Context: {0} - {1}", ContextId, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Context context = obj as Context;
            if (context != null) equals = context.ContextId == ContextId;
            return equals;
        }

        public override int GetHashCode()
        {
            return ContextId.GetHashCode();
        }
    }
}
