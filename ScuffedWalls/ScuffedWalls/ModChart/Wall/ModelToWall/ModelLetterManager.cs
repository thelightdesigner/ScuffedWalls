using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ModChart.Wall
{
    class ModelLetterManager : IPlaceableLetterWallCollection
    {
        public Cube[] Cubes;
        public alphabet Character { get; set; }
        public Vector2 Dimensions { get; set; }
        public TextSettings Settings { get; set; }

        private static readonly float scalefactor = 4;
        public static IEnumerable<ModelLetterManager> CreateLetters(Model model, TextSettings Settings)
        {
            var letters = model.Cubes
                .Where(c => c.Material != null && c.Material.Any(s => s.ToLower().Contains("letter_")))
                .GroupBy(c => Regex.Split(c.Material.Where(s => s.ToLower().Contains("letter_")).First(), "letter_", RegexOptions.IgnoreCase).Last());
            List<ModelLetterManager> Letters = new List<ModelLetterManager>();
            //Console.WriteLine(letters.Count());
            foreach (var lettercollect in letters)
            {
                object CharVal = alphabet.nonchar;

                if (!Enum.TryParse(typeof(alphabet), lettercollect.Key.ToString(), out CharVal))
                {
                    throw new ArgumentException($"Character {lettercollect.Key} is not a member of the character enumerator");
                }



                //scale accordingly
                var cubes = Cube.TransformCollection(new DeltaTransformOptions()
                {
                    cubes = lettercollect,
                    Scale = Settings.ImageSettings.scale * scalefactor,
                    TransformOrigin = DeltaTransformOptions.TransformOptions.WorldOrigin
                });

                Transformation FullDim = cubes.Select(L => L.Matrix.Value).GetBoundingBox().Main;

                cubes = Cube.TransformCollection(new DeltaTransformOptions()
                {
                    cubes = cubes,
                    Position = new Vector3(((-FullDim.Position.X) - (FullDim.Scale.X / 2f)), 0, 0)
                });

                Vector2 Dim = new Vector2(FullDim.Scale.X, FullDim.Scale.Y);

                Letters.Add(new ModelLetterManager()
                {
                    Cubes = cubes.ToArray(),
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
        public IEnumerable<ICustomDataMapObject> PlaceAt(Vector2 pos)
        {
            var transCubes = Cube.TransformCollection(new DeltaTransformOptions()
            {
                cubes = Cubes,
                Position = new Vector3(-pos.X, pos.Y, 0)
            });
            var extracted = new WallModel(transCubes.ToArray(), Settings.ModelSettings);
            return extracted.Output._obstacles.Cast<ICustomDataMapObject>()
                .CombineWith(extracted.Output._notes.Cast<ICustomDataMapObject>());
        }
    }
}
