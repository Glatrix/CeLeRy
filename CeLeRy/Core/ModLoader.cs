using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CeLeRy.Logging;

namespace CeLeRy.Core
{
    public static class ModLoader
    {
        public static string LogFile = $".\\CeLeRy\\latest_logs.txt";
        public static string ModsFolder = $".\\mods\\";

        public static bool IncludeDesktopAppDependencies = true;
        public static bool IncludeASPNETCoreDependencies = false;

        private static string[] BIG_TEXT = 
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

        private static string[] BAD_DEPS =
        [
            "Microsoft.DiaSymReader.Native.x86.dll",
            "System.IO.Compression.Native.dll",
            "System.Private.CoreLib.dll",
        ];

        // ==================================
        // Functions To:
        // 1. Setup Console and Logging
        // 2. Load Needed .NET ?.? Runtimes
        // 3. Load and Invoke Mods
        // ==================================

        /// <summary>
        /// Makes the whole thing go!
        /// </summary>
        public static void Run()
        {
            Setup();

            // Was Originally In Setup. Moved here for visibility
            Logger.StartSavingLogsInFile(LogFile);

            PrintBigText();
            LoadRuntime();
            LoadMods();
        }

        /// <summary>
        /// Misc Mod Loader Setup
        /// </summary>
        private static void Setup()
        {
            Console.Title = "CeLeRy Mod Loader";
        }

        /// <summary>
        /// Print The Big Text!!! Super Important!!!
        /// </summary>
        private static void PrintBigText()
        {
            foreach (string bigTextPiece in BIG_TEXT)
            {
                Logger.Log(bigTextPiece, LogLevel.Information);
            }
            Logger.Log("", LogLevel.Information);
            Logger.Log("", LogLevel.Information);
            Logger.Log("", LogLevel.Information);
        }

        /// <summary>
        /// Load Intended .NET ?.? Runtimes
        /// </summary>
        private static void LoadRuntime()
        {
            Logger.Log("Loading Runtime...", LogLevel.Information);
            List<string> loadedDeps = new List<string>();

            List<string> runtimeDlls = GetRuntimeDLLs();

            runtimeDlls = runtimeDlls.OrderBy((dep) => dep.Count((c) => c == '.')).ToList();

            foreach (string dep in runtimeDlls)
            {
                try
                {
                    string dllName = Path.GetFileName(dep);
                    if (!loadedDeps.Contains(dllName))
                    {
                        Assembly assembly = Assembly.LoadFrom(dep);
                        loadedDeps.Add(dllName);
                    }
                }
                catch
                {
                    Logger.Log($"Runtime Load DLL Fail: {Path.GetFileName(dep)}", LogLevel.Warning);
                }
            }


        }

        /// <summary>
        /// Find, Load, and Run All Mods
        /// </summary>
        private static void LoadMods()
        {
            Logger.Log("Loading Mods...", LogLevel.Information);

            List<ModInfo> mods = GetMods();

            foreach (ModInfo mod in mods)
            {
                new Thread(() => { mod.EntryPoint.Invoke(null, null); }).Start();
            }

            Logger.Log("Done! Mods Now Running", LogLevel.Information);
        }

        // ==================================
        // Helper Functions Beyond This Point
        // ==================================

        private static List<string> GetRuntimeDLLs()
        {
            List<string> ret = new();

            string coreRuntime = RuntimeEnvironment.GetRuntimeDirectory();
            string deskRuntime = coreRuntime.Replace("Microsoft.NETCore.App", "Microsoft.WindowsDesktop.App");
            string ASP_Runtime = coreRuntime.Replace("Microsoft.NETCore.App", "Microsoft.AspNetCore.App");

            void __GRAB(string folder)
            {
                if (!Directory.Exists(folder)) { return; }

                foreach(string file in Directory.GetFiles(folder))
                {
                    string fileName = Path.GetFileName(file);

                    bool isDll = fileName.EndsWith(".dll");
                    bool isSys = fileName.StartsWith("System.");
                    bool isMic = fileName.StartsWith("Microsoft.");
                    bool isBad = BAD_DEPS.Contains(fileName);

                    if (isDll && (isSys || isMic) && !isBad)
                    {
                        ret.Add(file);
                    }
                }
            }

            __GRAB(coreRuntime);
            if (IncludeDesktopAppDependencies) { __GRAB(deskRuntime); }
            if (IncludeASPNETCoreDependencies) { __GRAB(ASP_Runtime); }

            return ret;
        }

        private static void LogMod(ModInfo mod)
        {
            Logger.Log("+----------------| LOADING MOD |----------------+", LogLevel.Highlight);
            Logger.Log($"Mod Name: {mod.ModData.Name}", LogLevel.Highlight);
            Logger.Log($"Mod Auth: {mod.ModData.Author}", LogLevel.Highlight);
            Logger.Log($"Mod Vers: {mod.ModData.Version}", LogLevel.Highlight);
            Logger.Log("+-----------------------------------------------+", LogLevel.Highlight);
        }

        private static List<ModInfo> GetMods()
        {
            List<ModInfo> ret = new List<ModInfo>();

            if (!Directory.Exists(ModsFolder)) 
            {
                Directory.CreateDirectory(ModsFolder);
                return ret;
            }

            foreach (string file in Directory.GetFiles(ModsFolder))
            {
                try
                {
                    if (file.EndsWith(".dll") && TryGetModInfo(file, out ModInfo? mod))
                    {
                        ret.Add(mod);
                    }
                }
                catch(Exception ex)
                {
                    Logger.Log($"Error Loading: '{Path.GetFileName(file)}'\n{ex}", LogLevel.Error);
                }
            }

            return ret;
        }

        private static bool TryGetModInfo(string file, out ModInfo? modInfo)
        {
            bool attributeFound = false;
            string fileName = Path.GetFileName(file);
            Assembly assembly = Assembly.LoadFrom(file);
            foreach (var t in assembly.GetTypes())
            {
                CeLeRyMod? ceLeRyMod = t.GetCustomAttribute<CeLeRyMod>();
                if (ceLeRyMod == null)
                {
                    continue;
                }
                attributeFound = true;
                MethodInfo? entry = t.GetMethod(ceLeRyMod.EntryMethodName);
                if (entry == null)
                {
                    break;
                }
                modInfo = new ModInfo(ceLeRyMod, entry);
                LogMod(modInfo);
                return true;
            }

            if (attributeFound)
            {
                Logger.Log($"Mod '{fileName}' No Entry Function Found", LogLevel.Error);
            }
            else
            {
                Logger.Log($"Mod '{fileName}' No Entry Class Found", LogLevel.Error);
            }

            modInfo = null;
            return false;
        }
    }
}
