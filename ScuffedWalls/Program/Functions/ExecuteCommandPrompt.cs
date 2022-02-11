using System;
using System.Diagnostics;
using System.IO;

namespace ScuffedWalls.Functions
{
    [SFunction("Run", "cmd", "Terminal", "Execute")]
    class ExecuteCommandPrompt : ScuffedFunction
    {
        protected override void Init()
        {
            string JSfile = GetParam("Javascript", null, p => Path.Combine(ScuffedWallsContainer.ScuffedConfig.MapFolderPath, p));
            AddRefresh(JSfile);
            bool EarlyRun = GetParam("RunBefore", false, bool.Parse);


            string InputArgs;
            if (JSfile != null)
            {
             InputArgs = JSfile.EndsWith(".ts") ? $"ts-node \"{JSfile}\" " : $"node \"{JSfile}\"";
            }
            else
            {
                InputArgs =  GetParam("Args", "", p => p);
            }

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
                        ,
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

            if (!EarlyRun) ScuffedWallsContainer.OnProgramComplete += Execute;
            else ScuffedWallsContainer.OnChangeDetected += Execute;


        }
    }
}
