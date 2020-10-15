using System;
using System.Collections.Generic;
using System.Text;

namespace YAK.CLI.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CliCommandAttribute : Attribute
    {
        public string Name { get; set; }        

        public CliCommandAttribute(string name)
        {
            this.Name = name;
        }
    }
       
}
