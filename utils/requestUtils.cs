using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ComponentModel;

namespace OpenEFI_RemoteBuild.Utils
{
    public static class RequestUtils
    {

        public static bool IsAnyNullOrEmpty(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return true;

            return obj.GetType().GetProperties()
                ?.Any(x => IsNullOrEmpty(x.GetValue(obj)?.ToString() ?? "")) ?? true;
        }
        private static bool IsNullOrEmpty(string s)
        {
            bool result;
            result = s == null || s == string.Empty;
            return result;
        }
    }

    public class IBuildStatus
    {
        public string ID { get; set; }
        public string Status { get; set; }
        public string Build_info { get; set; }
    }
}
