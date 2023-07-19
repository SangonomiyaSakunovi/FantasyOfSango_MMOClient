using System;
using System.Threading;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public static class IOCPLog
    {
        public static Action<string> LogInfoCallBack;
        public static Action<string> LogErrorCallBack;
        public static Action<string> LogWarningCallBack;

        public static void Info(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.None);
            }
        }

        public static void Start(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.Blue);
            }
        }

        public static void Special(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.Magenta);
            }
        }

        public static void Done(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.Green);
            }
        }

        public static void Processing(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogInfoCallBack != null)
            {
                LogInfoCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.Cyan);
            }
        }

        public static void Error(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogErrorCallBack != null)
            {
                LogErrorCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.Red);
            }
        }

        public static void Warning(string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            if (LogWarningCallBack != null)
            {
                LogWarningCallBack(message);
            }
            else
            {
                ConsoleLog(message, IOCPLogColor.Yellow);
            }
        }

        private static void ConsoleLog(string message, IOCPLogColor color)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            message = string.Format("Thread:{0} {1}", threadId, message);
            switch (color)
            {
                case IOCPLogColor.None:
                    Console.WriteLine(message);
                    break;
                case IOCPLogColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Red:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
