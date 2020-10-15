using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YAK.CLI
{
    public class CLI
    {
        public static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static bool Confirm(string question)
        {
            Console.Write(question);
            var result = Console.ReadLine();
            if (result.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Prompt(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }
    }
}
