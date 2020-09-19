using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace OpenEFI_RemoteBuild.TemplateEngine
{
    public static class Templates
    {
        public static void CreateDefineFile(string path, BuildRequest data)
        {
            string merca = File.ReadAllText("./utils/defines.h");
            var template = new StringFormatter(merca);
            template.Add("{{OPENEFI_MAJOR}}", data.OPENEFI_VER_MAJOR ?? "2");
            template.Add("{{OPENEFI_MINOR}}", data.OPENEFI_VER_MINOR ?? "0");
            template.Add("{{OPENEFI_REV}}", data.OPENEFI_VER_REV ?? "3");
            template.Add("{{MTR}}",     data.MTR);
            template.Add("{{AVCI}}",    data.AVCI);
            template.Add("{{ECNT}}",    data.ECNT);
            template.Add("{{RPM_PER}}", data.RPM_PER);
            template.Add("{{DNT}}",     data.DNT);
            template.Add("{{Alpha}}",   data.Alpha);
            template.Add("{{ED}}",      data.ED);
            template.Add("{{PMSI}}",    data.PMSI);

            File.WriteAllText($"./files/{path}/include/defines.h", template.ToString());
        }
    }

    public class StringFormatter
    {

        public string Str { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        public StringFormatter(string instr)
        {
            Str = instr;
            Parameters = new Dictionary<string, object>();
        }

        public void Add(string key, object val)
        {
            Parameters.Add(key, val);
        }

        public override string ToString()
        {
            return Parameters.Aggregate(Str, (current, parameter) => current.Replace(parameter.Key, parameter.Value.ToString()));
        }

    }

}