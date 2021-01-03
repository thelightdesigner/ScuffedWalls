using System;
using System.Threading.Tasks;
using DiscordRPC;

namespace ScuffedWalls
{
    class RPC
    {
        DiscordRpcClient client;
        public MapObj currentMap { get; set; }
        public RPC()
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
            var autoUpdater = autoUpdateRPC();
        }
        async Task autoUpdateRPC()
        {
            while (currentMap == null) await Task.Delay(20);
            while(true)
            {
                for(int i = 0; i < 3; i++)
                {
                    refresh(currentMap.MapName, currentMap,(MapObj.MapObjs)i);
                    await Task.Delay(5000);
                }
            }
        }
        void refresh(string mapName, MapObj mapobjcount, MapObj.MapObjs type)
        {
            client.UpdateDetails($"{mapName}");
            client.UpdateState($"{typeof(MapObj).GetField(type.ToString()).GetValue(mapobjcount)} {type}");
        }
    }
    public class MapObj
    {
        public string MapName;
        public int Walls;
        public int Notes;
        public int CustomEvents;
        public enum MapObjs
        {
            Walls,
            Notes,
            CustomEvents
        }
    }
}
