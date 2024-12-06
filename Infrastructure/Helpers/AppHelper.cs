using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Constants;

namespace Shared.Helpers
{
    public class AppHelper
    {
        public static bool IsProduction(string environment)
        {
            return environment == AppConstants.Production;
        }
    }
}
