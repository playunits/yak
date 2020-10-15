using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace YAK.CLI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CliContainerAttribute : Attribute
    {
        public string Name { get; set; }
        public CliContainerAttribute(string name)
        {
            this.Name = name;
        }
    }
}
