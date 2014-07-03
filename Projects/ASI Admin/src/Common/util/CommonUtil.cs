using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util
{
   public class CommonUtil
    {
       public static Guid GuidCreator(string id)
       {
           StringBuilder a = new StringBuilder();
           int rem = id.Length - 32;
           for (int i = 0; i < Math.Abs(rem); i++)
           {
               a.Append("0");
           }
           a.Append(id);
           Guid extRef = new Guid(a.ToString());
           return extRef;
       }
    }
}
