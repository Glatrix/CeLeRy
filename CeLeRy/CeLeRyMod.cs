using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeLeRy
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CeLeRyMod : Attribute
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string EntryMethodName { get; set; }

        public CeLeRyMod(string name, string author, string version, string entry_method_name)
        {
            Name = name;
            Author = author;
            Version = version;
            EntryMethodName = entry_method_name;
        }
    }
}
