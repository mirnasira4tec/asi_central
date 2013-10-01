using asi.asicentral.model.ROI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface IROIService
    {
        Category[] GetImpressionsPerCategory(int asiNumber);
    }
}
