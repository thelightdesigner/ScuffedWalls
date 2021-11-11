using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

using static ModChart.BeatMap.Note;

namespace ScuffedWalls.Functions
{
    [SFunction("AddSound")]
    class SoundExtensions : ScuffedFunction
    {
        protected override void Init()
        {
            float[] Times = GetParam("times", Array.Empty<float>(), p => p.Split(',').Select(h => float.Parse(h)).ToArray());
            NoteType FilterType = GetParam("type", NoteType.Bomb | NoteType.Right | NoteType.Left, p => Enum.Parse<NoteType>(p));
            CutDirection FilterDirection = GetParam("direction",
                CutDirection.Dot | CutDirection.Down | CutDirection.DownLeft | CutDirection.DownRight | CutDirection.Left | CutDirection.Right | CutDirection.Up | CutDirection.UpLeft | CutDirection.UpRight,
                p => Enum.Parse<CutDirection>(p));




            string path = GetParam("path", "", p => p);

            int id = 0;

            Utils.InfoDifficulty["_customData"] ??= new TreeDictionary();

            if (Utils.InfoDifficulty["_customData._sounds"] is IList<object> _sounds)
            {
                if (_sounds.All(s => s.ToString() != path)) _sounds.Add(path);
                id = _sounds.IndexOf(path);
            }
            else
            {
                var newList = new List<object>
                {
                    path
                };

                Utils.InfoDifficulty["_customData._sounds"] = newList;
                
            }


            var soundnotes = InstanceWorkspace.Notes.Where(n => Times.Any(t => t == n._time) && FilterType.HasFlag(n._type) && FilterDirection.HasFlag(n._cutDirection));
            foreach (var note in soundnotes)
            {
                note._customData ??= new TreeDictionary();
                note._customData["_soundID"] = id;
            }

        }
    }
}
