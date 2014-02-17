using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface IDateUTCAndSource
    {

        DateTime CreateDateUTC { get; set; }

        DateTime UpdateDateUTC { get; set; }

        string UpdateSource { get; set; }
    }
}
