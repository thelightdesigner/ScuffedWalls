using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ModChart.Wall
{
    static class ImageConverter
    {
        public class ImageSettings
        {
            public ImageSettings(bool isBlackEmpty, float scale, float thicc, bool track, bool centered, float spread, float alfa, BeatMap.Obstacle baseWall)
            { this.isBlackEmpty = isBlackEmpty; this.scale = scale; this.thicc = thicc; this.centered = centered; this.spread = spread; this.alfa = alfa; this.Wall = baseWall; }
            public ImageSettings() { }


            public bool isBlackEmpty; public float scale; public float thicc; public bool track; public bool centered; public float spread; public float alfa; public BeatMap.Obstacle Wall;
        }
        public static BeatMap.Obstacle[] Imag3ToWall(string ImagePath, ImageSettings settings)
        {
            Bitmap Image = new Bitmap(ImagePath);
            Pixel[] FactoredPixels = Pixel.Analyze(Image,0.05f);
            return FactoredPixels.Select(p => { return p.toWall(settings); }).ToArray();
        }

        private class Pixel
        {
            public int StartPos { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Layer { get; set; }
            public Color Color { get; set; }
            public void addWidth()
            {
                Width++;
            }
            public void addHeight()
            {
                Height++;
            }
            public BeatMap.Obstacle toWall(ImageSettings imageSettings)
            {
                return new BeatMap.Obstacle()
                {
                    _time = imageSettings.Wall._time.,
                    _type = 0,
                    _lineIndex = 0,
                    _duration = imageSettings.Wall._duration,
                    _width = 0,
                    _customData = new BeatMap.CustomData()
                    {
                        _color = new object[] { Color.R, Color.G, Color.B, 1 },
                        _scale = new object[] { Width, Height, 1 },
                        _position = new object[] { StartPos, Layer }
                    }
                };
            }
            public Pixel flipVertical(int imageHeight)
            {
                Layer = imageHeight - Layer;
                return this;
            }
            public static Pixel getLast(Pixel[] pixels, int x, int y)
            {
                return getCurrent(pixels, x, y - 1);
            }
            public static Pixel getCurrent(Pixel[] pixels, int x, int y)
            {
                if (!pixels.Any(p => p.Layer == y && p.StartPos == x)) return null;
                return pixels.Where(p => p.Layer == y && p.StartPos == x).First();
            }
            public bool Equals(Pixel otherpixel, float tolerance)
            {
                //if (StartPos != otherpixel.StartPos) return false; //these are kind of redundant
                //if (Layer - otherpixel.Layer > 1) return false;
                if (otherpixel == null) return false;
                if (!Color.Equals(otherpixel.Color, tolerance)) return false;
                if (Width != otherpixel.Width) return false;
                return true;
            }
            public static Pixel[] Analyze(Bitmap WallImageBitMap, float CompressionFactor)
            {
                List<Pixel> pixels = new List<Pixel>();
                Pixel CurrentPixel = null;
                //0,0 at top left
                for (int y = 0; y < WallImageBitMap.Height; y++)
                {
                    for (int x = 0; x < WallImageBitMap.Width; x++)
                    {
                        if (GetCurrent(x, y).Equals(GetLast(x, y), CompressionFactor) && x != 0)
                        {
                            CurrentPixel.addWidth();
                        }
                        else
                        {
                            if (CurrentPixel != null) pixels.Add(CurrentPixel.DeepClone());
                            CurrentPixel = new Pixel() { Color = GetCurrent(x, y), Height = 1, Layer = y, StartPos = x, Width = 1 };
                        }
                    }
                }
                if (CurrentPixel != null) pixels.Add(CurrentPixel.DeepClone());
                //mirror x, 0,0 bottom left to better work with beat saber
                pixels = pixels.Select(p => { return p.flipVertical(WallImageBitMap.Height - 1); }).ToList();

                List<Pixel> factoredPixels = new List<Pixel>();

                //0,0 at bottom left 
                
                for (int x = 0; x < WallImageBitMap.Width; x++)
                {
                    for (int y = 0; y < WallImageBitMap.Height; y++)
                    {
                        if (getCurrent(pixels.ToArray(), x, y) != null && getCurrent(pixels.ToArray(), x, y).Equals(getLast(pixels.ToArray(), x, y), CompressionFactor))
                        {
                            CurrentPixel.addHeight();
                        }
                        else
                        {
                            if (CurrentPixel != null) factoredPixels.Add(CurrentPixel.DeepClone());
                            CurrentPixel = getCurrent(pixels.ToArray(), x, y);
                        }
                    }
                }
                
             //   return pixels.ToArray();
                return factoredPixels.ToArray();

                Color GetLast(int x, int y)
                {
                    if (x > 0) return GetCurrent(x - 1, y);
                    return null;
                }
                Color GetCurrent(int x, int y)
                {
                    return new Color()
                    {
                        R = Convert.ToSingle(WallImageBitMap.GetPixel(x, y).R) / 255f,
                        G = Convert.ToSingle(WallImageBitMap.GetPixel(x, y).G) / 255f,
                        B = Convert.ToSingle(WallImageBitMap.GetPixel(x, y).B) / 255f,
                        A = Convert.ToSingle(WallImageBitMap.GetPixel(x, y).A) / 255f
                    };
                }
            }
        }












        //older version
        public static BeatMap.Obstacle[] Image2Wall(string ImagePath, bool isBlackEmpty, float scale, float thicc, bool track, bool centered, float spread, float alfa, BeatMap.CustomData customData, BeatMap.Obstacle baseWall)
        {
            Bitmap WallImageBitMap = new Bitmap(ImagePath, true);
            List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
            Random rnd = new Random();
            customData._animation ??= new BeatMap.CustomData.Animation();

            for (int y = 0; y < WallImageBitMap.Height; y++)
            {
                for (int x = 0; x < WallImageBitMap.Width; x++)
                {

                    if (!isBlackEmpty || ((WallImageBitMap.GetPixel(x, y).R + WallImageBitMap.GetPixel(x, y).G + WallImageBitMap.GetPixel(x, y).B > 10) && isBlackEmpty && WallImageBitMap.GetPixel(x, y).A > 10))
                    {
                        //position
                        object[][] defPos = null;
                        object[] pos = null;

                        if (!track)
                        {
                            customData._animation._definitePosition ??= new object[][] { new object[] { 0, 0, 0, 0 }, new object[] { 0, 0, 0, 1 } };
                            List<object[]> DefPos = new List<object[]>();
                            foreach (var defposset in customData._animation._definitePosition)
                            {
                                object X = (x * scale) + Convert.ToSingle(defposset[0].ToString());
                                if (centered) X = (((scale * x) - ((scale * WallImageBitMap.Width) / 2)) + 2);

                                List<object> points = new List<object>();
                                points.AddRange(new object[] {
                                X,
                                (scale * (WallImageBitMap.Height - y)) + Convert.ToSingle(defposset[1].ToString()),
                                Convert.ToSingle(defposset[2].ToString()),
                                Convert.ToSingle(defposset[3].ToString())
                                });
                                if (defposset.Length > 4) points.Add(defposset[4]);

                                DefPos.Add(points.ToArray());
                            }
                            defPos = DefPos.ToArray();
                        }
                        else
                        {
                            customData._position ??= new object[] { 0, 0 };
                            object X;
                            if (!centered) X = (x * scale) + Convert.ToSingle(customData._position[0].ToString());
                            else X = (((scale * x) - ((scale * WallImageBitMap.Width) / 2)));

                            pos = new object[] {
                                X,
                                (scale * (WallImageBitMap.Height - y)) + Convert.ToSingle(customData._position[1].ToString())
                            };
                        }

                        object[] color = { Convert.ToSingle(WallImageBitMap.GetPixel(x, y).R) / 255f, Convert.ToSingle(WallImageBitMap.GetPixel(x, y).G) / 255f, Convert.ToSingle(WallImageBitMap.GetPixel(x, y).B) / 255f, alfa };
                        if (customData._color != null) color = customData._color;

                        Walls.Add(new BeatMap.Obstacle()
                        {
                            _time = baseWall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * spread,
                            _duration = baseWall._duration,
                            _lineIndex = 0,
                            _type = 0,
                            _width = 0,
                            _customData = new BeatMap.CustomData()
                            {
                                _color = color,
                                _position = pos,
                                _scale = new object[] { scale / thicc, scale / thicc, scale / thicc },
                                _animation = new BeatMap.CustomData.Animation()
                                {
                                    _definitePosition = defPos,
                                    _scale = new object[][] { new object[] { thicc, thicc, thicc, 0 }, new object[] { thicc, thicc, thicc, 1 } }
                                }
                            }
                        }.Append(customData, 0)); //no overwrites
                    }
                }
            }
            return Walls.ToArray();
        }
    }

}
