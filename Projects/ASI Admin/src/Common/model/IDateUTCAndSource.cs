using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public interface IDateUTCAndSource
    {

        DateTime CreateDate { get; set; }

        DateTime UpdateDate { get; set; }

        string UpdateSource { get; set; }
    }
}
