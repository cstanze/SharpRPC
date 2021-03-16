using System;
using System.Diagnostics;

namespace SharpRPC
{
    public class Logger
    {
        public string InstanceID { get; private set; }

        public int Level { get; set; }

        public const int LevelError = 0;
        public const int LevelInfo = 1;
        public const int LevelDebug = 2;

        public Logger(string ID, int level = LevelDebug)
        {
            this.InstanceID = ID;
            this.Level = level;
        }

        public void info(string infoText)
        {
            if (this.Level < LevelInfo) return;
            Console.WriteLine("[*] [Info] [" + InstanceID + "] " + infoText);
        }

        public void debug(string infoText)
        {
            if (this.Level < LevelDebug) return;
            Console.WriteLine("[*] [Debug] [" + InstanceID + "] " + infoText);
        }

        public void exception(string infoText)
        {
            if (this.Level < LevelError) return;
            Console.WriteLine("[*] [Exception] [" + InstanceID + "] " + infoText);
        }

        private string ConstructCallerID()
        {
            return GetCallingFile() + "." + GetCallingMethod();
        }

        private string GetCallingFile()
        {
            return (new System.Diagnostics.StackTrace()).GetFrame(1).GetFileName().Replace(".cs", "") ?? "UnknownFile";
        }

        private string GetCallingMethod()
        {
            return (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name;
        }
    }
}
