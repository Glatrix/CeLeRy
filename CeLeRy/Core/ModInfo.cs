using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CeLeRy.Core
{
    public class ModInfo
    {
        public CeLeRyMod ModData;
        public MethodInfo EntryPoint;

        public ModInfo(CeLeRyMod modInfo, MethodInfo entryPoint)
        {
            ModData = modInfo;
            EntryPoint = entryPoint;
        }
    }
}
