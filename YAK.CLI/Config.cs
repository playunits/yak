using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using AnhangExporter.Core.Serialization;
using System.Reflection;

namespace YAK.CLI
{

    public class Config
    {
        public List<DomainStructure> Domains { get; set; } = new List<DomainStructure>();

        public Config()
        {
            if (!File.Exists(GlobalConfig.ConfigFile))
            {
                File.Create(GlobalConfig.ConfigFile);
            }

            this.Load();
        }

        private void Load()
        {
            var result = JSONWriter.Read<Config>(GlobalConfig.ConfigFile);

            if (!result.success)
            {
                return;
            }

            foreach (PropertyInfo info in result.element.GetType().GetProperties())
            {
                info.SetValue(this, info.GetValue(result.element    ));
            }
        }
    }

    public class DomainStructure
    {
        public List<string> Subdomains { get; set; } = new List<string>();
        public string Domain { get; set; } = "";
    }
}
