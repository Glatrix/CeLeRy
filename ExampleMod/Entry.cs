using CeLeRy;
using CeLeRy.Logging;

namespace ExampleMod
{
    [CeLeRyMod("Example Mod", "Glatrix", "1.0.0", "Main")]
    public class Class1
    {
        public static void Main()
        {
            Logger.Log("Mod Running!", LogLevel.Information);
        }
    }
}
