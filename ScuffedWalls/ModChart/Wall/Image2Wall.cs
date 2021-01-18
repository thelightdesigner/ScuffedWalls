using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace ModChart.Wall
{
    class WallImage
    {
        ImageSettings _Settings;
        Bitmap _Bitmap;
        BeatMap.Obstacle[] Walls;

        public WallImage(string path, ImageSettings settings)
        {
            _Settings = settings;
            _Bitmap = new Bitmap(path);
            Run();
        }
        public WallImage(Bitmap image, ImageSettings settings)
        {
            _Settings = settings;
            _Bitmap = image;
            Run();
        }

        public BeatMap.Obstacle[] GetWalls()
        {
            return Walls;
        }

        public void Run()
        {
            FloatingPixel[] Pixels =
                AnalyzeImage()
                .Select(p =>
                {
                    return p
                   .FlipVertical(_Bitmap)
                   .ToFloating()
                   .Resize(_Settings.scale);
                }) //size
                .ToArray();

            //centered offseter
            if (_Settings.centered) Pixels = Pixels.Select(p => { return p.Transform(new Vector2() { X = -(_Bitmap.Width.toFloat() * _Settings.scale / 2), Y = 0 }); }).ToArray();

            //position offseter
            if (_Settings.Wall._customData._position != null) Pixels = Pixels.Select(p => { return p.Transform(new Vector2() { X = _Settings.Wall._customData._position[0].toFloat(), Y = _Settings.Wall._customData._position[1].toFloat() }); }).ToArray();

            Random rnd = new Random();
            Walls = Pixels.Select(p =>
            {
                p = p.Transform(new Vector2() { X = (p.Scale.X / 2f) - (1f / _Settings.thicc * 2f), Y = 0 }); //thicc offseter
                float spread = (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * _Settings.spread;
                return new BeatMap.Obstacle()
                {
                    _time = _Settings.Wall._time.toFloat() + spread,
                    _duration = _Settings.Wall._duration,
                    _lineIndex = 0,
                    _type = 0,
                    _width = 0,
                    _customData = new BeatMap.CustomData()
                    {
                        _position = new object[] { p.Position.X, p.Position.Y },
                        _scale = new object[] { 1f / _Settings.thicc, 1f / _Settings.thicc, 1f / _Settings.thicc },
                        _color = p.Color.ToObjArray(_Settings.alfa),
                        _animation = new BeatMap.CustomData.Animation()
                        {
                            _scale = new object[][] { new object[] { p.Scale.X * _Settings.thicc, p.Scale.Y * _Settings.thicc, _Settings.scale * _Settings.thicc, 0 }, new object[] { p.Scale.X * _Settings.thicc, p.Scale.Y * _Settings.thicc, _Settings.scale * _Settings.thicc, 1 } }
                        }
                    }
                }.Append(_Settings.Wall._customData, AppendTechnique.NoOverwrites);

            }).ToArray();
        }

        Pixel[] AnalyzeImage()
        {
            return
                CompressPixels(
                CompressPixels(GetAllPixels(), _Settings.tolerance / _Settings.shift)
                .Select(p => { return p.Inverse(); }).ToArray(), _Settings.tolerance * _Settings.shift)
                .Select(p => { return p.Inverse(); }).ToArray();


            Pixel[] GetAllPixels()
            {
                List<Pixel> pixels = new List<Pixel>();
                IntVector2 Pos = new IntVector2();
                for (Pos.Y = 0; Pos.Y < _Bitmap.Height; Pos.Y++)
                {
                    for (Pos.X = 0; Pos.X < _Bitmap.Width; Pos.X++)
                    {
                        pixels.Add(_Bitmap.ToPixel(Pos));

                    }
                }
                return pixels.ToArray();
            }

            Pixel[] CompressPixels(Pixel[] pixels, float tolerance)
            {
                IntVector2 Pos = new IntVector2();
                IntVector2 Dimensions = pixels.GetDimensions();
                List<Pixel> CompressedPixels = new List<Pixel>(pixels.Length);

                Pixel CurrentPixel = null;
                for (Pos.X = 0; Pos.X < Dimensions.X; Pos.X++)
                {
                    for (Pos.Y = 0; Pos.Y < Dimensions.Y; Pos.Y++)
                    {
                        Pixel Current = pixels.GetCurrent(Pos);

                        bool CountPixel =
                            Current != null && //this pixel has to exist
                            Current.Equals(pixels.GetCurrent(Pos.Transform(new IntVector2() { X = 0, Y = -1 })), tolerance) && // the last one has to exist and be the same
                            Current.Color.Equals(CurrentPixel.Color, tolerance) &&
                            !(_Settings.isBlackEmpty && Current.Color.isBlackOrEmpty(0.01f)) &&  //stop immediatly cunt
                            (CurrentPixel.Scale.Y < _Settings.maxPixelLength); //hehe

                        if (CountPixel) //this vs last, color, width, existance
                        {
                            CurrentPixel.AddHeight();
                        }
                        else
                        {
                            //add and reset
                            if (CurrentPixel != null && !(_Settings.isBlackEmpty && CurrentPixel.Color.isBlackOrEmpty(0.01f))) CompressedPixels.Add(CurrentPixel);

                            CurrentPixel = Current;
                        }
                    }
                }
                if (CurrentPixel != null) CompressedPixels.Add(CurrentPixel);
                return CompressedPixels.ToArray();
            }

        }
    }

    public class ImageSettings
    {
        public ImageSettings(bool isBlackEmpty, float scale, float thicc, bool track, bool centered, float spread, float alfa, BeatMap.Obstacle baseWall)
        {
            this.isBlackEmpty = isBlackEmpty;
            this.scale = scale;
            this.thicc = thicc;
            this.centered = centered;
            this.spread = spread;
            this.alfa = alfa;
            this.Wall = baseWall;
        }
        public ImageSettings() { }

        public bool isBlackEmpty { get; set; }
        public float scale { get; set; }
        public float thicc { get; set; }
        public bool track { get; set; }
        public int maxPixelLength { get; set; }
        public bool centered { get; set; }
        public float spread { get; set; }
        public float alfa { get; set; }
        public float shift { get; set; }
        public float tolerance { get; set; }
        public BeatMap.Obstacle Wall { get; set; }
    } //settings

    class Pixel
    {
        //data type
        public IntVector2 Position { get; set; }
        public IntVector2 Scale { get; set; }
        public Color Color { get; set; }
        public Pixel FlipVertical(Bitmap bitmap)
        {
            return new Pixel()
            {
                Position = new IntVector2() { X = Position.X, Y = bitmap.Height - Position.Y - Scale.Y },
                Scale = Scale,
                Color = Color
            };
        }
        public Pixel Inverse()
        {
            return new Pixel()
            {
                Position = new IntVector2() { X = Position.Y, Y = Position.X },
                Scale = new IntVector2() { X = Scale.Y, Y = Scale.X },
                Color = Color
            };
        }
        public void AddHeight()
        {
            Scale = Scale.Transform(new IntVector2() { X = 0, Y = 1 });
        }
        public bool Equals(Pixel pixel, float tolerance)
        {
            if (pixel == null) return false;
            if (!Color.Equals(pixel.Color, tolerance)) return false;
            if (Scale.X != pixel.Scale.X) return false;
            return true;
        }
        public FloatingPixel ToFloating()
        {
            return new FloatingPixel()
            {
                Position = new Vector2() { X = Position.X.toFloat(), Y = Position.Y.toFloat() },
                Scale = new Vector2() { X = Scale.X.toFloat(), Y = Scale.Y.toFloat() },
                Color = Color
            };
        }

    }

    class FloatingPixel
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; }

        //0,0 origin
        public FloatingPixel Resize(float Factor)
        {
            return new FloatingPixel()
            {
                Position = new Vector2()
                {
                    X = Position.X * Factor,
                    Y = Position.Y * Factor,
                },
                Scale = new Vector2()
                {
                    X = Scale.X * Factor,
                    Y = Scale.Y * Factor,
                },
                Color = Color
            };
        }
        public FloatingPixel Transform(Vector2 tranformation)
        {
            return new FloatingPixel()
            {
                Position = new Vector2()
                {
                    X = Position.X + tranformation.X,
                    Y = Position.Y + tranformation.Y,
                },
                Scale = Scale,
                Color = Color
            };
        }
    }

    static class BitmapHelper
    {
        public static Pixel GetCurrent(this Pixel[] pixels, IntVector2 pos)
        {
            if (!pixels.Any(p => p.Position.X == pos.X && p.Position.Y == pos.Y)) return null; //if it dont exist
            //DateTime dateTime = DateTime.Now;
            Pixel thing = pixels.Where(p => p.Position.Y == pos.Y && p.Position.X == pos.X).First();
            //Console.WriteLine(DateTime.Now - dateTime);
            return thing;
            //  System.Threading.Tasks.Parallel.
        }
        public static Color GetCurrent(this Bitmap bitmap, IntVector2 pos)
        {
            return new Color()
            {
                R = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).R) / 255f,
                G = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).G) / 255f,
                B = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).B) / 255f,
                A = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).A) / 255f
            };
        }
        public static Pixel ToPixel(this Bitmap bitmap, IntVector2 pos)
        {
            return new Pixel()
            {
                Position = pos,
                Scale = new IntVector2() { X = 1, Y = 1 },
                Color = bitmap.GetCurrent(pos)
            };
        }
        public static IntVector2 GetDimensions(this Pixel[] pixels)
        {
            if (pixels.Length == 0) return new IntVector2() { X = 0, Y = 0 };
            Pixel[] Sorted = pixels.OrderBy(p => p.Position.Y).ThenBy(p => p.Position.X).ToArray();
            return new IntVector2()
            {
                Y = Sorted.Last().Position.Y + Sorted.Last().Scale.Y,
                X = Sorted.Last().Position.X + Sorted.Last().Scale.X
            };

        }
        public static IntVector2 Transform(this IntVector2 t1, IntVector2 t2)
        {
            return new IntVector2()
            {
                X = t1.X + t2.X,
                Y = t1.Y + t2.Y
            };
        }

        //text to wall things
    }

}
