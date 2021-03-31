using System;
using System.Collections.Generic;
using System.Linq;
using ModChart;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace ModChart.Wall
{
    class ModelLetterManager : IPlaceableLetterWallCollection
    {
        public Cube[] Cubes;
        public alphabet Character { get ; set; }
        public Vector2 Dimensions { get; set; }
        public TextSettings Settings { get; set; }
        public static IEnumerable<ModelLetterManager> CreateLetters(Model model, TextSettings Settings)
        {
            var letters = model.Cubes
                .Where(c =>  c.Material.Any( s =>  s.ToLower().Contains("letter_")))
                .GroupBy(c => Regex.Split(c.Material.Where(s => s.ToLower().Contains("letter_")).First(),"letter_",RegexOptions.IgnoreCase).Last());
            List<ModelLetterManager> Letters = new List<ModelLetterManager>();
            //Console.WriteLine(letters.Count());
            foreach(var lettercollect in letters)
            {
                object CharVal = alphabet.nonchar;
                
                if (!Enum.TryParse(typeof(alphabet), lettercollect.Key.ToString(), out CharVal))
                {
                    throw new ArgumentException($"Character {lettercollect.Key.ToString()} is not a member of the character enumerator");
                }
                else
                {
                    Console.WriteLine("added" + CharVal.ToString());
                }
                var FullDim = lettercollect.Select(L => L.Matrix.Value).GetBoundingBox().Main;
                var Dim = new Vector2(FullDim.Scale.X, FullDim.Scale.Y);

                Letters.Add(new ModelLetterManager()
                {
                    Cubes = lettercollect.ToArray(),
                    Character = (alphabet)CharVal,
                    Dimensions = Dim,
                    Settings = Settings
                });
            }

            return Letters;
        }
        /// <summary>
        /// Extracts the walls and transforms them to this position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public IEnumerable<BeatMap.Obstacle> PlaceAt(Vector2 pos)
        {
            var transCubes = Cube.TransformCollection(Cubes,new Vector3(-pos.X,pos.Y,0),new Vector3(0), 1, SetPos: true);
            var extracted = new WallModel(transCubes.ToArray(),Settings.ModelSettings);
            return extracted.Output._obstacles;
        }
    }
}
