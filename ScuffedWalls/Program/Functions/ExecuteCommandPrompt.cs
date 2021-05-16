using System;
using System.Diagnostics;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Run", "cmd", "Terminal", "Execute")]
    class ExecuteCommandPrompt : SFunction
    {
        public override void Run()
        {
            string InputArgs = GetParam("Args", "", p => p);
            bool EarlyRun = GetParam("RunBefore", false, p => bool.Parse(p));


            void Execute()
            {
                Process cmd = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true
                    }
                };
                cmd.Start();

                cmd.StandardInput.WriteLine($"{InputArgs}");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                string output = cmd.StandardOutput.ReadToEnd();
                string error = cmd.StandardError.ReadToEnd();
                string stream = string.IsNullOrEmpty(error) ? output : error;
                ConsoleColor color = string.IsNullOrEmpty(error) ? ConsoleColor.Green : ConsoleColor.Yellow;


                Console.ForegroundColor = color;
                Console.WriteLine("------------Command Prompt Output------------");
                Console.ResetColor();

                Console.WriteLine(stream);

                Console.ForegroundColor = color;
                Console.WriteLine("---------- End Command Prompt Output---------");
                Console.ResetColor();

                cmd.WaitForExit();
            }

            if (!EarlyRun) Utils.OnProgramComplete += Execute;
            else Utils.OnChangeDetected += Execute;


        }
    }
}
