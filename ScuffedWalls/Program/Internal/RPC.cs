using DiscordRPC;
using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class RPC
    {
        static DiscordRpcClient client;
        public BeatMap CurrentMap { get; set; }
        public int Workspaces { get; set; }
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
            while (CurrentMap == null) await Task.Delay(500);

            if (!Utils.ScuffedConfig.HideMapInRPC) client.UpdateDetails(Utils.Info["_songName"].ToString());

            while (true)
            {
                List<string> RPCMsg = new List<string>()
                {
                    $"{CurrentMap._events.Count} Lights",
                    $"{CurrentMap._notes.Count} Notes",
                    $"{CurrentMap._obstacles.Count} Walls",
                    $"{Workspaces} Workspaces"
                };

                if (CurrentMap._customData != null)
                    foreach (var coolthing in CurrentMap._customData.Where(item => item.Value is IEnumerable<object> aray && aray.Count() > 0))
                        RPCMsg.Add($"{((IEnumerable<object>)coolthing.Value).Count()} {coolthing.Key}");

                foreach (string mesg in RPCMsg)
                {
                    client.UpdateState(mesg);
                    await Task.Delay(5000);
                }
            }
        }
    }


}
