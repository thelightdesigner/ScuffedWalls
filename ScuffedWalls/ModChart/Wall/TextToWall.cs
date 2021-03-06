using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace ModChart.Wall
{
    class WallText
    {
        public BeatMap.Obstacle[] Walls { get; private set; }

        TextSettings Settings;
        LetterCollection[] letterCollection;
        public WallText(TextSettings settings)
        {
            Settings = settings;
            letterCollection = LetterCollection.CreateLetters(new Bitmap(Settings.ImagePath), Settings.ImageSettings);
            GenerateText();
        }
        void GenerateText()
        {
            //Console.WriteLine(letterCollection.Length);
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();
            float scalefactor = Settings.ImageSettings.scale * 4f;
            float LineLayerPos = 0;
            for (int LineLayer = 0; LineLayer < Settings.Text.Length; LineLayer++)
            {
                float LineIndexPos = 0;
                for (int LineIndex = 0; LineIndex < Settings.Text[LineLayer].Length; LineIndex++)
                {
                    alphabet letter = alphabet.nonchar;
                    letter = (alphabet)Settings.Text[LineLayer][LineIndex];

                    if (letterCollection.Any(l => l.Character == letter))
                    {
                        var wallletter = letterCollection
                            .Where(letr => letr.Character == letter)
                            .First();

                        walls.AddRange(wallletter.PlaceAt(new Vector2(LineIndexPos, LineLayerPos)));
                        LineIndexPos += wallletter.Dimensions.X + (Settings.Letting * scalefactor);
                    }
                    else
                    {
                        LineIndexPos += Settings.Letting * scalefactor;
                    }

                }
                LineLayerPos += letterCollection.First().Dimensions.Y + (Settings.Leading * scalefactor);
            }

            //centeres the text
            Walls = walls.ToArray().Transform_Pos(new Vector2(-walls.ToArray().GetDimensions().X / 2f, 0));
        }

    }
    public class LetterCollection
    {
        public BeatMap.Obstacle[] Walls { get; set; }
        public alphabet Character { get; set; }
        public Vector2 Dimensions { get; private set; }

        public static LetterCollection[] CreateLetters(Bitmap bitmap, ImageSettings settings)
        {
            int i = 0;
            return new LetterBitmap(bitmap).Letters.Select(letr =>
            {
                var collection = new LetterCollection()
                {
                    Walls = new WallImage(letr, settings).Walls,
                    Character = Enum.Parse<alphabet>(((alphabetOrder)i).ToString()),
                    Dimensions = new Vector2(letr.Width.toFloat(), letr.Height.toFloat()) * settings.scale
                };
                i++;
                return collection;
            }).ToArray();
        }

        public static LetterCollection[] CreateLetters(Model model, ImageSettings settings)
        {
            return null;
        }

        public BeatMap.Obstacle[] PlaceAt(Vector2 pos)
        {
            return this.DeepClone().Set_Position(pos);
        }
    }
    class LetterBitmap
    {
        public Bitmap[] Letters { get; private set; }
        Bitmap LetterIMG;
        public LetterBitmap(Bitmap path)
        {
            LetterIMG = path;
            CreateLetters();
        }

        void CreateLetters()
        {
            List<Pixel> letters = new List<Pixel>();
            Pixel CurrentLetter = null;
            for (int x = 0; x < LetterIMG.Width; x++)
            {
                Pixel CurrentVerticleLine = null;
                if (!LetterIMG.IsVerticalBlackOrEmpty(new IntVector2(x, 0))) CurrentVerticleLine = new Pixel() { Position = new IntVector2(x, 0), Scale = new IntVector2(1, LetterIMG.Height) };

                bool CountLetter = CurrentVerticleLine != null && CurrentLetter != null;
                if (CountLetter)
                {
                    CurrentLetter.AddWidth();
                }
                else
                {
                    if (CurrentLetter != null) letters.Add(CurrentLetter);
                    CurrentLetter = CurrentVerticleLine;
                }
            }
            if (CurrentLetter != null) letters.Add(CurrentLetter);
            Letters = letters.Select(l =>
            {
                var cropped = LetterIMG.Crop(l);
                return cropped;
            }).ToArray();
        }
    }

    public class TextSettings
    {
        public string ImagePath { get; set; }
        public string[] Text { get; set; }
        public bool Centered { get; set; }
        public float Letting { get; set; }
        public float Leading { get; set; }
        public ImageSettings ImageSettings { get; set; }

    }
    public static class TextHelper
    {
        //sets the position of a collection of walls, account for thicc
        public static BeatMap.Obstacle[] Set_Position(this BeatMap.Obstacle[] walls, Vector2 Pos)
        {
            float XCorner = walls.OrderBy(w => w._customData._position.ToVector2().X).First()._customData._position[0].toFloat();
            float YCorner = walls.OrderBy(w => w._customData._position.ToVector2().Y).First()._customData._position[1].toFloat();

            Vector2 difference = new Vector2(XCorner, YCorner) - Pos;
            return walls.Select(wall =>
            {
                wall._customData._position = (wall._customData._position.ToVector2() - difference).FromVector2();
                return wall;
            }).ToArray();
        }
        public static BeatMap.Obstacle[] Set_Position(this LetterCollection walltext, Vector2 Pos)
        {
            return walltext.Walls.Select(wall =>
            {
                wall._customData._position = (wall._customData._position.ToVector2() + Pos).FromVector2();
                return wall;
            }).ToArray();
        }
        public static BeatMap.Obstacle[] Transform_Pos(this BeatMap.Obstacle[] walls, Vector2 pos)
        {
            return walls.Select(wall =>
            {
                wall._customData._position = (wall._customData._position.ToVector2() + pos).FromVector2();
                return wall;
            }).ToArray();
        }
    }
    public enum alphabet
    {
        a = 'a', b = 'b', c = 'c', d = 'd', e = 'e', f = 'f', g = 'g', h = 'h', i = 'i', j = 'j', k = 'k', l = 'l', m = 'm', n = 'n', o = 'o', p = 'p', q = 'q', r = 'r', s = 's', t = 't', u = 'u', v = 'v', w = 'w', x = 'x', y = 'y', z = 'z',
        A = 'A', B = 'B', C = 'C', D = 'D', E = 'E', F = 'F', G = 'G', H = 'H', I = 'I', J = 'J', K = 'K', L = 'L', M = 'M', N = 'N', O = 'O', P = 'P', Q = 'Q', R = 'R', S = 'S', T = 'T', U = 'U', V = 'V', W = 'W', X = 'X', Y = 'Y', Z = 'Z',
        questionmark = '?', period = '.', exclamation = '!', space = ' ', apostrophe = '\'',
        nonchar = 0
    }
    public enum alphabetOrder
    {
        a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        questionmark, period, exclamation, apostrophe
    }



}


