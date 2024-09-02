using System;
using System.Reflection;
using System.Threading;

namespace CeLeRy
{
    public static class Entry
    {
        public static void Main() => new Thread(ShadowMain).Start();


        public static string[] BIG_TEXT = 
        [
            @"   _____     _          _____       ",
            @"  / ____|   | |        |  __ \      ",
            @" | |     ___| |     ___| |__) |   _ ",
            @" | |    / _ \ |    / _ \  _  / | | |",
            @" | |___|  __/ |___|  __/ | \ \ |_| |",
            @"  \_____\___|______\___|_|  \_\__, |",
            @"  Lightweight .NET Mod Loader  __/ |",
            @"  By Glatrix - 2024           |___/ "
        ];


        private static void ShadowMain()
        {
            Console.Title = "CeLeRy Mod Loader";
            ConsoleColor modColor = ConsoleColor.Green;

            // ====================================
            // Big Text
            // ====================================
            foreach(string bigTextPiece in BIG_TEXT)
            {
                Log(bigTextPiece);
            }
            Log("");
            Log("");
            Log("");



            // ====================================
            // Loading Runtime
            // ====================================
            if (Directory.Exists(".\\CeLeRy\\runtime"))
            {
                Log("Loading Runtime...");
                string[] deps = Directory.GetFiles(".\\CeLeRy\\runtime").Where((file) => file.EndsWith(".dll")).ToArray();
                foreach (string dep in deps)
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(dep);
                    }
                    catch
                    {
                        Log($"(Warning) Load Failure: {Path.GetFileName(dep)}", ConsoleColor.Yellow);
                    }
                }
            }



            // ====================================
            // Loading For Mods
            // ====================================
            Log("Loading Mods...");

            if (Directory.Exists(".\\mods"))
            {
                List<MethodInfo> EntryPoints = new List<MethodInfo>();
                string[] mods = Directory.GetFiles(".\\mods").Where((file) => file.EndsWith(".dll")).ToArray();

                foreach (string mod in mods)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(mod);

                        foreach (var t in assembly.GetTypes())
                        {
                            CeLeRyMod? ceLeRyMod = t.GetCustomAttribute<CeLeRyMod>();
                            if (ceLeRyMod != null)
                            {
                                MethodInfo? entry = t.GetMethod(ceLeRyMod.EntryMethodName);
                                if (entry != null)
                                {
                                    try
                                    {
                                        Log("================= LOADING MOD =================", modColor);
                                        Log($"Mod Name: {ceLeRyMod.Name}", modColor);
                                        Log($"Mod Auth: {ceLeRyMod.Author}", modColor);
                                        Log($"Mod Vers: {ceLeRyMod.Version}", modColor);
                                        Log("================================================", modColor);

                                        EntryPoints.Add(entry);
                                    }
                                    catch
                                    {
                                        Log($"Exception Thrown in Entry Point of {ceLeRyMod.Name} in '{mod}'", ConsoleColor.Red);
                                    }
                                }
                                else
                                {
                                    Log($"Invalid Mod Entry Method for {ceLeRyMod.Name}: {t.FullName} in '{mod}'", ConsoleColor.Red);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"DLL Not Recognized As Mod: '{mod}'", ConsoleColor.Red);
                        Log(ex.ToString(), ConsoleColor.Red);
                    }
                }

                // ====================================
                // Enabling Mods
                // ====================================
                Log("Done! Sending Pulse to Mods");
                foreach (var entry in EntryPoints)
                {
                    new Thread(() => { entry.Invoke(null, null); }).Start();
                }
            }
        }

        private static void Log(string msg, ConsoleColor color = ConsoleColor.Cyan)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            string time = DateTime.UtcNow.ToString("HH:mm:ss:ff");
            Console.WriteLine($"[{time}] [CeLeRy]: {msg}");

            Console.ForegroundColor = oldColor;
        }
    }
}
