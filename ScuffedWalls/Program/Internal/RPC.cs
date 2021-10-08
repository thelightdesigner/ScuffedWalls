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

        static Task AutoUpdater;
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
            AutoUpdater = autoUpdateRPC();
        }
        async Task autoUpdateRPC()
        {
            while (CurrentMap == null) await Task.Delay(500);

            if (!Utils.ScuffedConfig.HideMapInRPC) client.UpdateDetails(Utils.Info["_songName"].ToString());

            while (true)
            {
                List<KeyValuePair<string, int>> RPCMsg = CurrentMap.Stats;
                RPCMsg.Add(new KeyValuePair<string, int>("Workspace".MakePlural(Workspaces), Workspaces));

                foreach (var mesg in RPCMsg)
                {
                    client.UpdateState($"{mesg.Value} {mesg.Key}");
                    await Task.Delay(5000);
                }
            }
        }
    }


}
