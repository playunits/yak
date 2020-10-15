using System;
using System.Collections.Generic;
using System.Text;

namespace YAK.CLI
{
    public static class GlobalConfig
    {
        public static string CmdName { get; set; } = "yak";
        public static string ConfigFile { get; set; } = "settings.json";
        public static Config Config { get; set; } = new Config();
    }
}
