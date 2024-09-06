using System.Text;

namespace CeLeRy.Logging
{
    [Flags]
    public enum LogLevel
    {
        Information = 1,
        Highlight   = 2,
        Warning     = 4,
        Error       = 8,
        All         = 16,
    }

    public static class Logger
    {
        private static string? LogFileName;
        private static bool saveLogs = false;

        public static void StartSavingLogsInFile(string fileName)
        {
            string fullPath = Path.GetFullPath(fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            File.Create(fullPath).Close();

            saveLogs = true;
            LogFileName = fullPath;
        }

        public static void Log(string message, LogLevel logLevel)
        {
            ConsoleColor oldColor = Console.ForegroundColor;

            string time = DateTime.UtcNow.ToString("HH:mm:ss:ff");
            string format = $"[{time}] [CeLeRy]: {message}";

            switch (logLevel)
            {
                case LogLevel.Information:  Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Highlight:    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:      Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:        Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:                    Console.ForegroundColor = oldColor;
                    break;
            }

            Console.WriteLine(format);

            if (saveLogs && LogFileName != null)
            {
                using (var write = File.AppendText(LogFileName))
                {
                    write.WriteLine(format);
                }
            }

            Console.ForegroundColor = oldColor;
        }
    }
}