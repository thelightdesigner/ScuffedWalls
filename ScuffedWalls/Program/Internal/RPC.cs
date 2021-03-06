using DiscordRPC;
using ModChart;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class RPC
    {
        static DiscordRpcClient client;
        public BeatMap currentMap { get; set; }
        public int workspace { get; set; }
        public RPC()
        {
            if (client == null)
            {
                client = new DiscordRpcClient("791404111161196545");
                client.OnError += (object sender, DiscordRPC.Message.ErrorMessage args) => { Console.WriteLine($"RPC Error: {args.Message}"); };
                client.Initialize();
                client.SetPresence(new RichPresence()
                {
                    Details = $"??",
                    State = $"?? Map Objects",
                    Timestamps = Timestamps.Now,
                    Assets = new Assets()
                    {
                        LargeImageKey = "scuffed_png",
                        LargeImageText = $"ScuffedWalls {ScuffedWalls.ver}",
                        SmallImageKey = "??",
                        SmallImageText = "??"
                    }
                });
            }
            var autoUpdater = autoUpdateRPC();
        }
        async Task autoUpdateRPC()
        {
            //while (currentMap == null) await Task.Delay(20);

            client.UpdateDetails($"remies map");
            Random rnd = new Random();

            while (true)
            {
                string[] RPCMsg =
                {
                $"at least 5 CustomEvents i think dont go asking me because i probably dont know or wouldnt answer anyways tbh i dont even know how discord can fit this long of a message into its rich presence",
                $"~2 Events",
                $"1,237,241 Notes",
                $"7.8+ Billion Walls",
                $"609+ billion Workspaces"
                };
                foreach (string mesg in RPCMsg)
                {
                    Console.WriteLine("updating");
                    client.UpdateState(mesg);
                    Console.WriteLine("done");
                    await Task.Delay(5000);
                }
            }
        }
    }


}
