using System.Collections.Generic;

namespace ModChart
{
    public class MapStats : Dictionary<string, int>
    {
        public void AddStat(string name, int count)
        {
            if (ContainsKey(name)) this[name] += count;
            else this[name] = count;
        }
        public void AddStat(KeyValuePair<string, int> stat)
        {
            AddStat(stat.Key, stat.Value);
        }
        public void AddStats(IEnumerable<KeyValuePair<string, int>> stats)
        {
            foreach (var stat in stats) AddStat(stat);
        }
    }
}
