using DiscordRPC;
using ModChart;
using System;
using System.IO;
using System.Threading;
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
                        LargeImageKey = "scuffed_v1_update",
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
            while (currentMap == null) await Task.Delay(500);

            client.UpdateDetails(Utils.Info._songName.ToString());

            while (true)
            {
                string[] RPCMsg =
                {
                $"{currentMap._customData._customEvents.Length} CustomEvents",
                $"{currentMap._events.Length} Lights",
                $"{currentMap._notes.Length} Notes",
                $"{currentMap._obstacles.Length} Walls",
                $"{workspace} Workspaces"
                };
                foreach (string mesg in RPCMsg)
                {
                    client.UpdateState(mesg);
                    await Task.Delay(5000);
                }
            }
        }
    }


}
