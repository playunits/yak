using System;
using System.Collections.Generic;
using System.Text;

namespace YAK.CLI.UI
{
    public class Utils
    {
        public static void ClearCurrentLine()
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }

        public static void Status(IOStatus status)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            switch (status)
            {
                case IOStatus.OK:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case IOStatus.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case IOStatus.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case IOStatus.DEFAULT:                    
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.Write(status.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");
        }

        public enum IOStatus
        {
            OK,
            ERROR,
            WARNING,
            DEFAULT
        }
    }
}
