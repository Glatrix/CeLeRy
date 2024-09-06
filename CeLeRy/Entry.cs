using CeLeRy.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace CeLeRy
{
    // !!MUST!! Stay Public
    public static class Entry
    {
        // !!MUST!! Stay Public
        // Make New Thread so CeLeRy can load everything
        public static void Main() => new Thread(__Main).Start();

        private static void __Main()
        {
            ModLoader.Run();
        }
    }
}
