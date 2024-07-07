using DiscordRPC;
using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class RPC
    {
        static DiscordRpcClient client;

        static Task AutoUpdater;
        public DifficultyV3 CurrentMap { get; set; }
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
                        LargeImageKey = "scuffed_v2_update",
                        LargeImageText = $"ScuffedWalls {ScuffedWalls.Version}",
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

            if (!ScuffedWallsContainer.ScuffedConfig.HideMapInRPC) client.UpdateDetails(ScuffedWallsContainer.Info["_songName"].ToString());

            while (true)
            {
                List<KeyValuePair<string, int>> RPCMsg = GetMapStats(CurrentMap);

                RPCMsg.Add(new KeyValuePair<string, int>("Workspace".MakePlural(Workspaces), Workspaces));

                foreach (var mesg in RPCMsg)
                {
                    client.UpdateState($"{mesg.Value} {mesg.Key}");
                    await Task.Delay(5000);
                }
            }
        }
        public List<KeyValuePair<string, int>> GetMapStats(DifficultyV3 diff)
        {
            List<KeyValuePair<string, int>> stats = new();
            foreach (var prop in typeof(DifficultyV3).GetProperties().Where(p => p.GetCustomAttributes<MapStatAttribute>().Any()))
            {
                object value = prop.GetValue(this);
                if (value is IEnumerable<object> array) stats.Add(new(prop.Name.MakeTitleFormat().MakePlural(array.Count()), array.Count()));
                else if (value is IDictionary<string, object> dict)
                {
                    foreach (var key in dict.Keys)
                        if (dict[key] is IEnumerable<object> arr)
                            stats.Add(new(key.MakeTitleFormat().MakePlural(arr.Count()), arr.Count()));
                }
            }
            return stats;
        }
    }


}
